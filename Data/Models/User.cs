namespace Stronghold.AppDashboard.Data.Models;

public class User
{
    public int UserId { get; set; }
    public Guid AzureAdObjectId { get; set; }
    public string? FirstName { get; set; }
    public string? LastName { get; set; }
    public string? Email { get; set; }
    public string? Company { get; set; }
    public string? Department { get; set; }
    public string? Title { get; set; }
    public bool Active { get; set; }
    public DateTimeOffset? DisabledOn { get; set; }
    public int? DisabledById { get; set; }
    public string? DisabledReason { get; set; }
    public DateTimeOffset? LastLogin { get; set; }
    public DateTimeOffset CreatedOn { get; set; } = DateTimeOffset.UtcNow;
    public DateTimeOffset ModifiedOn { get; set; } = DateTimeOffset.UtcNow;
    public ICollection<UserRole> UserRoles { get; set; } = new List<UserRole>();
}
