using MediatR;
using Microsoft.AspNetCore.Mvc;
using Stronghold.AppDashboard.Api.Domain.ReferenceData;
using Stronghold.AppDashboard.Api.Helpers;
using Stronghold.AppDashboard.Api.Models;

namespace Stronghold.AppDashboard.Api.Controllers;

[Route(Constants.Routes.DefaultControllerTemplate)]
public class ReferenceDataController : V1ControllerBase
{
    public ReferenceDataController(IMediator mediator, ILogger<ReferenceDataController> logger)
        : base(mediator, logger) { }

    [MapToApiVersion(Constants.ApiVersions.V1)]
    [HttpGet("companies")]
    [ProducesResponseType(typeof(List<RefCompanyDto>), StatusCodes.Status200OK)]
    public async Task<ActionResult<List<RefCompanyDto>>> GetCompanies()
    {
        return await TryExecuteAsync<ActionResult<List<RefCompanyDto>>>(
            async () =>
            {
                await GetUser();
                return Ok(await Mediator.Send(new GetCompanies()));
            },
            ex => Error<List<RefCompanyDto>>(ex)
        );
    }

    [MapToApiVersion(Constants.ApiVersions.V1)]
    [HttpGet("regions")]
    [ProducesResponseType(typeof(List<RefRegionDto>), StatusCodes.Status200OK)]
    public async Task<ActionResult<List<RefRegionDto>>> GetRegions([FromQuery] Guid? companyId)
    {
        return await TryExecuteAsync<ActionResult<List<RefRegionDto>>>(
            async () =>
            {
                await GetUser();
                return Ok(await Mediator.Send(new GetRegions { CompanyId = companyId }));
            },
            ex => Error<List<RefRegionDto>>(ex)
        );
    }

    [MapToApiVersion(Constants.ApiVersions.V1)]
    [HttpGet("severities")]
    [ProducesResponseType(typeof(List<RefSeverityDto>), StatusCodes.Status200OK)]
    public async Task<ActionResult<List<RefSeverityDto>>> GetSeverities()
    {
        return await TryExecuteAsync<ActionResult<List<RefSeverityDto>>>(
            async () =>
            {
                await GetUser();
                return Ok(await Mediator.Send(new GetSeverities()));
            },
            ex => Error<List<RefSeverityDto>>(ex)
        );
    }

    [MapToApiVersion(Constants.ApiVersions.V1)]
    [HttpGet("incident-options")]
    [ProducesResponseType(typeof(Dictionary<string, List<RefOptionDto>>), StatusCodes.Status200OK)]
    public async Task<ActionResult<Dictionary<string, List<RefOptionDto>>>> GetIncidentReferenceOptions()
    {
        return await TryExecuteAsync<ActionResult<Dictionary<string, List<RefOptionDto>>>>(
            async () =>
            {
                await GetUser();
                return Ok(await Mediator.Send(new GetIncidentReferenceOptions()));
            },
            ex => Error<Dictionary<string, List<RefOptionDto>>>(ex)
        );
    }

    [MapToApiVersion(Constants.ApiVersions.V1)]
    [HttpGet("workflow-states")]
    [ProducesResponseType(typeof(List<RefWorkflowStateDto>), StatusCodes.Status200OK)]
    public async Task<ActionResult<List<RefWorkflowStateDto>>> GetWorkflowStates([FromQuery] string? domain)
    {
        return await TryExecuteAsync<ActionResult<List<RefWorkflowStateDto>>>(
            async () =>
            {
                await GetUser();
                return Ok(await Mediator.Send(new GetWorkflowStates { Domain = domain }));
            },
            ex => Error<List<RefWorkflowStateDto>>(ex)
        );
    }

    // --- Reference Types ---

    [MapToApiVersion(Constants.ApiVersions.V1)]
    [HttpGet("reference-types")]
    [ProducesResponseType(typeof(List<RefReferenceTypeDto>), StatusCodes.Status200OK)]
    public async Task<ActionResult<List<RefReferenceTypeDto>>> GetReferenceTypes()
    {
        return await TryExecuteAsync<ActionResult<List<RefReferenceTypeDto>>>(
            async () =>
            {
                await GetUser();
                return Ok(await Mediator.Send(new GetReferenceTypes()));
            },
            ex => Error<List<RefReferenceTypeDto>>(ex)
        );
    }

    // --- Companies CRUD ---

    [MapToApiVersion(Constants.ApiVersions.V1)]
    [HttpPost("companies")]
    [ProducesResponseType(typeof(RefCompanyDto), StatusCodes.Status200OK)]
    public async Task<ActionResult<RefCompanyDto>> CreateCompany([FromBody] SaveCompany command)
    {
        return await TryExecuteAsync<ActionResult<RefCompanyDto>>(
            async () =>
            {
                await GetUser();
                command.Id = null;
                return Ok(await Mediator.Send(command));
            },
            ex => Error<RefCompanyDto>(ex)
        );
    }

