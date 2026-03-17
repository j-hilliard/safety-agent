using AutoMapper;
using MediatR;
using Microsoft.EntityFrameworkCore;
using Stronghold.AppDashboard.Api.Authorization;
using Stronghold.AppDashboard.Api.Models;
using Stronghold.AppDashboard.Data;
using Stronghold.AppDashboard.Shared.Enumerations;

namespace Stronghold.AppDashboard.Api.Domain.ReferenceData;

[AllowedAuthorizationRole(AuthorizationRole.Administrator, AuthorizationRole.User)]
public class GetRegions : IRequest<List<RefRegionDto>>
{
    public Guid? CompanyId { get; set; }
}

public class GetRegionsHandler : IRequestHandler<GetRegions, List<RefRegionDto>>
{
    private readonly AppDbContext _context;
    private readonly IMapper _mapper;

    public GetRegionsHandler(AppDbContext context, IMapper mapper)
    {
        _context = context;
        _mapper = mapper;
    }

    public async Task<List<RefRegionDto>> Handle(GetRegions request, CancellationToken cancellationToken)
    {
        var query = _context.Regions.Where(r => r.IsActive).AsQueryable();

        if (request.CompanyId.HasValue)
            query = query.Where(r => r.CompanyId == request.CompanyId);

        var regions = await query.OrderBy(r => r.Name).ToListAsync(cancellationToken);

        // Fallback: if no regions are mapped to this company yet, show all active regions.
        // This handles partial mapping — once company_id values are populated for all companies,
        // the filter will automatically show only the correct regions.
        if (regions.Count == 0 && request.CompanyId.HasValue)
            regions = await _context.Regions.Where(r => r.IsActive).OrderBy(r => r.Name).ToListAsync(cancellationToken);

        return _mapper.Map<List<RefRegionDto>>(regions);
    }
}
