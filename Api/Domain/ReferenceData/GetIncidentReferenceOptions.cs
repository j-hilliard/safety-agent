using AutoMapper;
using MediatR;
using Microsoft.EntityFrameworkCore;
using Stronghold.AppDashboard.Api.Authorization;
using Stronghold.AppDashboard.Api.Models;
using Stronghold.AppDashboard.Data;
using Stronghold.AppDashboard.Shared.Enumerations;

namespace Stronghold.AppDashboard.Api.Domain.ReferenceData;

[AllowedAuthorizationRole(AuthorizationRole.Administrator, AuthorizationRole.User)]
public class GetIncidentReferenceOptions : IRequest<Dictionary<string, List<RefOptionDto>>> { }

public class GetIncidentReferenceOptionsHandler : IRequestHandler<GetIncidentReferenceOptions, Dictionary<string, List<RefOptionDto>>>
{
    private readonly AppDbContext _context;
    private readonly IMapper _mapper;

    public GetIncidentReferenceOptionsHandler(AppDbContext context, IMapper mapper)
    {
        _context = context;
        _mapper = mapper;
    }

    public async Task<Dictionary<string, List<RefOptionDto>>> Handle(GetIncidentReferenceOptions request, CancellationToken cancellationToken)
    {
        var options = await _context.IncidentReportReferenceOptions
            .Include(r => r.ReferenceType)
            .Where(r => r.IsActive && r.ReferenceType.AppliesTo == "incident_report")
            .OrderBy(r => r.Name)
            .ToListAsync(cancellationToken);

        var mapped = _mapper.Map<List<RefOptionDto>>(options);

        return mapped
            .GroupBy(o => o.ReferenceTypeCode)
            .ToDictionary(g => g.Key, g => g.ToList());
    }
}
