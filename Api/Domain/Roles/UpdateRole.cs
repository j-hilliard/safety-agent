using AutoMapper;
using MediatR;
using Stronghold.AppDashboard.Api.Authorization;
using Stronghold.AppDashboard.Api.Models;
using Stronghold.AppDashboard.Data;
using Stronghold.AppDashboard.Shared.Enumerations;

namespace Stronghold.AppDashboard.Api.Domain.Roles;

[AllowedAuthorizationRole(AuthorizationRole.Administrator)]
public class UpdateRole : IRequest<Role?>
{
    public int RoleId { get; set; }
    public Role RoleUpdate { get; set; } = null!;
}

public class UpdateRoleHandler : IRequestHandler<UpdateRole, Role?>
{
    private readonly AppDbContext _context;
    private readonly IMapper _mapper;

    public UpdateRoleHandler(AppDbContext context, IMapper mapper)
    {
        _context = context;
        _mapper = mapper;
    }

    public async Task<Role?> Handle(UpdateRole request, CancellationToken cancellationToken)
    {
        var role = await _context.Roles.FindAsync(request.RoleId);
        if (role == null)
            return null;

        _mapper.Map(request.RoleUpdate, role);
        await _context.SaveChangesAsync(cancellationToken);

        return _mapper.Map<Role>(role);
    }
}
