namespace Stronghold.AppDashboard.Data.Models.Safety;

public class RefRegion
{
    public Guid Id { get; set; }
    public Guid? CompanyId { get; set; }
    public string Code { get; set; } = null!;
    public string Name { get; set; } = null!;
    public bool IsActive { get; set; }
    public DateTime CreatedAt { get; set; }
    public DateTime UpdatedAt { get; set; }

    public RefCompany? Company { get; set; }
}
