using FluentValidation;
using MediatR;
using Microsoft.AspNetCore.Mvc;
using Stronghold.AppDashboard.Api.Domain.Users;
using Stronghold.AppDashboard.Api.Helpers;
using Stronghold.AppDashboard.Api.Models;

namespace Stronghold.AppDashboard.Api.Controllers;

[Route(Constants.Routes.DefaultControllerTemplate)]
public class UserController : V1ControllerBase
{
    private readonly IValidator<User> _validator;

    public UserController(
        IMediator mediator,
        ILogger<UserController> logger,
        IValidator<User> validator
    )
        : base(mediator, logger)
    {
        _validator = validator;
    }

    /* Get All users */
    [MapToApiVersion(Constants.ApiVersions.V1)]
    [HttpGet("AllUsers")]
    [ProducesResponseType(typeof(List<User>), StatusCodes.Status200OK)]
    [ProducesResponseType(StatusCodes.Status404NotFound)]
    public async Task<ActionResult<List<User>>> GetAllUsers()
    {
        return await TryExecuteAsync<ActionResult<List<User>>>(
            async () =>
            {
                await GetUser();

                return Ok(await Mediator.Send(new GetAllUsers()));
            },
            ex => Error<List<User>>(ex)
        );
    }

    /* Get Active Users */
    [MapToApiVersion(Constants.ApiVersions.V1)]
    [HttpGet("ActiveUsers")]
    [ProducesResponseType(typeof(List<User>), StatusCodes.Status200OK)]
    [ProducesResponseType(StatusCodes.Status404NotFound)]
    public async Task<ActionResult<List<User>>> GetActiveUsers()
    {
        return await TryExecuteAsync<ActionResult<List<User>>>(
            async () =>
            {
                await GetUser();

                return Ok(await Mediator.Send(new GetActiveUsers()));
            },
            ex => Error<List<User>>(ex)
        );
    }

    /* Get Disabled Users */
    [MapToApiVersion(Constants.ApiVersions.V1)]
    [HttpGet("DisabledUsers")]
    [ProducesResponseType(typeof(List<User>), StatusCodes.Status200OK)]
    [ProducesResponseType(StatusCodes.Status404NotFound)]
    public async Task<ActionResult<List<User>>> GetDisabledUsers()
    {
        return await TryExecuteAsync<ActionResult<List<User>>>(
            async () =>
            {
                await GetUser();

                return Ok(await Mediator.Send(new GetDisabledUsers()));
            },
            ex => Error<List<User>>(ex)
        );
    }

    /* Get user by userId */
    [MapToApiVersion(Constants.ApiVersions.V1)]
    [HttpGet("ByUserId/{userId:int}")]
    [ProducesResponseType(typeof(User), StatusCodes.Status200OK)]
    [ProducesResponseType(StatusCodes.Status404NotFound)]
    public async Task<ActionResult<User>> GetUserByUserId([FromRoute] int userId)
    {
        return await TryExecuteAsync<ActionResult<User>>(
            async () =>
            {
                await GetUser();

                var user = await Mediator.Send(new GetUser() { UserId = userId });

                return user == null ? NotFound() : Ok(user);
            },
            ex => Error<User>(ex)
        );
    }

    /* Get user by AzureAdObjectId */
    [MapToApiVersion(Constants.ApiVersions.V1)]
    [HttpGet("ByAzureAdObjectId/{azureAdObjectId}")]
    [ProducesResponseType(typeof(User), StatusCodes.Status200OK)]
    [ProducesResponseType(StatusCodes.Status404NotFound)]
    public async Task<ActionResult<User>> GetUserByAzureAdObjectId([FromRoute] Guid azureAdObjectId)
    {
        return await TryExecuteAsync<ActionResult<User>>(
            async () =>
            {
                await GetUser();

                var user = await Mediator.Send(
                    new GetUserByAzureAdObjectId() { AzureAdObjectId = azureAdObjectId }
                );

                return user == null ? NotFound() : Ok(user);
            },
            ex => Error<User>(ex)
        );
    }

