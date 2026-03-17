using MediatR;
using Microsoft.EntityFrameworkCore;
using Stronghold.AppDashboard.Api.Authorization;
using Stronghold.AppDashboard.Data;
using Stronghold.AppDashboard.Shared.Enumerations;

namespace Stronghold.AppDashboard.Api.Domain.IncidentReports;

[AllowedAuthorizationRole(AuthorizationRole.Administrator)]
public class DeleteIncident : IRequest<bool?>
{
    public Guid IncidentReportId { get; set; }
}

public class DeleteIncidentHandler : IRequestHandler<DeleteIncident, bool?>
{
    private readonly AppDbContext _context;

    public DeleteIncidentHandler(AppDbContext context)
    {
        _context = context;
    }

    public async Task<bool?> Handle(DeleteIncident request, CancellationToken cancellationToken)
    {
        var entity = await _context.IncidentReports
            .Include(r => r.EmployeesInvolved)
            .Include(r => r.Actions)
            .Include(r => r.References)
            .FirstOrDefaultAsync(r => r.Id == request.IncidentReportId, cancellationToken);

        if (entity == null)
            return null;

        // Null out process log FK references (nullable) rather than deleting audit trail
        await _context.ProcessLogs
            .Where(l => l.IncidentReportId == request.IncidentReportId)
            .ExecuteUpdateAsync(s => s.SetProperty(l => l.IncidentReportId, (Guid?)null), cancellationToken);

        _context.IncidentReports.Remove(entity);
        await _context.SaveChangesAsync(cancellationToken);

        return true;
    }
}
