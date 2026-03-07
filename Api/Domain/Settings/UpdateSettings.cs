using AutoMapper;
using MediatR;
using Stronghold.AppDashboard.Api.Authorization;
using Stronghold.AppDashboard.Data;
using Stronghold.AppDashboard.Shared.Enumerations;

namespace Stronghold.AppDashboard.Api.Domain.Settings;

[AllowedAuthorizationRole(AuthorizationRole.Administrator)]
public class UpdateSettings : IRequest<Models.Settings?>
{
    public int SettingsId { get; set; }
    public Models.Settings SettingsUpdate { get; set; } = null!;
}

public class UpdateSettingsHandler : IRequestHandler<UpdateSettings, Models.Settings?>
{
    private readonly AppDbContext _context;
    private readonly IMapper _mapper;
    private readonly IMediator _mediator;

    public UpdateSettingsHandler(AppDbContext context, IMapper mapper, IMediator mediator)
    {
        _context = context;
        _mapper = mapper;
        _mediator = mediator;
    }

    public async Task<Models.Settings?> Handle(
        UpdateSettings request,
        CancellationToken cancellationToken
    )
    {
        var settings = await _context.Settings.FindAsync(request.SettingsId);
        if (settings == null)
            throw new Exception("Existing Settings not found");

        var currentUser = await _mediator.Send(new GetCurrentUser(), cancellationToken);
        if (request.SettingsUpdate.ModifiedById == null && currentUser != null)
        {
            request.SettingsUpdate.ModifiedById = currentUser.UserId;
        }

        _mapper.Map(request.SettingsUpdate, settings);
        await _context.SaveChangesAsync(cancellationToken);

        return _mapper.Map<Models.Settings>(settings);
    }
}
