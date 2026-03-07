using AutoMapper;
using MediatR;
using Microsoft.EntityFrameworkCore;
using Stronghold.AppDashboard.Api.Authorization;
using Stronghold.AppDashboard.Api.Models;
using Stronghold.AppDashboard.Data;
using Stronghold.AppDashboard.Shared.Enumerations;

namespace Stronghold.AppDashboard.Api.Domain.UserRoles;

[AllowedAuthorizationRole(AuthorizationRole.Administrator)]
public class GetAllUsersInRole : IRequest<List<User>>
{
    public int RoleId { get; set; }
}

public class GetAllUsersInRoleHandler : IRequestHandler<GetAllUsersInRole, List<User>>
{
    private readonly AppDbContext _context;
    private readonly IMapper _mapper;

    public GetAllUsersInRoleHandler(AppDbContext context, IMapper mapper)
    {
        _context = context;
        _mapper = mapper;
    }

    public async Task<List<User>> Handle(
        GetAllUsersInRole request,
        CancellationToken cancellationToken
    )
    {
        var userRoles = await _context
            .UserRoles.Include(ur => ur.User)
            .Where(ur => ur.RoleId == request.RoleId)
            .ToListAsync(cancellationToken);

        var users = userRoles.Select(ur => ur.User).ToList();

        return _mapper.Map<List<User>>(users);
    }
}
