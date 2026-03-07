namespace Stronghold.AppDashboard.Data.Models;

public class TechSupport
{
    public required int TechSupportId { get; set; }
    public required int ApplicationId { get; set; }
    public required string Name { get; set; } = null!;
    public required string Email { get; set; } = null!;
    public required string Phone { get; set; } = null!;
    public required string Title { get; set; } = null!;
    public required string Department { get; set; } = null!;
    public required string Description { get; set; } = null!;
    public DateTimeOffset CreatedOn { get; set; } = DateTimeOffset.UtcNow;
    public DateTimeOffset ModifiedOn { get; set; } = DateTimeOffset.UtcNow;
}
