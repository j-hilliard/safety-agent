using System.Diagnostics;
using FluentValidation;
using MediatR;
using Microsoft.EntityFrameworkCore;
using Microsoft.Identity.Client;
using Microsoft.Identity.Web;
using NSwag;
using NSwag.AspNetCore;
using NSwag.Generation.Processors.Security;
using Stronghold.AppDashboard.Api.Authorization;
using Stronghold.AppDashboard.Api.Configuration;
using Stronghold.AppDashboard.Api.Domain;
using Stronghold.AppDashboard.Api.Services;
using Stronghold.AppDashboard.Data;
using ZymLabs.NSwag.FluentValidation;

string tenantId = "78d53608-54ca-4a74-8beb-8a1399c1189c";
string clientId = "619f5cca-8c0c-465c-8cfc-25427697f82c";

var builder = WebApplication.CreateBuilder(args);
var environment = builder.Environment.EnvironmentName;
var isLocal = builder.Environment.IsEnvironment("Local");

if (isLocal)
{
    // Local dev often runs without permission to write Windows EventLog.
    // Keep logs visible in terminal/debug output and avoid startup crashes.
    builder.Logging.ClearProviders();
    builder.Logging.AddConsole();
    builder.Logging.AddDebug();
}

#pragma warning disable ASP0013
builder.Host.ConfigureAppConfiguration(configurationBuilder =>
{
    const string path = "CoreApi/";

    // Add Azure App Config Service as second to last
    configurationBuilder.ConfigureApp(opt =>
    {
        opt.Environment = environment;
        opt.TenantId = Environment.GetEnvironmentVariable("TenantId");
        opt.AzureConfigUri = Environment.GetEnvironmentVariable("AppConfigUri");
        opt.AppConfigManagedId = Environment.GetEnvironmentVariable("AppConfigManagedId");

        opt.AddConfigSection(path, ConfigurationSections.AzureAd);
        opt.AddConfigSection(path, ConfigurationSections.Logging);
        opt.AddConfigSection(path, ConfigurationSections.AllowedHosts);
        opt.AddConfigSection(path, ConfigurationSections.ConnectionStrings);
        opt.AddConfigSection(path, ConfigurationSections.ApplicationInsights);
        opt.AddConfigSection(path, ConfigurationSections.AppRegSecretForGraphAccess);
    });

    // Add appsettings.{environment}.json last to override configuration on developers' local machines
    configurationBuilder.AddJsonFile($"appsettings.{environment}.json", true, true);

    if (isLocal)
        configurationBuilder.AddUserSecrets<Program>();
});
#pragma warning restore ASP0013

builder.Services.Configure<AppConfigExtensions.AppConfigOptions>(
    builder.Configuration.GetSection(ConfigurationSections.AzureAd)
);
builder.Services.Configure<AppConfigExtensions.AppConfigOptions>(
    builder.Configuration.GetSection(ConfigurationSections.Logging)
);
builder.Services.Configure<AppConfigExtensions.AppConfigOptions>(
    builder.Configuration.GetSection(ConfigurationSections.AllowedHosts)
);
builder.Services.Configure<AppConfigExtensions.AppConfigOptions>(
    builder.Configuration.GetSection(ConfigurationSections.ConnectionStrings)
);
builder.Services.Configure<AppConfigExtensions.AppConfigOptions>(
    builder.Configuration.GetSection(ConfigurationSections.ApplicationInsights)
);
builder.Services.Configure<AppConfigExtensions.AppConfigOptions>(
    builder.Configuration.GetSection(ConfigurationSections.AppRegSecretForGraphAccess)
);

if ((environment == "Development" || environment == "Production") && !AppConfigExtensions.IsRunningForNswagCodegen())
{
    var azureAdSection = builder.Configuration.GetSection("AzureAd");

    if (azureAdSection == null)
        throw new ArgumentException(
            "AzureAd section not configured correctly and/or not loaded from App Config Service"
        );

    tenantId = azureAdSection["TenantId"] ?? "";
    clientId = azureAdSection["ClientId"] ?? "";

    if (string.IsNullOrEmpty(tenantId))
        throw new ArgumentException(
            "AzureAd section not configured correctly and/or Tenant ID not loaded from App Config Service"
        );

    if (string.IsNullOrEmpty(clientId))
        throw new ArgumentException(
            "AzureAd section not configured correctly and/or Client ID not loaded from App Config Service"
        );
}

builder.Services.AddMemoryCache();
builder.Services.AddControllers();
builder.Services.AddValidatorsFromAssemblyContaining<Program>();

// Logging services
builder.Services.AddScoped<IProcessLogService, ProcessLogService>();
if (!AppConfigExtensions.IsRunningForNswagCodegen())
    builder.Services.AddHostedService<LogPurgeService>();

// Auth
builder.Services.AddScoped<GraphService>();
if (!isLocal)
    builder.Services.AddAzureAppConfiguration();
