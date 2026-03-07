using AutoMapper;
using MediatR;
using Microsoft.EntityFrameworkCore;
using Stronghold.AppDashboard.Api.Authorization;
using Stronghold.AppDashboard.Api.Models;
using Stronghold.AppDashboard.Data;
using Stronghold.AppDashboard.Shared.Enumerations;

namespace Stronghold.AppDashboard.Api.Domain.Roles;

[AllowedAuthorizationRole(AuthorizationRole.Administrator, AuthorizationRole.User)]
public class GetAllRoles : IRequest<List<Role>> { }

public class GetAllRolesHandler : IRequestHandler<GetAllRoles, List<Role>>
{
    private readonly AppDbContext _context;
    private readonly IMapper _mapper;

    public GetAllRolesHandler(AppDbContext context, IMapper mapper)
    {
        _context = context;
        _mapper = mapper;
    }

    public async Task<List<Role>> Handle(GetAllRoles request, CancellationToken cancellationToken)
    {
        var roles = await _context.Roles.ToListAsync(cancellationToken);
        return _mapper.Map<List<Role>>(roles);
    }
}
