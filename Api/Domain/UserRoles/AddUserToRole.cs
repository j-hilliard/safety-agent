using AutoMapper;
using MediatR;
using Stronghold.AppDashboard.Api.Authorization;
using Stronghold.AppDashboard.Api.Models;
using Stronghold.AppDashboard.Data;
using Stronghold.AppDashboard.Shared.Enumerations;

namespace Stronghold.AppDashboard.Api.Domain.UserRoles;

[AllowedAuthorizationRole(AuthorizationRole.Administrator)]
public class AddUserToRole : IRequest<UserRole>
{
    public UserRole UserToAddToRole { get; set; } = null!;
}

public class AddUserToRoleHandler : IRequestHandler<AddUserToRole, UserRole>
{
    private readonly AppDbContext _context;
    private readonly IMapper _mapper;

    public AddUserToRoleHandler(AppDbContext context, IMapper mapper)
    {
        _context = context;
        _mapper = mapper;
    }

    public async Task<UserRole> Handle(AddUserToRole request, CancellationToken cancellationToken)
    {
        var userRole = _mapper.Map<Data.Models.UserRole>(request.UserToAddToRole);
        var existingUser = await _context.Users.FindAsync(
            new object[] { request.UserToAddToRole.UserId },
            cancellationToken
        );
        var existingRole = await _context.Roles.FindAsync(
            new object[] { request.UserToAddToRole.RoleId },
            cancellationToken
        );

        if (existingUser == null || existingRole == null)
        {
            throw new Exception("User or Role does not exist.");
        }

        userRole.User = existingUser;
        userRole.Role = existingRole;

        await _context.UserRoles.AddAsync(userRole, cancellationToken);
        await _context.SaveChangesAsync(cancellationToken);

        return _mapper.Map<UserRole>(userRole);
    }
}
