using FluentValidation;
using MediatR;
using Microsoft.AspNetCore.Mvc;
using Stronghold.AppDashboard.Api.Domain.UserRoles;
using Stronghold.AppDashboard.Api.Helpers;
using Stronghold.AppDashboard.Api.Models;

namespace Stronghold.AppDashboard.Api.Controllers;

[Route(Constants.Routes.DefaultControllerTemplate)]
public class UserRoleController : V1ControllerBase
{
    private readonly IValidator<UserRole> _validator;

    public UserRoleController(
        IMediator mediator,
        ILogger<RoleController> logger,
        IValidator<UserRole> validator
    )
        : base(mediator, logger)
    {
        _validator = validator;
    }

    /* Get Role Membership by RoleId */
    [MapToApiVersion(Constants.ApiVersions.V1)]
    [HttpGet("RoleMembership/{roleId:int}")]
    [ProducesResponseType(typeof(List<User>), StatusCodes.Status200OK)]
    [ProducesResponseType(StatusCodes.Status404NotFound)]
    public async Task<ActionResult<List<User>>> GetRoleMembership([FromRoute] int roleId)
    {
        return await TryExecuteAsync<ActionResult<List<User>>>(
            async () =>
            {
                await GetUser();

                var users = await Mediator.Send(new GetAllUsersInRole() { RoleId = roleId });

                return users.Count == 0 ? NotFound() : Ok(users);
            },
            ex => Error<List<User>>(ex)
        );
    }

    /* Add User to Role */
    [MapToApiVersion(Constants.ApiVersions.V1)]
    [HttpPost]
    [ProducesResponseType(typeof(UserRole), StatusCodes.Status201Created)]
    [ProducesResponseType(typeof(IEnumerable<ValidationError>), StatusCodes.Status400BadRequest)]
    [ProducesResponseType(StatusCodes.Status400BadRequest)]
    [ProducesResponseType(StatusCodes.Status404NotFound)]
    public async Task<ActionResult<UserRole>> AddUserToRole([FromBody] UserRole userrole)
    {
        return await TryExecuteAsync<ActionResult<UserRole>>(
            async () =>
            {
                await GetUser();

                var validationResults = await _validator.ValidateAsync(userrole);

                if (!validationResults.IsValid)
                {
                    var validationErrors = validationResults.ToValidationErrors();
                    return BadRequest(validationErrors);
                }

                var created = await Mediator.Send(
                    new AddUserToRole() { UserToAddToRole = userrole }
                );

                return CreatedAtAction(nameof(AddUserToRole), new { userrole = created }, created);
            },
            ex => Error<UserRole>(ex)
        );
    }

    /* Remove User from Role */
    [MapToApiVersion(Constants.ApiVersions.V1)]
    [HttpDelete("{userId:int}/{roleId:int}")]
    [ProducesResponseType(StatusCodes.Status202Accepted)]
    [ProducesResponseType(StatusCodes.Status400BadRequest)]
    [ProducesResponseType(StatusCodes.Status404NotFound)]
    public async Task<ActionResult<Role>> DeleteRole([FromRoute] int userId, [FromRoute] int roleId)
    {
        return await TryExecuteAsync<ActionResult<Role>>(
            async () =>
            {
                await GetUser();

                var deleted = await Mediator.Send(
                    new RemoveUserFromRole() { UserId = userId, RoleId = roleId }
                );

                return deleted == null ? NotFound() : AcceptedAtAction(nameof(DeleteRole), deleted);
            },
            ex => Error<Role>(ex)
        );
    }
}
