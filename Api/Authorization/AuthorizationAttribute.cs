using Stronghold.AppDashboard.Shared.Enumerations;

namespace Stronghold.AppDashboard.Api.Authorization;

[AttributeUsage(AttributeTargets.Class)]
public class AllowedAuthorizationRole : Attribute
{
    public AuthorizationRole[] AllowedAuthorizationRoles { get; }

    public AllowedAuthorizationRole(AuthorizationRole userRole)
    {
        AllowedAuthorizationRoles = new[] { userRole };
    }

    public AllowedAuthorizationRole(params AuthorizationRole[] userRoles)
    {
        AllowedAuthorizationRoles = userRoles;
    }

    public bool IsAllowed(List<string>? userRoles)
    {
        if (AllowedAuthorizationRoles.Contains(AuthorizationRole.AuthenticatedUser))
            return true;

        if (userRoles == null)
        {
            return false;
        }

        return userRoles.Any(role =>
            AllowedAuthorizationRoles.Select(r => r.ToString()).Contains(role)
        );
    }
}
