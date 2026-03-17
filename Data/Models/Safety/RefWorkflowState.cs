namespace Stronghold.AppDashboard.Data.Models.Safety;

public class RefWorkflowState
{
    public Guid Id { get; set; }
    public string Domain { get; set; } = null!;
    public string Code { get; set; } = null!;
    public string Name { get; set; } = null!;
    public bool IsActive { get; set; }
    public DateTime CreatedAt { get; set; }
    public DateTime UpdatedAt { get; set; }
}
