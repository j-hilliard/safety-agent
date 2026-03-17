using AutoMapper;
using MediatR;
using Microsoft.EntityFrameworkCore;
using Stronghold.AppDashboard.Api.Authorization;
using Stronghold.AppDashboard.Api.Models;
using Stronghold.AppDashboard.Data;
using Stronghold.AppDashboard.Shared.Enumerations;

namespace Stronghold.AppDashboard.Api.Domain.ReferenceData;

[AllowedAuthorizationRole(AuthorizationRole.Administrator, AuthorizationRole.User)]
public class GetCompanies : IRequest<List<RefCompanyDto>> { }

public class GetCompaniesHandler : IRequestHandler<GetCompanies, List<RefCompanyDto>>
{
    private readonly AppDbContext _context;
    private readonly IMapper _mapper;

    public GetCompaniesHandler(AppDbContext context, IMapper mapper)
    {
        _context = context;
        _mapper = mapper;
    }

    public async Task<List<RefCompanyDto>> Handle(GetCompanies request, CancellationToken cancellationToken)
    {
        var companies = await _context.Companies
            .Where(c => c.IsActive)
            .OrderBy(c => c.Name)
            .ToListAsync(cancellationToken);

        return _mapper.Map<List<RefCompanyDto>>(companies);
    }
}
