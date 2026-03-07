using FluentValidation;
using MediatR;
using Microsoft.AspNetCore.Mvc;
using Stronghold.AppDashboard.Api.Domain.Roles;
using Stronghold.AppDashboard.Api.Helpers;
using Stronghold.AppDashboard.Api.Models;

namespace Stronghold.AppDashboard.Api.Controllers;

[Route(Constants.Routes.DefaultControllerTemplate)]
public class RoleController : V1ControllerBase
{
    private readonly IValidator<Role> _validator;

    public RoleController(
        IMediator mediator,
        ILogger<RoleController> logger,
        IValidator<Role> validator
    )
        : base(mediator, logger)
    {
        _validator = validator;
    }

    /* Get All Roles */
    [MapToApiVersion(Constants.ApiVersions.V1)]
    [HttpGet]
    [ProducesResponseType(typeof(List<Role>), StatusCodes.Status200OK)]
    [ProducesResponseType(StatusCodes.Status404NotFound)]
    public async Task<ActionResult<List<Role>>> GetAllRoles()
    {
        return await TryExecuteAsync<ActionResult<List<Role>>>(
            async () =>
            {
                await GetUser();

                var result = await Mediator.Send(new GetAllRoles());
                return Ok(result);
            },
            ex => Error<List<Role>>(ex)
        );
    }

    /* Get Role by RoleId */
    [MapToApiVersion(Constants.ApiVersions.V1)]
    [HttpGet("{roleId:int}")]
    [ProducesResponseType(typeof(Role), StatusCodes.Status200OK)]
    [ProducesResponseType(StatusCodes.Status404NotFound)]
    public async Task<ActionResult<Role>> GetUser([FromRoute] int roleId)
    {
        return await TryExecuteAsync<ActionResult<Role>>(
            async () =>
            {
                await GetUser();

                var role = await Mediator.Send(new GetRole() { RoleId = roleId });

                return role == null ? NotFound() : Ok(role);
            },
            ex => Error<Role>(ex)
        );
    }

    /* Create Role */
    [MapToApiVersion(Constants.ApiVersions.V1)]
    [HttpPost]
    [ProducesResponseType(typeof(Role), StatusCodes.Status201Created)]
    [ProducesResponseType(typeof(IEnumerable<ValidationError>), StatusCodes.Status400BadRequest)]
    [ProducesResponseType(StatusCodes.Status400BadRequest)]
    [ProducesResponseType(StatusCodes.Status404NotFound)]
    public async Task<ActionResult<Role>> CreateRole([FromBody] Role role)
    {
        return await TryExecuteAsync<ActionResult<Role>>(
            async () =>
            {
                await GetUser();

                var validationResults = await _validator.ValidateAsync(role);

                if (!validationResults.IsValid)
                {
                    var validationErrors = validationResults.ToValidationErrors();
                    return BadRequest(validationErrors);
                }

                var created = await Mediator.Send(new CreateRole() { RoleToCreate = role });

                return CreatedAtAction(
                    nameof(CreateRole),
                    new { roleId = created.RoleId },
                    created
                );
            },
            ex => Error<Role>(ex)
        );
    }

    /* Update Role */
    [MapToApiVersion(Constants.ApiVersions.V1)]
    [HttpPut("{roleId:int}")]
    [ProducesResponseType(typeof(Role), StatusCodes.Status202Accepted)]
    [ProducesResponseType(typeof(IEnumerable<ValidationError>), StatusCodes.Status400BadRequest)]
    [ProducesResponseType(StatusCodes.Status404NotFound)]
    public async Task<ActionResult<Role>> UpdateRole([FromRoute] int roleId, [FromBody] Role role)
    {
        return await TryExecuteAsync<ActionResult<Role>>(
            async () =>
            {
                await GetUser();

                var validationResults = await _validator.ValidateAsync(role);

                if (!validationResults.IsValid)
                    return BadRequest(validationResults.ToValidationErrors());

                var updated = await Mediator.Send(
                    new UpdateRole { RoleId = roleId, RoleUpdate = role }
                );

                return updated == null ? NotFound() : AcceptedAtAction(nameof(UpdateRole), updated);
            },
            ex => Error<Role>(ex)
        );
    }
}
