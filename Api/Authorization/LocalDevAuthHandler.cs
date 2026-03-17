using System.Security.Claims;
using System.Text.Encodings.Web;
using Microsoft.AspNetCore.Authentication;
using Microsoft.Extensions.Options;

namespace Stronghold.AppDashboard.Api.Authorization;

/// <summary>
/// Authentication handler for local development that bypasses Azure AD.
/// Only registered when ASPNETCORE_ENVIRONMENT=Local.
/// </summary>
public class LocalDevAuthHandler : AuthenticationHandler<AuthenticationSchemeOptions>
{
    // Must match DBInitializer.cs seeded local user AzureAdObjectId
    private static readonly Guid LocalUserOid = new("00000000-0000-0000-0000-000000000000");
    private const string LocalTenantId = "78d53608-54ca-4a74-8beb-8a1399c1189c";

    public LocalDevAuthHandler(
        IOptionsMonitor<AuthenticationSchemeOptions> options,
        ILoggerFactory logger,
        UrlEncoder encoder,
        ISystemClock clock) : base(options, logger, encoder, clock) { }

    protected override Task<AuthenticateResult> HandleAuthenticateAsync()
    {
        var claims = new[]
        {
            new Claim("oid", LocalUserOid.ToString()),
            new Claim("http://schemas.microsoft.com/identity/claims/objectidentifier", LocalUserOid.ToString()),
            new Claim("tid", LocalTenantId),
            new Claim("http://schemas.microsoft.com/identity/claims/tenantid", LocalTenantId),
            new Claim(ClaimTypes.Name, "Local Dev User"),
        };

        var identity = new ClaimsIdentity(claims, Scheme.Name);
        var principal = new ClaimsPrincipal(identity);
        var ticket = new AuthenticationTicket(principal, Scheme.Name);

        return Task.FromResult(AuthenticateResult.Success(ticket));
    }
}
