namespace Stronghold.AppDashboard.Api.Helpers;

public class Constants
{
    public static class ApiVersions
    {
        public const string V1 = "1.0";
    }

    public static class Routes
    {
        public const string ApiTemplate = "v{version:apiVersion}";
        public const string DefaultControllerTemplate = $"{ApiTemplate}/[controller]";
    }

    public static class AuthenticationSchemes
    {
        public const string AzureAd = $"{Microsoft.Identity.Web.Constants.AzureAd}";
    }
}
