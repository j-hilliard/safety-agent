namespace Stronghold.AppDashboard.Data.Models.Safety;

public class InvestigationAttributeType
{
    public Guid Id { get; set; }
    public string Code { get; set; } = null!;
    public string Name { get; set; } = null!;
    public string AppliesTo { get; set; } = null!;
    public bool IsActive { get; set; }
    public DateTime CreatedAt { get; set; }
    public DateTime UpdatedAt { get; set; }

    public ICollection<RefInvestigationReference> InvestigationReferences { get; set; } = new List<RefInvestigationReference>();
}
