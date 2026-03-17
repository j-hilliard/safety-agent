using FluentValidation;
using MediatR;
using Microsoft.AspNetCore.Mvc;
using Stronghold.AppDashboard.Api.Domain.IncidentReports;
using Stronghold.AppDashboard.Api.Helpers;
using Stronghold.AppDashboard.Api.Models;

namespace Stronghold.AppDashboard.Api.Controllers;

[Route(Constants.Routes.DefaultControllerTemplate)]
public class IncidentReportController : V1ControllerBase
{
    private readonly IValidator<IncidentReport> _validator;

    public IncidentReportController(
        IMediator mediator,
        ILogger<IncidentReportController> logger,
        IValidator<IncidentReport> validator
    )
        : base(mediator, logger)
    {
        _validator = validator;
    }

    [MapToApiVersion(Constants.ApiVersions.V1)]
    [HttpGet]
    [ProducesResponseType(typeof(List<IncidentReportListItem>), StatusCodes.Status200OK)]
    public async Task<ActionResult<List<IncidentReportListItem>>> GetIncidentList(
        [FromQuery] string? searchTerm,
        [FromQuery] Guid? companyId,
        [FromQuery] Guid? regionId,
        [FromQuery] string? status,
        [FromQuery] DateTime? dateFrom,
        [FromQuery] DateTime? dateTo)
    {
        return await TryExecuteAsync<ActionResult<List<IncidentReportListItem>>>(
            async () =>
            {
                await GetUser();
                return Ok(await Mediator.Send(new GetIncidentList
                {
                    SearchTerm = searchTerm,
                    CompanyId = companyId,
                    RegionId = regionId,
                    Status = status,
                    DateFrom = dateFrom,
                    DateTo = dateTo
                }));
            },
            ex => Error<List<IncidentReportListItem>>(ex)
        );
    }

    [MapToApiVersion(Constants.ApiVersions.V1)]
    [HttpGet("{id:guid}")]
    [ProducesResponseType(typeof(IncidentReport), StatusCodes.Status200OK)]
    [ProducesResponseType(StatusCodes.Status404NotFound)]
    public async Task<ActionResult<IncidentReport>> GetIncident([FromRoute] Guid id)
    {
        return await TryExecuteAsync<ActionResult<IncidentReport>>(
            async () =>
            {
                await GetUser();
                var result = await Mediator.Send(new GetIncident { IncidentReportId = id });
                return result == null ? NotFound() : Ok(result);
            },
            ex => Error<IncidentReport>(ex)
        );
    }

    [MapToApiVersion(Constants.ApiVersions.V1)]
    [HttpPost]
    [ProducesResponseType(typeof(IncidentReport), StatusCodes.Status201Created)]
    [ProducesResponseType(typeof(IEnumerable<ValidationError>), StatusCodes.Status400BadRequest)]
    public async Task<ActionResult<IncidentReport>> CreateIncident([FromBody] IncidentReport incidentReport)
    {
        return await TryExecuteAsync<ActionResult<IncidentReport>>(
            async () =>
            {
                await GetUser();

                var validationResult = await _validator.ValidateAsync(incidentReport);
                if (!validationResult.IsValid)
                    return BadRequest(validationResult.ToValidationErrors());

                var created = await Mediator.Send(new CreateIncident { IncidentReportToCreate = incidentReport });

                return CreatedAtAction(nameof(GetIncident), new { id = created.Id }, created);
            },
            ex => Error<IncidentReport>(ex)
        );
    }

    [MapToApiVersion(Constants.ApiVersions.V1)]
    [HttpPut("{id:guid}")]
    [ProducesResponseType(typeof(IncidentReport), StatusCodes.Status202Accepted)]
    [ProducesResponseType(typeof(IEnumerable<ValidationError>), StatusCodes.Status400BadRequest)]
    [ProducesResponseType(StatusCodes.Status404NotFound)]
    public async Task<ActionResult<IncidentReport>> UpdateIncident([FromRoute] Guid id, [FromBody] IncidentReport incidentReport)
    {
        return await TryExecuteAsync<ActionResult<IncidentReport>>(
            async () =>
            {
                await GetUser();

                var validationResult = await _validator.ValidateAsync(incidentReport);
                if (!validationResult.IsValid)
                    return BadRequest(validationResult.ToValidationErrors());

                var updated = await Mediator.Send(new UpdateIncident { IncidentReportId = id, IncidentReportToUpdate = incidentReport });

                return updated == null ? NotFound() : AcceptedAtAction(nameof(UpdateIncident), updated);
            },
            ex => Error<IncidentReport>(ex)
        );
    }

    [MapToApiVersion(Constants.ApiVersions.V1)]
    [HttpDelete("{id:guid}")]
    [ProducesResponseType(StatusCodes.Status202Accepted)]
    [ProducesResponseType(StatusCodes.Status404NotFound)]
    public async Task<ActionResult<bool>> DeleteIncident([FromRoute] Guid id)
    {
        return await TryExecuteAsync<ActionResult<bool>>(
            async () =>
            {
                await GetUser();
                var deleted = await Mediator.Send(new DeleteIncident { IncidentReportId = id });
                return deleted == null ? NotFound() : AcceptedAtAction(nameof(DeleteIncident), deleted);
            },
            ex => Error<bool>(ex)
        );
    }
}
