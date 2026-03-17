using AutoMapper;
using MediatR;
using Microsoft.EntityFrameworkCore;
using Stronghold.AppDashboard.Api.Authorization;
using Stronghold.AppDashboard.Api.Models;
using Stronghold.AppDashboard.Data;
using Stronghold.AppDashboard.Shared.Enumerations;

namespace Stronghold.AppDashboard.Api.Domain.ReferenceData;

[AllowedAuthorizationRole(AuthorizationRole.Administrator, AuthorizationRole.User)]
public class GetReferenceTypes : IRequest<List<RefReferenceTypeDto>> { }

public class GetReferenceTypesHandler : IRequestHandler<GetReferenceTypes, List<RefReferenceTypeDto>>
{
    private readonly AppDbContext _context;
    private readonly IMapper _mapper;

    public GetReferenceTypesHandler(AppDbContext context, IMapper mapper)
    {
        _context = context;
        _mapper = mapper;
    }

    public async Task<List<RefReferenceTypeDto>> Handle(GetReferenceTypes request, CancellationToken cancellationToken)
    {
        var types = await _context.ReferenceTypes
            .Where(t => t.IsActive)
            .OrderBy(t => t.AppliesTo)
            .ThenBy(t => t.Name)
            .ToListAsync(cancellationToken);

        return _mapper.Map<List<RefReferenceTypeDto>>(types);
    }
}
