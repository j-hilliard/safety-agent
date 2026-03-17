using MediatR;
using Microsoft.EntityFrameworkCore;
using Stronghold.AppDashboard.Api.Authorization;
using Stronghold.AppDashboard.Data;
using Stronghold.AppDashboard.Shared.Enumerations;

namespace Stronghold.AppDashboard.Api.Domain.ReferenceData;

[AllowedAuthorizationRole(AuthorizationRole.Administrator, AuthorizationRole.User)]
public class DeleteRegion : IRequest<Unit>
{
    public Guid Id { get; set; }
}

public class DeleteRegionHandler : IRequestHandler<DeleteRegion, Unit>
{
    private readonly AppDbContext _context;

    public DeleteRegionHandler(AppDbContext context)
    {
        _context = context;
    }

    public async Task<Unit> Handle(DeleteRegion request, CancellationToken cancellationToken)
    {
        var entity = await _context.Regions
            .FirstOrDefaultAsync(r => r.Id == request.Id, cancellationToken)
            ?? throw new KeyNotFoundException($"Region {request.Id} not found.");

        entity.IsActive = false;
        entity.UpdatedAt = DateTime.UtcNow;
        await _context.SaveChangesAsync(cancellationToken);
        return Unit.Value;
    }
}
