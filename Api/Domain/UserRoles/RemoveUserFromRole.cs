using AutoMapper;
using MediatR;
using Microsoft.EntityFrameworkCore;
using Stronghold.AppDashboard.Api.Authorization;
using Stronghold.AppDashboard.Data;
using Stronghold.AppDashboard.Shared.Enumerations;

namespace Stronghold.AppDashboard.Api.Domain.UserRoles;

[AllowedAuthorizationRole(AuthorizationRole.Administrator)]
public class RemoveUserFromRole : IRequest<bool?>
{
    public int UserId { get; set; }
    public int RoleId { get; set; }
}

public class RemoveUserFromRoleHandler : IRequestHandler<RemoveUserFromRole, bool?>
{
    private readonly AppDbContext _context;
    private readonly IMapper _mapper;

    public RemoveUserFromRoleHandler(AppDbContext context, IMapper mapper)
    {
        _context = context;
        _mapper = mapper;
    }

    public async Task<bool?> Handle(RemoveUserFromRole request, CancellationToken cancellationToken)
    {
        var userRole = await _context.UserRoles.FirstOrDefaultAsync(
            userRole => userRole.UserId == request.UserId && userRole.RoleId == request.RoleId,
            cancellationToken
        );
        if (userRole == null)
            return null;

        _context.UserRoles.Remove(userRole);
        await _context.SaveChangesAsync(cancellationToken);

        return true;
    }
}
