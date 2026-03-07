using System.Net.Mail;
using System.Security.Claims;
using Microsoft.Identity.Web;

namespace Stronghold.AppDashboard.Api.Helpers;

public static class ClaimsPrincipalExtensions
{
    public static string? GetEmail(this ClaimsPrincipal principal)
    {
        var email =
            principal.FindFirstValue(ClaimTypes.Email)?.Trim().ToLowerInvariant()
            ?? principal.FindFirstValue(ClaimTypes.Upn)?.Trim().ToLowerInvariant()
            ?? principal.FindFirstValue("emails")?.Trim().ToLowerInvariant();

        return MailAddress.TryCreate(email, out var mailAddress) ? mailAddress.Address : null;
    }

    public static Guid? GetObjectIdAsGuid(this ClaimsPrincipal principal)
    {
        return Guid.TryParse(principal.GetObjectId(), out var objectId) ? objectId : null;
    }

    public static string? GetFirstName(this ClaimsPrincipal principal)
    {
        return principal.FindFirstValue(ClaimTypes.GivenName)?.Trim();
    }

    public static string? GetLastName(this ClaimsPrincipal principal)
    {
        return principal.FindFirstValue(ClaimTypes.Surname)?.Trim();
    }
}
