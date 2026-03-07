using MediatR;
using Microsoft.AspNetCore.Mvc;
using Stronghold.AppDashboard.Api.Domain.ActiveDirectoryInfo;
using Stronghold.AppDashboard.Api.Helpers;
using Stronghold.AppDashboard.Api.Models;

namespace Stronghold.AppDashboard.Api.Controllers;

[Route(Constants.Routes.DefaultControllerTemplate)]
public class AdHelperController : V1ControllerBase
{
    public AdHelperController(IMediator mediator, ILogger<AdHelperController> logger)
        : base(mediator, logger) { }

    [MapToApiVersion(Constants.ApiVersions.V1)]
    [HttpGet("getaduserinfo/{azureAdObjectId:guid}")]
    [ProducesResponseType(typeof(AdUserInfo), StatusCodes.Status200OK)]
    [ProducesResponseType(StatusCodes.Status404NotFound)]
    public async Task<ActionResult<AdUserInfo?>> GetAdUserInfo([FromRoute] Guid azureAdObjectId)
    {
        return await TryExecuteAsync<ActionResult<AdUserInfo?>>(
            async () =>
            {
                await GetUser();

                var adUserInfo = await Mediator.Send(
                    new GetAdUserInfo() { UserAzureAdObjectId = azureAdObjectId }
                );

                return adUserInfo == null ? NotFound() : Ok(adUserInfo);
            },
            ex => Error<AdUserInfo?>(ex)
        );
    }

    [MapToApiVersion(Constants.ApiVersions.V1)]
    [HttpGet("getadgroupinfo/{groupName}")]
    [ProducesResponseType(typeof(AdGroupInfo), StatusCodes.Status200OK)]
    [ProducesResponseType(StatusCodes.Status404NotFound)]
    public async Task<ActionResult<AdGroupInfo?>> GetAdGroupInfo([FromRoute] string groupName)
    {
        return await TryExecuteAsync<ActionResult<AdGroupInfo?>>(
            async () =>
            {
                await GetUser();

                var adGroupInfo = await Mediator.Send(
                    new GetAdGroupInfo() { GroupName = groupName }
                );

                return adGroupInfo == null ? NotFound() : Ok(adGroupInfo);
            },
            ex => Error<AdGroupInfo?>(ex)
        );
    }

    [MapToApiVersion(Constants.ApiVersions.V1)]
    [HttpGet("getadgroupmembers/{azureAdObjectId:guid}")]
    [ProducesResponseType(typeof(List<AdUserInfo>), StatusCodes.Status200OK)]
    [ProducesResponseType(StatusCodes.Status404NotFound)]
    public async Task<ActionResult<List<AdUserInfo>?>> GetAdGroupMembers(
        [FromRoute] Guid azureAdObjectId
    )
    {
        return await TryExecuteAsync<ActionResult<List<AdUserInfo>?>>(
            async () =>
            {
                await GetUser();

                var adGroupMembers = await Mediator.Send(
                    new GetAdGroupMembers() { GroupAzureObjectSid = azureAdObjectId }
                );

                return !adGroupMembers.Any() ? NotFound() : Ok(adGroupMembers);
            },
            ex => Error<List<AdUserInfo>?>(ex)
        );
    }
}
