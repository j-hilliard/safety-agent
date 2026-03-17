using Stronghold.AppDashboard.Data;
using Stronghold.AppDashboard.Data.Models.Safety;

namespace Stronghold.AppDashboard.Api.Services;

public interface IProcessLogService
{
    Task LogAsync(
        string processName,
        string processType,
        string logType,
        string message,
        Guid? incidentReportId = null,
        string? messageDetail = null,
        string? relatedObject = null);
}

public class ProcessLogService : IProcessLogService
{
    private readonly AppDbContext _context;
    private readonly string _runId;

    public ProcessLogService(AppDbContext context, IHttpContextAccessor httpContextAccessor)
    {
        _context = context;
        // Use trace identifier per request as the run_id so all logs in one request share an ID
        _runId = httpContextAccessor.HttpContext?.TraceIdentifier ?? Guid.NewGuid().ToString();
    }

    public async Task LogAsync(
        string processName,
        string processType,
        string logType,
        string message,
        Guid? incidentReportId = null,
        string? messageDetail = null,
        string? relatedObject = null)
    {
        var entry = new ProcessLog
        {
            Id = Guid.NewGuid(),
            IncidentReportId = incidentReportId,
            ProcessName = processName,
            ProcessType = processType,
            LogType = logType,
            Message = message,
            MessageDetail = messageDetail,
            RelatedObject = relatedObject,
            RunId = _runId,
            LoggedAt = DateTime.UtcNow
        };

        await _context.ProcessLogs.AddAsync(entry);
        await _context.SaveChangesAsync();
    }
}
