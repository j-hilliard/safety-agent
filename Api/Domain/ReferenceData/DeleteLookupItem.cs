using MediatR;
using Microsoft.EntityFrameworkCore;
using Stronghold.AppDashboard.Api.Authorization;
using Stronghold.AppDashboard.Data;
using Stronghold.AppDashboard.Shared.Enumerations;

namespace Stronghold.AppDashboard.Api.Domain.ReferenceData;

[AllowedAuthorizationRole(AuthorizationRole.Administrator, AuthorizationRole.User)]
public class DeleteLookupItem : IRequest<Unit>
{
    public Guid Id { get; set; }
}

public class DeleteLookupItemHandler : IRequestHandler<DeleteLookupItem, Unit>
{
    private readonly AppDbContext _context;

    public DeleteLookupItemHandler(AppDbContext context)
    {
        _context = context;
    }

    public async Task<Unit> Handle(DeleteLookupItem request, CancellationToken cancellationToken)
    {
        var entity = await _context.IncidentReportReferenceOptions
            .FirstOrDefaultAsync(r => r.Id == request.Id, cancellationToken)
            ?? throw new KeyNotFoundException($"Lookup item {request.Id} not found.");

        entity.IsActive = false;
        entity.UpdatedAt = DateTime.UtcNow;
        await _context.SaveChangesAsync(cancellationToken);
        return Unit.Value;
    }
}
