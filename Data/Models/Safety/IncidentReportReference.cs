namespace Stronghold.AppDashboard.Data.Models.Safety;

public class IncidentReportReference
{
    public Guid IncidentReportId { get; set; }
    public Guid ReferenceId { get; set; }
    public DateTime CreatedAt { get; set; }

    public IncidentReport IncidentReport { get; set; } = null!;
    public RefIncidentReportReference Reference { get; set; } = null!;
}