    /* Create user */
    [MapToApiVersion(Constants.ApiVersions.V1)]
    [HttpPost]
    [ProducesResponseType(typeof(User), StatusCodes.Status201Created)]
    [ProducesResponseType(typeof(IEnumerable<ValidationError>), StatusCodes.Status400BadRequest)]
    [ProducesResponseType(StatusCodes.Status400BadRequest)]
    [ProducesResponseType(StatusCodes.Status404NotFound)]
    public async Task<ActionResult<User>> CreateUser([FromBody] User user)
    {
        return await TryExecuteAsync<ActionResult<User>>(
            async () =>
            {
                await GetUser();

                var validationResults = await _validator.ValidateAsync(user);

                if (!validationResults.IsValid)
                {
                    var validationErrors = validationResults.ToValidationErrors();
                    return BadRequest(validationErrors);
                }

                var created = await Mediator.Send(new CreateUser() { UserToCreate = user });

                return CreatedAtAction(
                    nameof(CreateUser),
                    new { userId = created.UserId },
                    created
                );
            },
            ex => Error<User>(ex)
        );
    }

    /* Update user */
    [MapToApiVersion(Constants.ApiVersions.V1)]
    [HttpPut("{userId:int}")]
    [ProducesResponseType(typeof(User), StatusCodes.Status202Accepted)]
    [ProducesResponseType(typeof(IEnumerable<ValidationError>), StatusCodes.Status400BadRequest)]
    [ProducesResponseType(StatusCodes.Status404NotFound)]
    public async Task<ActionResult<User>> UpdateUser([FromRoute] int userId, [FromBody] User user)
    {
        return await TryExecuteAsync<ActionResult<User>>(
            async () =>
            {
                await GetUser();

                var validationResults = await _validator.ValidateAsync(user);

                if (!validationResults.IsValid)
                    return BadRequest(validationResults.ToValidationErrors());

                var updated = await Mediator.Send(
                    new UpdateUser() { UserId = userId, UserToUpdate = user }
                );

                return updated == null ? NotFound() : AcceptedAtAction(nameof(UpdateUser), updated);
            },
            ex => Error<User>(ex)
        );
    }

    /* Activate user */
    [MapToApiVersion(Constants.ApiVersions.V1)]
    [HttpPut("ActivateUser/{userId:int}")]
    [ProducesResponseType(StatusCodes.Status202Accepted)]
    [ProducesResponseType(typeof(IEnumerable<ValidationError>), StatusCodes.Status400BadRequest)]
    [ProducesResponseType(StatusCodes.Status404NotFound)]
    public async Task<ActionResult<bool>> ActivateUser([FromRoute] int userId)
    {
        return await TryExecuteAsync<ActionResult<bool>>(
            async () =>
            {
                await GetUser();

                var success = await Mediator.Send(new EnableUser() { UserId = userId });

                if (success == null)
                    return NotFound();

                if (success == false)
                    return BadRequest();

                return AcceptedAtAction(nameof(ActivateUser), success);
            },
            ex => Error<bool>(ex)
        );
    }

    /* Disable user */
    [MapToApiVersion(Constants.ApiVersions.V1)]
    [HttpPut("DisableUser/{userId:int}")]
    [ProducesResponseType(StatusCodes.Status202Accepted)]
    [ProducesResponseType(typeof(IEnumerable<ValidationError>), StatusCodes.Status400BadRequest)]
    [ProducesResponseType(StatusCodes.Status404NotFound)]
    public async Task<ActionResult<bool>> DisableUser([FromRoute] int userId)
    {
        return await TryExecuteAsync<ActionResult<bool>>(
            async () =>
            {
                await GetUser();

                var success = await Mediator.Send(new DisableUser() { UserId = userId });

                if (success == null)
                    return NotFound();

                if (success == false)
                    return BadRequest();

                return AcceptedAtAction(nameof(DisableUser), success);
            },
            ex => Error<bool>(ex)
        );
    }

    /* Delete user */
    [MapToApiVersion(Constants.ApiVersions.V1)]
    [HttpDelete("{userId:int}")]
    [ProducesResponseType(StatusCodes.Status202Accepted)]
    [ProducesResponseType(StatusCodes.Status400BadRequest)]
    [ProducesResponseType(StatusCodes.Status404NotFound)]
    public async Task<ActionResult<User>> DeleteUser([FromRoute] int userId)
    {
        return await TryExecuteAsync<ActionResult<User>>(
            async () =>
            {
                await GetUser();

                var deleted = await Mediator.Send(new DeleteUser() { UserId = userId });

                return deleted == null ? NotFound() : AcceptedAtAction(nameof(DeleteUser), deleted);
            },
            ex => Error<User>(ex)
        );
    }
}
