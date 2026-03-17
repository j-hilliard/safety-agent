namespace Stronghold.AppDashboard.Data.Models.Safety;

public class RefCompany
{
    public Guid Id { get; set; }
    public string Code { get; set; } = null!;
    public string Name { get; set; } = null!;
    public int NextIncidentNumber { get; set; }
    public bool IsActive { get; set; }
    public DateTime CreatedAt { get; set; }
    public DateTime UpdatedAt { get; set; }

    public ICollection<RefRegion> Regions { get; set; } = new List<RefRegion>();
}