    [MapToApiVersion(Constants.ApiVersions.V1)]
    [HttpPut("companies/{id:guid}")]
    [ProducesResponseType(typeof(RefCompanyDto), StatusCodes.Status200OK)]
    public async Task<ActionResult<RefCompanyDto>> UpdateCompany(Guid id, [FromBody] SaveCompany command)
    {
        return await TryExecuteAsync<ActionResult<RefCompanyDto>>(
            async () =>
            {
                await GetUser();
                command.Id = id;
                return Ok(await Mediator.Send(command));
            },
            ex => Error<RefCompanyDto>(ex)
        );
    }

    [MapToApiVersion(Constants.ApiVersions.V1)]
    [HttpDelete("companies/{id:guid}")]
    [ProducesResponseType(StatusCodes.Status204NoContent)]
    public async Task<IActionResult> DeleteCompany(Guid id)
    {
        return await TryExecuteAsync<IActionResult>(
            async () =>
            {
                await GetUser();
                await Mediator.Send(new DeleteCompany { Id = id });
                return NoContent();
            },
            ex => Error(ex)
        );
    }

    // --- Regions CRUD ---

    [MapToApiVersion(Constants.ApiVersions.V1)]
    [HttpPost("regions")]
    [ProducesResponseType(typeof(RefRegionDto), StatusCodes.Status200OK)]
    public async Task<ActionResult<RefRegionDto>> CreateRegion([FromBody] SaveRegion command)
    {
        return await TryExecuteAsync<ActionResult<RefRegionDto>>(
            async () =>
            {
                await GetUser();
                command.Id = null;
                return Ok(await Mediator.Send(command));
            },
            ex => Error<RefRegionDto>(ex)
        );
    }

    [MapToApiVersion(Constants.ApiVersions.V1)]
    [HttpPut("regions/{id:guid}")]
    [ProducesResponseType(typeof(RefRegionDto), StatusCodes.Status200OK)]
    public async Task<ActionResult<RefRegionDto>> UpdateRegion(Guid id, [FromBody] SaveRegion command)
    {
        return await TryExecuteAsync<ActionResult<RefRegionDto>>(
            async () =>
            {
                await GetUser();
                command.Id = id;
                return Ok(await Mediator.Send(command));
            },
            ex => Error<RefRegionDto>(ex)
        );
    }

    [MapToApiVersion(Constants.ApiVersions.V1)]
    [HttpDelete("regions/{id:guid}")]
    [ProducesResponseType(StatusCodes.Status204NoContent)]
    public async Task<IActionResult> DeleteRegion(Guid id)
    {
        return await TryExecuteAsync<IActionResult>(
            async () =>
            {
                await GetUser();
                await Mediator.Send(new DeleteRegion { Id = id });
                return NoContent();
            },
            ex => Error(ex)
        );
    }

    // --- Lookup Items CRUD ---

    [MapToApiVersion(Constants.ApiVersions.V1)]
    [HttpPost("lookup-items")]
    [ProducesResponseType(typeof(RefOptionDto), StatusCodes.Status200OK)]
    public async Task<ActionResult<RefOptionDto>> CreateLookupItem([FromBody] SaveLookupItem command)
    {
        return await TryExecuteAsync<ActionResult<RefOptionDto>>(
            async () =>
            {
                await GetUser();
                command.Id = null;
                return Ok(await Mediator.Send(command));
            },
            ex => Error<RefOptionDto>(ex)
        );
    }

    [MapToApiVersion(Constants.ApiVersions.V1)]
    [HttpPut("lookup-items/{id:guid}")]
    [ProducesResponseType(typeof(RefOptionDto), StatusCodes.Status200OK)]
    public async Task<ActionResult<RefOptionDto>> UpdateLookupItem(Guid id, [FromBody] SaveLookupItem command)
    {
        return await TryExecuteAsync<ActionResult<RefOptionDto>>(
            async () =>
            {
                await GetUser();
                command.Id = id;
                return Ok(await Mediator.Send(command));
            },
            ex => Error<RefOptionDto>(ex)
        );
    }

    [MapToApiVersion(Constants.ApiVersions.V1)]
    [HttpDelete("lookup-items/{id:guid}")]
    [ProducesResponseType(StatusCodes.Status204NoContent)]
    public async Task<IActionResult> DeleteLookupItem(Guid id)
    {
        return await TryExecuteAsync<IActionResult>(
            async () =>
            {
                await GetUser();
                await Mediator.Send(new DeleteLookupItem { Id = id });
                return NoContent();
            },
            ex => Error(ex)
        );
    }
}
