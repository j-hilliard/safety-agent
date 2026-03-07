using Microsoft.Identity.Web;

namespace Stronghold.AppDashboard.Api.Configuration;

public static class ConfigurationSections
{
    public const string AllowedHosts = nameof(AllowedHosts);
    public const string ApplicationInsights = nameof(ApplicationInsights);
    public const string AzureAd = Constants.AzureAd;
    public const string ConnectionStrings = nameof(ConnectionStrings);
    public const string AppRegSecretForGraphAccess = nameof(AppRegSecretForGraphAccess);
    public const string Logging = nameof(Logging);
}
