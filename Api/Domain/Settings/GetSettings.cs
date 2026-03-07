using AutoMapper;
using MediatR;
using Microsoft.EntityFrameworkCore;
using Stronghold.AppDashboard.Api.Authorization;
using Stronghold.AppDashboard.Data;
using Stronghold.AppDashboard.Shared.Enumerations;

namespace Stronghold.AppDashboard.Api.Domain.Settings;

[AllowedAuthorizationRole(AuthorizationRole.Administrator)]
public class GetSettings : IRequest<Models.Settings?> { }

public class GetSettingsHandler : IRequestHandler<GetSettings, Models.Settings?>
{
    private readonly AppDbContext _context;
    private readonly IMapper _mapper;

    public GetSettingsHandler(AppDbContext context, IMapper mapper)
    {
        _context = context;
        _mapper = mapper;
    }

    public async Task<Api.Models.Settings?> Handle(
        GetSettings request,
        CancellationToken cancellationToken
    )
    {
        var settings = await _context.Settings.FirstOrDefaultAsync(cancellationToken);
        return _mapper.Map<Models.Settings>(settings);
    }
}
