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
            .FirstOrDefaultAsync(r => r.Id == request.IncidentReportId, cancellationToken);

        if (entity == null)
            return null;

        _context.IncidentReports.Remove(entity);
        await _context.SaveChangesAsync(cancellationToken);

        return true;
    }
}
