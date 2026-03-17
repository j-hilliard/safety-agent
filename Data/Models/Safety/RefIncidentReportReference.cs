namespace Stronghold.AppDashboard.Data.Models.Safety;

public class RefIncidentReportReference
{
    public Guid Id { get; set; }
    public Guid ReferenceTypeId { get; set; }
    public string Code { get; set; } = null!;
    public string Name { get; set; } = null!;
    public bool IsActive { get; set; }
    public DateTime CreatedAt { get; set; }
    public DateTime UpdatedAt { get; set; }

    public RefReferenceType ReferenceType { get; set; } = null!;
}
