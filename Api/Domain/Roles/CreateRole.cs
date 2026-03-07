using AutoMapper;
using MediatR;
using Stronghold.AppDashboard.Api.Authorization;
using Stronghold.AppDashboard.Api.Models;
using Stronghold.AppDashboard.Data;
using Stronghold.AppDashboard.Shared.Enumerations;

namespace Stronghold.AppDashboard.Api.Domain.Roles;

[AllowedAuthorizationRole(AuthorizationRole.Administrator)]
public class CreateRole : IRequest<Role>
{
    public Role RoleToCreate { get; set; } = null!;
}

public class CreateRoleHandler : IRequestHandler<CreateRole, Role>
{
    private readonly AppDbContext _context;
    private readonly IMapper _mapper;

    public CreateRoleHandler(AppDbContext context, IMapper mapper)
    {
        _context = context;
        _mapper = mapper;
    }

    public async Task<Models.Role> Handle(CreateRole request, CancellationToken cancellationToken)
    {
        var role = _mapper.Map<Data.Models.Role>(request.RoleToCreate);

        await _context.Roles.AddAsync(role, cancellationToken);
        await _context.SaveChangesAsync(cancellationToken);

        return _mapper.Map<Role>(role);
    }
}
