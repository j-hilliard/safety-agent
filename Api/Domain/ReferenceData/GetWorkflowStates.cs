using AutoMapper;
using MediatR;
using Microsoft.EntityFrameworkCore;
using Stronghold.AppDashboard.Api.Authorization;
using Stronghold.AppDashboard.Api.Models;
using Stronghold.AppDashboard.Data;
using Stronghold.AppDashboard.Shared.Enumerations;

namespace Stronghold.AppDashboard.Api.Domain.ReferenceData;

[AllowedAuthorizationRole(AuthorizationRole.Administrator, AuthorizationRole.User)]
public class GetWorkflowStates : IRequest<List<RefWorkflowStateDto>>
{
    public string? Domain { get; set; }
}

public class GetWorkflowStatesHandler : IRequestHandler<GetWorkflowStates, List<RefWorkflowStateDto>>
{
    private readonly AppDbContext _context;
    private readonly IMapper _mapper;

    public GetWorkflowStatesHandler(AppDbContext context, IMapper mapper)
    {
        _context = context;
        _mapper = mapper;
    }

    public async Task<List<RefWorkflowStateDto>> Handle(GetWorkflowStates request, CancellationToken cancellationToken)
    {
        var query = _context.WorkflowStates.Where(s => s.IsActive).AsQueryable();

        if (!string.IsNullOrWhiteSpace(request.Domain))
            query = query.Where(s => s.Domain == request.Domain);

        var states = await query.OrderBy(s => s.Name).ToListAsync(cancellationToken);

        return _mapper.Map<List<RefWorkflowStateDto>>(states);
    }
}
