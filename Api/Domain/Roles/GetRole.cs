using AutoMapper;
using MediatR;
using Microsoft.EntityFrameworkCore;
using Stronghold.AppDashboard.Api.Authorization;
using Stronghold.AppDashboard.Api.Models;
using Stronghold.AppDashboard.Data;
using Stronghold.AppDashboard.Shared.Enumerations;

namespace Stronghold.AppDashboard.Api.Domain.Roles;

[AllowedAuthorizationRole(AuthorizationRole.Administrator)]
public class GetRole : IRequest<Role?>
{
    public int RoleId { get; set; }
}

public class GetRoleHandler : IRequestHandler<GetRole, Role?>
{
    private readonly AppDbContext _context;
    private readonly IMapper _mapper;

    public GetRoleHandler(AppDbContext context, IMapper mapper)
    {
        _context = context;
        _mapper = mapper;
    }

    public async Task<Role?> Handle(GetRole request, CancellationToken cancellationToken)
    {
        var roles = await _context.Roles.ToListAsync(cancellationToken);
        return _mapper.Map<Role>(roles);
    }
}
