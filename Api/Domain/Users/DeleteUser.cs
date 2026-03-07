using MediatR;
using Microsoft.EntityFrameworkCore;
using Stronghold.AppDashboard.Api.Authorization;
using Stronghold.AppDashboard.Data;
using Stronghold.AppDashboard.Shared.Enumerations;

namespace Stronghold.AppDashboard.Api.Domain.Users;

[AllowedAuthorizationRole(AuthorizationRole.Administrator)]
public class DeleteUser : IRequest<bool?>
{
    public int UserId { get; set; }
}

public class DeleteUserHandler : IRequestHandler<DeleteUser, bool?>
{
    private readonly AppDbContext _context;

    public DeleteUserHandler(AppDbContext context)
    {
        _context = context;
    }

    public async Task<bool?> Handle(DeleteUser request, CancellationToken cancellationToken)
    {
        try
        {
            var user = await _context.Users.FirstOrDefaultAsync(
                user => user.UserId == request.UserId,
                cancellationToken
            );
            if (user == null)
                return null;

            // When deleting a role, first check to see if any users are assigned to the role.
            // If there are users assigned to the role, remove those users from the role first, then proceed with removing the role.
            List<Data.Models.UserRole>? assignedUserRole = await _context
                .UserRoles.Where(userRole => userRole.UserId == request.UserId)
                .ToListAsync(cancellationToken);

            if (assignedUserRole.Count > 0)
            {
                foreach (Data.Models.UserRole userRole in assignedUserRole)
                {
                    _context.UserRoles.Remove(userRole);
                }
            }

            _context.Users.Remove(user);
            await _context.SaveChangesAsync(cancellationToken);
            return true;
        }
        catch (Exception)
        {
            throw;
        }
    }
}
