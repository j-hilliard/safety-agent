namespace Stronghold.AppDashboard.Data.Models;

public class SystemOwner
{
    public required int SystemOwnerId { get; set; }
    public required int ApplicationId { get; set; }
    public required string Name { get; set; } = null!;
    public required string Email { get; set; } = null!;
    public string Phone { get; set; } = null!;
    public string Title { get; set; } = null!;
    public string Department { get; set; } = null!;
    public string Description { get; set; } = null!;
    public DateTimeOffset CreatedOn { get; set; } = DateTimeOffset.UtcNow;
    public DateTimeOffset ModifiedOn { get; set; } = DateTimeOffset.UtcNow;
}
