using AutoMapper;
using MediatR;
using Microsoft.EntityFrameworkCore;
using Stronghold.AppDashboard.Api.Authorization;
using Stronghold.AppDashboard.Api.Models;
using Stronghold.AppDashboard.Data;
using Stronghold.AppDashboard.Shared.Enumerations;

namespace Stronghold.AppDashboard.Api.Domain;

[AllowedAuthorizationRole(AuthorizationRole.Administrator)]
public class GetCurrentUser : IRequest<User?> { }

public class GetCurrentUserHandler : IRequestHandler<GetCurrentUser, User?>
{
    private readonly IHttpContextAccessor _httpContextAccessor;
    private readonly AppDbContext _context;
    private readonly IMapper _mapper;

    public GetCurrentUserHandler(
        IHttpContextAccessor httpContextAccessor,
        AppDbContext context,
        IMapper mapper
    )
    {
        _httpContextAccessor = httpContextAccessor;
        _context = context;
        _mapper = mapper;
    }

    public async Task<User?> Handle(GetCurrentUser request, CancellationToken cancellationToken)
    {
        var contextUser = _httpContextAccessor.HttpContext?.User;

        if (
            contextUser == null
            || contextUser.Identity == null
            || contextUser.Identity.IsAuthenticated == false
        )
            throw new InvalidOperationException("No logged-in user found.");

        var azureAdObjectId = contextUser
            .FindFirst("http://schemas.microsoft.com/identity/claims/objectidentifier")
            ?.Value;

        if (azureAdObjectId == null)
            throw new InvalidOperationException("No logged-in user found.");

        var currentUser = await _context
            .Users.Where(u => u.AzureAdObjectId == new Guid(azureAdObjectId))
            .FirstOrDefaultAsync(cancellationToken);

        if (currentUser == null)
            throw new InvalidOperationException("User not found.");

        return _mapper.Map<User?>(currentUser);
    }
}
