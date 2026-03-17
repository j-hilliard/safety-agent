using AutoMapper;
using MediatR;
using Microsoft.EntityFrameworkCore;
using Microsoft.Extensions.Caching.Memory;
using Stronghold.AppDashboard.Api.Models;
using Stronghold.AppDashboard.Api.Services;
using Stronghold.AppDashboard.Data;
using Stronghold.AppDashboard.Shared.Enumerations;

namespace Stronghold.AppDashboard.Api.Authorization;

public class AuthorizationBehavior<TRequest, TResponse> : IPipelineBehavior<TRequest, TResponse>
    where TRequest : notnull
{
    private readonly IMapper _mapper;
    private readonly IMemoryCache _cache;
    private readonly AppDbContext _appDbContext;
    private readonly IHttpContextAccessor _httpContextAccessor;
    private readonly ILogger<AuthorizationBehavior<TRequest, TResponse>> _logger;
    private readonly AzureAdHelper _azureAdHeler;
    private readonly IWebHostEnvironment _env;

    public AuthorizationBehavior(
        IMapper mapper,
        IMemoryCache cache,
        AzureAdHelper azureAdHelper,
        AppDbContext appDbContext,
        IHttpContextAccessor httpContextAccessor,
        ILogger<AuthorizationBehavior<TRequest, TResponse>> logger,
        IWebHostEnvironment env
    )
    {
        _mapper = mapper;
        _cache = cache;
        _azureAdHeler = azureAdHelper;
        _appDbContext = appDbContext;
        _httpContextAccessor = httpContextAccessor;
        _logger = logger;
        _env = env;
    }

    public async Task<TResponse> Handle(
        TRequest request,
        RequestHandlerDelegate<TResponse> next,
        CancellationToken cancellationToken
    )
    {
        if (_env.EnvironmentName == "Local")
        {
            return await next();
        }

        var httpContext = _httpContextAccessor.HttpContext;
        if (
            httpContext == null
            || httpContext.User == null
            || httpContext.User.Identity?.IsAuthenticated != true
        )
        {
            throw new UnauthorizedAccessException("User is not authenticated");
        }

        var azureAdObjectId = httpContext
            .User.FindFirst("http://schemas.microsoft.com/identity/claims/objectidentifier")
            ?.Value;
        if (string.IsNullOrEmpty(azureAdObjectId))
        {
            throw new UnauthorizedAccessException("User ID not valid");
        }
        if (!Guid.TryParse(azureAdObjectId, out Guid guidAzureAdObjectId))
        {
            throw new UnauthorizedAccessException("User ID not valid");
        }

        var cacheKey = $"UserRoles_{azureAdObjectId}";
        if (!_cache.TryGetValue(cacheKey, out List<string>? roles) || roles == null)
        {
            if (roles == null || roles.Count == 0) // no previously cached roles were found for this user so load them from database and cache them
            {
                // lets first check if the user who is logged in has a user account saved in the sql database
                var existingUser = await _appDbContext.Users.FirstOrDefaultAsync(
                    u => u.AzureAdObjectId == guidAzureAdObjectId,
                    cancellationToken
                );
                if (existingUser == null)
                {
                    // no existing user account is saved in sql so lets create one and save it.  We will also add the user to the user role as well.


                    var adUserInfo = await _azureAdHeler.GetAdUserInfoByObjectIdAsync(
                        guidAzureAdObjectId
                    );
                    if (
                        adUserInfo != null
                        && adUserInfo.AzureAdObjectId != null
                        && adUserInfo.AzureAdObjectId == guidAzureAdObjectId
                    )
                    {
                        var newUser = new User
                        {
                            AzureAdObjectId = guidAzureAdObjectId,
                            FirstName = adUserInfo.FirstName,
                            LastName = adUserInfo.LastName,
                            Email = adUserInfo.Email,
                            Company = adUserInfo.Company,
                            Department = adUserInfo.Department,
                            Title = adUserInfo.Title,
                            Active = true,
                            CreatedOn = DateTime.UtcNow,
                            LastLogin = DateTime.UtcNow,
                        };

                        var user = _mapper.Map<Data.Models.User>(newUser);
                        await _appDbContext.Users.AddAsync(user, cancellationToken);
                        await _appDbContext.SaveChangesAsync(cancellationToken);

                        if (user.UserId > 0)
                        {
                            // adding user to the user role
                            var userRole = await _appDbContext.Roles.FirstOrDefaultAsync(
                                r => r.Name == AuthorizationRoles.User,
                                cancellationToken
                            );
                            if (userRole != null)
                            {
                                var newUserRole = new UserRole();
                                newUserRole.UserId = user.UserId;
                                newUserRole.RoleId = userRole.RoleId;

                                await _appDbContext.UserRoles.AddAsync(
                                    _mapper.Map<Data.Models.UserRole>(newUserRole),
                                    cancellationToken
                                );
                                await _appDbContext.SaveChangesAsync(cancellationToken);
                            }
                        }
                    }
                    ;
                }

                // user has exsting account in the database so lets query which roles they are in and cache them
                /* roles = await _appDbContext.UserRoles
                .Where(ur => ur.User.AzureAdObjectId == guidAzureAdObjectId)
                .Select(ur => ur.Role.Name)
                .ToListAsync(cancellationToken); */


                var currentUser = await _appDbContext
                    .Users.Where(user => user.AzureAdObjectId == new Guid(azureAdObjectId))
                    .FirstOrDefaultAsync(cancellationToken);
                if (currentUser == null)
                {
                    throw new UnauthorizedAccessException("User not found in the database");
                }

                roles = _appDbContext
                    .UserRoles.Where(userRole => userRole.UserId == currentUser.UserId)
                    .Select(userRole => userRole.Role.Name) // Select the Role object
                    .ToList();

                if (!roles.Contains(AuthorizationRoles.User)) // lets check if the user has the default User level role assigned, if not then we will add it as default level access.
                    roles.Add(AuthorizationRoles.User); // Add the user role as a default role for all users.

                var cacheOptions = new MemoryCacheEntryOptions().SetAbsoluteExpiration(
                    TimeSpan.FromMinutes(60)
                );
                _cache.Set(cacheKey, roles, cacheOptions);
            }
        }

        if (request == null)
        {
            throw new ArgumentNullException(nameof(request), "Request cannot be null");
        }

        var attribute =
            Attribute.GetCustomAttribute(request.GetType(), typeof(AllowedAuthorizationRole))
            as AllowedAuthorizationRole;
        if (attribute == null)
        {
            throw new UnauthorizedAccessException(
                $"Allowed Authorization Role is not specified on the requested Api function called '{typeof(TRequest).Name}'"
            );
        }

        if (attribute.IsAllowed(roles))
        {
            return await next();
        }

        throw new UnauthorizedAccessException(
            $"User is not authorized to perform '{typeof(TRequest).Name}'"
        );
    }
}

public interface IRoleRequiredRequest
{
    string RequiredRole { get; }
}