builder.Services.AddSingleton<AzureAdHelper>();
if (isLocal)
{
    // Bypass Azure AD in local dev - accepts all requests as the seeded local user.
    builder.Services.AddAuthentication(Stronghold.AppDashboard.Api.Helpers.Constants.AuthenticationSchemes.AzureAd)
        .AddScheme<Microsoft.AspNetCore.Authentication.AuthenticationSchemeOptions, Stronghold.AppDashboard.Api.Authorization.LocalDevAuthHandler>(
            Stronghold.AppDashboard.Api.Helpers.Constants.AuthenticationSchemes.AzureAd, _ => { });
}
else
{
    builder
        .Services.AddAuthentication()
        .AddMicrosoftIdentityWebApi(
            builder.Configuration,
            ConfigurationSections.AzureAd,
            ConfigurationSections.AzureAd
        );
}
builder.Services.AddSingleton<IPublicClientApplication>(sp =>
    PublicClientApplicationBuilder
        .Create(clientId)
        .WithAuthority(new Uri($"https://login.microsoftonline.com/{tenantId}"))
        .Build()
);

// Versioning
builder.Services.AddEndpointsApiExplorer();
builder.Services.AddApiVersioning();
builder.Services.AddVersionedApiExplorer(opt =>
{
    opt.GroupNameFormat = "'v'V";
    opt.SubstituteApiVersionInUrl = true;
});

// NSwag
builder.Services.AddSingleton<FluentValidationSchemaProcessor>();
builder.Services.AddOpenApiDocument(
    (configure, serviceProvider) =>
    {
        configure.Version = "v1";
        configure.DocumentName = "v1";
        configure.ApiGroupNames = new[] { "v1" };
        configure.FlattenInheritanceHierarchy = false;
        configure.Title = "Stronghold App Template API";
        configure.SchemaProcessors.Add(
            serviceProvider.GetService<FluentValidationSchemaProcessor>()
        );
        configure.OperationProcessors.Add(new AspNetCoreOperationSecurityScopeProcessor("OAuth2"));
        configure.AddSecurity(
            "OAuth2",
            Enumerable.Empty<string>(),
            new NSwag.OpenApiSecurityScheme
            {
                Flow = OpenApiOAuth2Flow.Implicit,
                Description = "AAD authentication",
                Type = OpenApiSecuritySchemeType.OAuth2,
                Flows = new OpenApiOAuthFlows
                {
                    Implicit = new OpenApiOAuthFlow
                    {
                        AuthorizationUrl =
                            "https://login.microsoftonline.com/78d53608-54ca-4a74-8beb-8a1399c1189c/oauth2/v2.0/authorize",
                        TokenUrl =
                            "https://login.microsoftonline.com/78d53608-54ca-4a74-8beb-8a1399c1189c/oauth2/v2.0/token",
                        Scopes = new Dictionary<string, string>
                        {
                            {
                                $"api://{clientId}/default",
                                "Access the API as the signed-in user"
                            },
                        },
                    },
                },
            }
        );
    }
);

#if DEBUG
builder.Services.AddDbContextFactory<AppDbContext>(options =>
    options
        .UseSqlServer(
            builder.Configuration.GetConnectionString("SqlDb"),
            providerOptions =>
                providerOptions
                    .EnableRetryOnFailure()
                    .UseQuerySplittingBehavior(QuerySplittingBehavior.SplitQuery)
        )
        .EnableSensitiveDataLogging()
);
#else
builder.Services.AddDbContextFactory<AppDbContext>(options =>
    options.UseSqlServer(
        builder.Configuration.GetConnectionString("SqlDb"),
        providerOptions =>
            providerOptions
                .EnableRetryOnFailure()
                .UseQuerySplittingBehavior(QuerySplittingBehavior.SplitQuery)
    )
);
#endif

builder.Services.AddHttpContextAccessor();
builder.Services.AddDatabaseDeveloperPageExceptionFilter();
builder.Services.AddAutoMapper(AppDomain.CurrentDomain.GetAssemblies());
builder.Services.AddTransient(typeof(IPipelineBehavior<,>), typeof(LoggingBehavior<,>));
builder.Services.AddTransient(typeof(IPipelineBehavior<,>), typeof(AuthorizationBehavior<,>));
builder.Services.AddMediatR(configuration =>
    configuration.RegisterServicesFromAssemblyContaining<Program>()
);

var app = builder.Build();

if (app.Environment.IsDevelopment() || app.Environment.IsEnvironment("Local"))
{
    app.UseOpenApi();
    app.UseMigrationsEndPoint();
    app.UseDeveloperExceptionPage();
    app.UseSwaggerUi3(settings =>
    {
        settings.OAuth2Client = new OAuth2ClientSettings
        {
            ClientId = clientId,
            AppName = "Swagger",
        };
    });

    app.Use(
        async (_, next) =>
        {
            var configJsonString = AppConfigExtensions.SerializeConfig(builder.Configuration);
            Debug.WriteLine($"******************** Configuration Values: {configJsonString}");
            await next();
        }
    );
}
else
{
    app.UseHsts();
}

// If building an NSwag client, don't use Azure App Config
if (!AppConfigExtensions.IsRunningForNswagCodegen())
{
    if (!isLocal)
        app.UseAzureAppConfiguration();

    using var scope = app.Services.CreateScope();
    var services = scope.ServiceProvider;
    var context = services.GetRequiredService<AppDbContext>();

    if (app.Environment.IsEnvironment("Local") || app.Environment.IsDevelopment())
    {
        context.Database.EnsureCreated();
        DbInitializer.Initialize(context);
    }
    else if (app.Environment.IsProduction())
    {
        DbInitializer.Initialize(context, true);
    }
}

app.UseCors(policyBuilder => policyBuilder.AllowAnyOrigin().AllowAnyMethod().AllowAnyHeader());
app.UseHttpsRedirection();
app.UseAuthentication();
app.UseAuthorization();
app.MapControllers();
app.Run();
