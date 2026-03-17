namespace Stronghold.AppDashboard.Data.Models.Safety;

public class RefInvestigationReference
{
    public Guid Id { get; set; }
    public Guid ReferenceTypeId { get; set; }
    public string Code { get; set; } = null!;
    public string Name { get; set; } = null!;
    public bool IsActive { get; set; }
    public DateTime CreatedAt { get; set; }
    public DateTime UpdatedAt { get; set; }

    public InvestigationAttributeType ReferenceType { get; set; } = null!;
}
