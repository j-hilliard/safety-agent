namespace Stronghold.AppDashboard.Shared.Enumerations;

public enum AuthorizationRole
{
    User,
    ApplicationDirectoryManager,
    IntegratedApplicationManager,
    Administrator,
    AuthenticatedUser,
}

public static class AuthorizationRoles
{
    public const string Administrator = nameof(AuthorizationRole.Administrator);
    public const string ApplicationDirectoryManager = nameof(
        AuthorizationRole.ApplicationDirectoryManager
    );
    public const string IntegratedApplicationManager = nameof(
        AuthorizationRole.IntegratedApplicationManager
    );
    public const string User = nameof(AuthorizationRole.User);
}
