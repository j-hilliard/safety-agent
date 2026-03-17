namespace Stronghold.AppDashboard.Data.Models.Safety;

public class ProcessLog
{
    public Guid Id { get; set; }
    public Guid? IncidentReportId { get; set; }
    public string ProcessName { get; set; } = null!;
    public string ProcessType { get; set; } = null!;
    public string LogType { get; set; } = null!;
    public string Message { get; set; } = null!;
    public string? MessageDetail { get; set; }
    public string? RelatedObject { get; set; }
    public string RunId { get; set; } = null!;
    public DateTime LoggedAt { get; set; }
}
