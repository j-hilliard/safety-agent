namespace Stronghold.AppDashboard.Data.Models.Safety;

public class IncidentEmployeeInvolved
{
    public Guid Id { get; set; }
    public Guid IncidentReportId { get; set; }
    public string? EmployeeIdentifier { get; set; }
    public string? EmployeeName { get; set; }
    public string? InjuryTypeCode { get; set; }
    public bool? Recordable { get; set; }
    public decimal? HoursWorked { get; set; }
    public DateTime CreatedAt { get; set; }
    public DateTime UpdatedAt { get; set; }

    public IncidentReport IncidentReport { get; set; } = null!;
}
