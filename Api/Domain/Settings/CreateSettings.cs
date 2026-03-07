using AutoMapper;
using MediatR;
using Stronghold.AppDashboard.Api.Authorization;
using Stronghold.AppDashboard.Data;
using Stronghold.AppDashboard.Shared.Enumerations;

namespace Stronghold.AppDashboard.Api.Domain.Settings;

[AllowedAuthorizationRole(AuthorizationRole.Administrator)]
public class CreateSettings : IRequest<Models.Settings>
{
    public Models.Settings SettingsToCreate { get; set; } = null!;
}

public class CreateSettingsHandler : IRequestHandler<CreateSettings, Models.Settings>
{
    private readonly AppDbContext _context;
    private readonly IMapper _mapper;
    private readonly IMediator _mediator;

    public CreateSettingsHandler(AppDbContext context, IMapper mapper, IMediator mediator)
    {
        _context = context;
        _mapper = mapper;
        _mediator = mediator;
    }

    public async Task<Models.Settings> Handle(
        CreateSettings request,
        CancellationToken cancellationToken
    )
    {
        var currentLoggedinUser = await _mediator.Send(new GetCurrentUser(), cancellationToken);

        var settings = _mapper.Map<Data.Models.Settings>(request.SettingsToCreate);

        await _context.Settings.AddAsync(settings, cancellationToken);
        await _context.SaveChangesAsync(cancellationToken);

        return _mapper.Map<Models.Settings>(settings);
    }
}
