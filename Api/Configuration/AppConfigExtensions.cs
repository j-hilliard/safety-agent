using System.Diagnostics;
using Azure.Core;
using Azure.Identity;
using Newtonsoft.Json.Linq;

namespace Stronghold.AppDashboard.Api.Configuration
{
    public static class AppConfigExtensions
    {
        public static IConfigurationBuilder ConfigureApp(
            this IConfigurationBuilder configBuilder,
            Action<AppConfigOptions> configOptions
        )
        {
            var options = new AppConfigOptions();
            configOptions(options);

            // ✅ FIX: capture local intent BEFORE we mutate options.Environment
            var isLocal = string.Equals(options.Environment, "Local", StringComparison.OrdinalIgnoreCase);

            TokenCredential credential;

            if (!isLocal) // Azure Environment
            {
                options.WriteMessage(
                    $"******************** Using Managed Identity for App Config - AppConfigManagedId: {options.AppConfigManagedId}, TenantId: {options.TenantId}"
                );
                credential = new ManagedIdentityCredential(
                    options.AppConfigManagedId,
                    new DefaultAzureCredentialOptions
                    {
                        SharedTokenCacheTenantId = options.TenantId,
                        InteractiveBrowserTenantId = options.TenantId,
                        VisualStudioTenantId = options.TenantId,
                    }
                );
            }
            else // Local Developer Machine
            {
                options.WriteMessage(
                    $"******************** Using Default Azure Credential for App Config - TenantId: {options.TenantId}"
                );
                credential = new DefaultAzureCredential(
                    new DefaultAzureCredentialOptions
                    {
                        SharedTokenCacheTenantId = options.TenantId,
                        InteractiveBrowserTenantId = options.TenantId,
                        VisualStudioTenantId = options.TenantId,
                        VisualStudioCodeTenantId = options.TenantId,
                    }
                );

                // override options.Environment (not the ASPNETCORE_ENVIRONMENT variable) to look at Azure App Config "Development" keys
                // override Azure App Config "Development" keys for local development in appsettings.Local.json
                options.Environment = "Development";
            }

            // If building an NSwag client, don't add azure App Config
            if (IsRunningForNswagCodegen())
            {
                return configBuilder;
            }

            // ✅ FIX: For Local dev, do NOT call Azure App Configuration at startup.
            // Local settings should come from appsettings.Local.json / appsettings.Development.json.
            if (isLocal)
            {
                options.WriteMessage("******************** Local mode: skipping Azure App Configuration.");
                return configBuilder;
            }

            configBuilder.AddAzureAppConfiguration(opt =>
            {
                opt.ConfigureClientOptions(x =>
                {
                    x.Retry.Delay = TimeSpan.FromSeconds(1);
                    x.Retry.MaxRetries = 10;
                    x.Diagnostics.IsLoggingEnabled = true;
                    x.Diagnostics.IsLoggingContentEnabled = true;
                });

                if (options.AzureConfigUri != null)
                    opt.Connect(new Uri(options.AzureConfigUri), credential);

                foreach (var section in options.AppConfigSections)
                {
                    opt.Select(section.Path + section.SectionName + "*", options.Environment);
                    opt.TrimKeyPrefix(section.Path);
                    options.WriteMessage(
                        $"******************** Adding Config Path: {section.Path} and Section: {section.SectionName}*"
                    );
                }

                opt.ConfigureRefresh(refresh =>
                {
                    // NOTE: application will poll on a 10 second interval for new config updates, but the sentinel key must be changed in order for the application to pick up changes.
                    const string refreshSentinel = "CoreApi/Sentinel_RefreshAllConfig";
                    options.WriteMessage(
                        $"******************** Registering App Config Refresh/Sentinel Key: {refreshSentinel}"
                    );
                    refresh
                        .Register(refreshSentinel, options.Environment, true)
                        .SetCacheExpiration(TimeSpan.FromSeconds(10));
                });

                opt.ConfigureKeyVault(config =>
                {
                    // NOTE: Key Vault references through Azure App Config refresh based on the below interval, regardless of the interval or Sentinel set in Azure App Config
                    config.SetCredential(credential);
                    config.SetSecretRefreshInterval(TimeSpan.FromSeconds(30));
                });
            });

            return configBuilder;
        }

        public class AppConfigOptions
        {
            public AppConfigOptions()
            {
                WriteMessage = s => Debug.WriteLine(s);
            }

            public string? AzureConfigUri { get; set; }
            public string? AppConfigManagedId { get; set; }
            public string? TenantId { get; set; }
            public string? Environment { get; set; }

            public Action<string> WriteMessage { get; set; }

            public List<AppConfigSection> AppConfigSections { get; set; } = new();

            public AppConfigOptions AddConfigSection(string path, string sectionName)
            {
                return AddConfigSection(path, new[] { sectionName });
            }

            public AppConfigOptions AddConfigSection(string path, params string[] sectionNames)
            {
                foreach (var sectionName in sectionNames)
                {
                    AppConfigSections.Add(
                        new AppConfigSection { Path = path, SectionName = sectionName }
                    );
                }

                return this;
            }
        }

        public class AppConfigSection
        {
            public string? Path { get; set; }
            public string? SectionName { get; set; }
        }

        public static JToken SerializeConfig(IConfiguration config)
        {
            JObject obj = new JObject();
            foreach (var child in config.GetChildren())
            {
                obj.Add(child.Key, SerializeConfig(child));
            }

            if (!obj.HasValues && config is IConfigurationSection section)
                return new JValue(section.Value);

            return obj;
        }

        public static bool IsRunningForNswagCodegen()
        {
            var value = System
                .Reflection.Assembly.GetEntryAssembly()!
                .FullName!.ToLowerInvariant()
                .StartsWith("nswag");
            return value;
        }
    }
}