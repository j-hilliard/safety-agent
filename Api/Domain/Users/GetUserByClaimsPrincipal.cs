using System.Diagnostics.CodeAnalysis;
using System.Security.Claims;
using MediatR;
using Microsoft.Identity.Web;
using Stronghold.AppDashboard.Api.Authorization;
using Stronghold.AppDashboard.Api.Configuration;
using Stronghold.AppDashboard.Api.Helpers;
using Stronghold.AppDashboard.Api.Models;
using Stronghold.AppDashboard.Shared.Attributes;
using Stronghold.AppDashboard.Shared.Enumerations;

namespace Stronghold.AppDashboard.Api.Domain.Users;

[AllowedAuthorizationRole(AuthorizationRole.Administrator, AuthorizationRole.User)]
public class GetUserByClaimsPrincipal : IRequest<User?>
{
    [Sensitive]
    public ClaimsPrincipal ClaimsPrincipal { get; set; } = null!;
}

public class GetUserByClaimsPrincipalHandler : IRequestHandler<GetUserByClaimsPrincipal, User?>
{
    private readonly IConfiguration _configuration;
    private readonly IMediator _mediator;

    public GetUserByClaimsPrincipalHandler(IConfiguration configuration, IMediator mediator)
    {
        _configuration = configuration;
        _mediator = mediator;
    }

    public async Task<User?> Handle(
        GetUserByClaimsPrincipal request,
        CancellationToken cancellationToken
    )
    {
        var objectId = request.ClaimsPrincipal.GetObjectIdAsGuid();

        if (objectId == null)
            return null;

        var user = await GetUser(request, objectId, cancellationToken);

        return user;
    }

    private async Task<User?> GetUser(
        GetUserByClaimsPrincipal request,
        [DisallowNull] Guid? objectId,
        CancellationToken cancellationToken
    )
    {
        var azureAdTenantId = _configuration
            .GetSection(ConfigurationSections.AzureAd)
            .GetValue<Guid>("TenantId");
        if (
            Guid.TryParse(request.ClaimsPrincipal.GetTenantId(), out var tenantId)
            && tenantId == azureAdTenantId
        )
            return await GetByAzureAd(request.ClaimsPrincipal, objectId.Value, cancellationToken);

        return null;
    }

    private async Task<User?> GetByAzureAd(
        ClaimsPrincipal principal,
        Guid objectId,
        CancellationToken cancellationToken
    )
    {
        return await _mediator.Send(
            new GetUserByAzureAdObjectId { AzureAdObjectId = objectId },
            cancellationToken
        );
    }
}
