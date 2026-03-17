using AutoMapper;
using MediatR;
using Microsoft.EntityFrameworkCore;
using Stronghold.AppDashboard.Api.Authorization;
using Stronghold.AppDashboard.Api.Models;
using Stronghold.AppDashboard.Data;
using Stronghold.AppDashboard.Data.Models.Safety;
using Stronghold.AppDashboard.Shared.Enumerations;

namespace Stronghold.AppDashboard.Api.Domain.ReferenceData;

[AllowedAuthorizationRole(AuthorizationRole.Administrator, AuthorizationRole.User)]
public class GetSeverities : IRequest<List<RefSeverityDto>> { }

public class GetSeveritiesHandler : IRequestHandler<GetSeverities, List<RefSeverityDto>>
{
    private readonly AppDbContext _context;
    private readonly IMapper _mapper;

    public GetSeveritiesHandler(AppDbContext context, IMapper mapper)
    {
        _context = context;
        _mapper = mapper;
    }

    public async Task<List<RefSeverityDto>> Handle(GetSeverities request, CancellationToken cancellationToken)
    {
        var severities = await _context.Set<RefSeverity>("SeveritiesActual")
            .Where(s => s.IsActive)
            .OrderBy(s => s.Rank)
            .ToListAsync(cancellationToken);

        return _mapper.Map<List<RefSeverityDto>>(severities);
    }
}
