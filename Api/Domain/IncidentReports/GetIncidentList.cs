using AutoMapper;
using AutoMapper.QueryableExtensions;
using MediatR;
using Microsoft.EntityFrameworkCore;
using Stronghold.AppDashboard.Api.Authorization;
using Stronghold.AppDashboard.Api.Models;
using Stronghold.AppDashboard.Data;
using Stronghold.AppDashboard.Shared.Enumerations;

namespace Stronghold.AppDashboard.Api.Domain.IncidentReports;

[AllowedAuthorizationRole(AuthorizationRole.Administrator, AuthorizationRole.User)]
public class GetIncidentList : IRequest<List<IncidentReportListItem>>
{
    public string? SearchTerm { get; set; }
    public Guid? CompanyId { get; set; }
    public Guid? RegionId { get; set; }
    public string? Status { get; set; }
    public DateTime? DateFrom { get; set; }
    public DateTime? DateTo { get; set; }
}

public class GetIncidentListHandler : IRequestHandler<GetIncidentList, List<IncidentReportListItem>>
{
    private readonly AppDbContext _context;
    private readonly IMapper _mapper;

    public GetIncidentListHandler(AppDbContext context, IMapper mapper)
    {
        _context = context;
        _mapper = mapper;
    }

    public async Task<List<IncidentReportListItem>> Handle(GetIncidentList request, CancellationToken cancellationToken)
    {
        var query = _context.IncidentReports
            .Include(r => r.Company)
            .Include(r => r.Region)
            .AsQueryable();

        if (request.CompanyId.HasValue)
            query = query.Where(r => r.CompanyId == request.CompanyId);

        if (request.RegionId.HasValue)
            query = query.Where(r => r.RegionId == request.RegionId);

        if (!string.IsNullOrWhiteSpace(request.Status))
            query = query.Where(r => r.Status == request.Status);

        if (request.DateFrom.HasValue)
            query = query.Where(r => r.IncidentDate >= request.DateFrom.Value);

        if (request.DateTo.HasValue)
            query = query.Where(r => r.IncidentDate <= request.DateTo.Value);

        if (!string.IsNullOrWhiteSpace(request.SearchTerm))
        {
            var term = request.SearchTerm.ToLower();
            query = query.Where(r =>
                (r.IncidentNumber != null && r.IncidentNumber.ToLower().Contains(term)) ||
                (r.JobNumber != null && r.JobNumber.ToLower().Contains(term)) ||
                (r.ClientCode != null && r.ClientCode.ToLower().Contains(term)) ||
                (r.IncidentSummary != null && r.IncidentSummary.ToLower().Contains(term)));
        }

        var results = await query
            .OrderByDescending(r => r.IncidentDate)
            .ToListAsync(cancellationToken);

        return _mapper.Map<List<IncidentReportListItem>>(results);
    }
}
