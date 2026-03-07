using FluentValidation;
using MediatR;
using Microsoft.AspNetCore.Mvc;
using Stronghold.AppDashboard.Api.Domain.Settings;
using Stronghold.AppDashboard.Api.Helpers;
using Stronghold.AppDashboard.Api.Models;

namespace Stronghold.AppDashboard.Api.Controllers;

[Route(Constants.Routes.DefaultControllerTemplate)]
public class SettingsController : V1ControllerBase
{
    private readonly IValidator<Settings> _validator;

    public SettingsController(
        IMediator mediator,
        ILogger<SettingsController> logger,
        IValidator<Settings> validator
    )
        : base(mediator, logger)
    {
        _validator = validator;
    }

    /* Get Settings Membership by SettingsId */
    [MapToApiVersion(Constants.ApiVersions.V1)]
    [HttpGet]
    [ProducesResponseType(typeof(Settings), StatusCodes.Status200OK)]
    [ProducesResponseType(StatusCodes.Status404NotFound)]
    public async Task<ActionResult<Settings>> GetSettings()
    {
        return await TryExecuteAsync<ActionResult<Settings>>(
            async () =>
            {
                await GetUser();

                var settings = await Mediator.Send(new GetSettings());

                return settings == null ? NotFound() : Ok(settings);
            },
            ex => Error<Settings>(ex)
        );
    }

    /* Add User to Settings */
    [MapToApiVersion(Constants.ApiVersions.V1)]
    [HttpPost]
    [ProducesResponseType(typeof(Settings), StatusCodes.Status201Created)]
    [ProducesResponseType(typeof(IEnumerable<ValidationError>), StatusCodes.Status400BadRequest)]
    [ProducesResponseType(StatusCodes.Status400BadRequest)]
    [ProducesResponseType(StatusCodes.Status404NotFound)]
    public async Task<ActionResult<Settings>> CreateSettings([FromBody] Settings settings)
    {
        return await TryExecuteAsync<ActionResult<Settings>>(
            async () =>
            {
                await GetUser();

                var validationResults = await _validator.ValidateAsync(settings);

                if (!validationResults.IsValid)
                {
                    var validationErrors = validationResults.ToValidationErrors();
                    return BadRequest(validationErrors);
                }

                var created = await Mediator.Send(
                    new CreateSettings() { SettingsToCreate = settings }
                );

                return CreatedAtAction(
                    nameof(CreateSettings),
                    new { userSettings = created },
                    created
                );
            },
            ex => Error<Settings>(ex)
        );
    }

    /* Update Settings */
    [MapToApiVersion(Constants.ApiVersions.V1)]
    [HttpPut("{settingsId:int}")]
    [ProducesResponseType(typeof(Settings), StatusCodes.Status202Accepted)]
    [ProducesResponseType(typeof(IEnumerable<ValidationError>), StatusCodes.Status400BadRequest)]
    [ProducesResponseType(StatusCodes.Status404NotFound)]
    public async Task<ActionResult<Settings>> UpdateSettings(
        [FromRoute] int settingsId,
        [FromBody] Settings settings
    )
    {
        return await TryExecuteAsync<ActionResult<Settings>>(
            async () =>
            {
                await GetUser();

                var validationResults = await _validator.ValidateAsync(settings);

                if (!validationResults.IsValid)
                    return BadRequest(validationResults.ToValidationErrors());

                var updated = await Mediator.Send(
                    new UpdateSettings { SettingsId = settingsId, SettingsUpdate = settings }
                );

                return updated == null
                    ? NotFound()
                    : AcceptedAtAction(nameof(UpdateSettings), updated);
            },
            ex => Error<Settings>(ex)
        );
    }
}
