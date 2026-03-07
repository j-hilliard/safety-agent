using AutoMapper;
using MediatR;
using Microsoft.EntityFrameworkCore;
using Stronghold.AppDashboard.Api.Authorization;
using Stronghold.AppDashboard.Api.Models;
using Stronghold.AppDashboard.Data;
using Stronghold.AppDashboard.Shared.Enumerations;

namespace Stronghold.AppDashboard.Api.Domain.Users;

[AllowedAuthorizationRole(
    AuthorizationRole.Administrator,
    AuthorizationRole.User,
    AuthorizationRole.AuthenticatedUser
)]
public class GetUserByAzureAdObjectId : IRequest<User?>
{
    public Guid AzureAdObjectId { get; set; }
}

public class GetUserByAzureAdObjectIdHandler : IRequestHandler<GetUserByAzureAdObjectId, User?>
{
    private readonly AppDbContext _context;
    private readonly IMapper _mapper;

    public GetUserByAzureAdObjectIdHandler(AppDbContext context, IMapper mapper)
    {
        _context = context;
        _mapper = mapper;
    }

    public async Task<User?> Handle(
        GetUserByAzureAdObjectId request,
        CancellationToken cancellationToken
    )
    {
        var user = await _context.Users.FirstOrDefaultAsync(
            u => u.AzureAdObjectId == request.AzureAdObjectId,
            cancellationToken
        );
        if (user == null)
        {
            return null;
        }

        User userResult = _mapper.Map<User>(user);

        var roles = await _context
            .UserRoles.Where(ur => ur.UserId == user.UserId)
            .Include(ur => ur.Role)
            .ToListAsync(cancellationToken);

        userResult.Roles = _mapper.Map<List<UserRole>>(roles);

        return userResult;
    }
}
