using AutoMapper;
using MediatR;
using Microsoft.EntityFrameworkCore;
using Stronghold.AppDashboard.Api.Authorization;
using Stronghold.AppDashboard.Data;
using Stronghold.AppDashboard.Shared.Enumerations;

namespace Stronghold.AppDashboard.Api.Domain.Roles;

[AllowedAuthorizationRole(AuthorizationRole.Administrator)]
public class DeleteRole : IRequest<bool?>
{
    public int RoleId { get; set; }
}

public class DeleteRoleHandler : IRequestHandler<DeleteRole, bool?>
{
    private readonly AppDbContext _context;
    private readonly IMapper _mapper;

    public DeleteRoleHandler(AppDbContext context, IMapper mapper)
    {
        _context = context;
        _mapper = mapper;
    }

    public async Task<bool?> Handle(DeleteRole request, CancellationToken cancellationToken)
    {
        await using var transaction = await _context.Database.BeginTransactionAsync(
            cancellationToken
        );

        try
        {
            var role = await _context.Roles.FirstOrDefaultAsync(
                role => role.RoleId == request.RoleId,
                cancellationToken
            );
            if (role == null)
                return null;

            // When deleting a role, first check to see if any users are assigned to the role.
            // If there are users assigned to the role, remove those users from the role first, then proceed with removing the role.
            List<Data.Models.UserRole>? assignedUserRole = await _context
                .UserRoles.Where(userRole => userRole.RoleId == role.RoleId)
                .ToListAsync(cancellationToken);

            if (assignedUserRole.Count > 0)
            {
                foreach (Data.Models.UserRole userRole in assignedUserRole)
                {
                    _context.UserRoles.Remove(userRole);
                }
            }

            _context.Roles.Remove(role);
            await _context.SaveChangesAsync(cancellationToken);

            await transaction.CommitAsync(cancellationToken);
            return true;
        }
        catch (Exception)
        {
            await transaction.RollbackAsync(cancellationToken);
            throw;
        }
    }
}
