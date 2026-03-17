using Microsoft.EntityFrameworkCore;
using Stronghold.AppDashboard.Data;

namespace Stronghold.AppDashboard.Api.Services;

public class LogPurgeService : BackgroundService
{
    private readonly IServiceScopeFactory _scopeFactory;
    private readonly ILogger<LogPurgeService> _logger;
    private readonly IConfiguration _configuration;

    public LogPurgeService(
        IServiceScopeFactory scopeFactory,
        ILogger<LogPurgeService> logger,
        IConfiguration configuration)
    {
        _scopeFactory = scopeFactory;
        _logger = logger;
        _configuration = configuration;
    }

    protected override async Task ExecuteAsync(CancellationToken stoppingToken)
    {
        while (!stoppingToken.IsCancellationRequested)
        {
            await PurgeOldLogsAsync();
            await Task.Delay(TimeSpan.FromDays(1), stoppingToken);
        }
    }

    private async Task PurgeOldLogsAsync()
    {
        try
        {
            var retentionDays = _configuration.GetValue<int>("Logging:RetentionDays", 90);
            var cutoff = DateTime.UtcNow.AddDays(-retentionDays);

            using var scope = _scopeFactory.CreateScope();
            var context = scope.ServiceProvider.GetRequiredService<AppDbContext>();

            var deleted = await context.ProcessLogs
                .Where(p => p.LoggedAt < cutoff)
                .ExecuteDeleteAsync();

            if (deleted > 0)
                _logger.LogInformation("LogPurgeService: Deleted {Count} log entries older than {Days} days.", deleted, retentionDays);
        }
        catch (Exception ex)
        {
            _logger.LogError(ex, "LogPurgeService: Error during log purge.");
        }
    }
}
