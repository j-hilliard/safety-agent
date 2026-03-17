namespace Stronghold.AppDashboard.Data.Models.Safety;

public class RefReferenceType
{
    public Guid Id { get; set; }
    public string Code { get; set; } = null!;
    public string Name { get; set; } = null!;
    public string AppliesTo { get; set; } = null!;
    public bool IsActive { get; set; }
    public DateTime CreatedAt { get; set; }
    public DateTime UpdatedAt { get; set; }

    public ICollection<RefIncidentReportReference> IncidentReportReferences { get; set; } = new List<RefIncidentReportReference>();
}
