namespace Stronghold.AppDashboard.Data.Models.Safety;

public class IncidentAction
{
    public Guid Id { get; set; }
    public Guid IncidentReportId { get; set; }
    public string? ActionType { get; set; }
    public string? ActionDescription { get; set; }
    public string? AssignedTo { get; set; }
    public DateTime? DueDate { get; set; }
    public string? Status { get; set; }
    public DateTime? ClosedAt { get; set; }
    public DateTime CreatedAt { get; set; }
    public DateTime UpdatedAt { get; set; }

    public IncidentReport IncidentReport { get; set; } = null!;
}
