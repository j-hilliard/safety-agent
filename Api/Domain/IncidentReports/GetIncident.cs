using AutoMapper;
using MediatR;
using Microsoft.EntityFrameworkCore;
using Stronghold.AppDashboard.Api.Authorization;
using Stronghold.AppDashboard.Api.Models;
using Stronghold.AppDashboard.Data;
using Stronghold.AppDashboard.Shared.Enumerations;

namespace Stronghold.AppDashboard.Api.Domain.IncidentReports;

[AllowedAuthorizationRole(AuthorizationRole.Administrator, AuthorizationRole.User)]
public class GetIncident : IRequest<IncidentReport?>
{
    public Guid IncidentReportId { get; set; }
}

public class GetIncidentHandler : IRequestHandler<GetIncident, IncidentReport?>
{
    private readonly AppDbContext _context;
    private readonly IMapper _mapper;

    public GetIncidentHandler(AppDbContext context, IMapper mapper)
    {
        _context = context;
        _mapper = mapper;
    }

    public async Task<IncidentReport?> Handle(GetIncident request, CancellationToken cancellationToken)
    {
        var entity = await _context.IncidentReports
            .Include(r => r.Company)
            .Include(r => r.Region)
            .Include(r => r.EmployeesInvolved)
            .Include(r => r.Actions)
            .Include(r => r.References)
            .FirstOrDefaultAsync(r => r.Id == request.IncidentReportId, cancellationToken);

        return entity == null ? null : _mapper.Map<IncidentReport>(entity);
    }
}
