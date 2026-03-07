namespace Stronghold.AppDashboard.Data.Models;

public class Settings
{
    public int SettingsId { get; set; }
    public string ADUsersGroupName { get; set; } = null!;
    public Guid ADUsersGroupGUID { get; set; }
    public string ADAdminsGroupName { get; set; } = null!;
    public Guid ADAdminsGroupGUID { get; set; }
    public DateTimeOffset CreatedOn { get; set; } = DateTimeOffset.UtcNow;
    public DateTimeOffset ModifiedOn { get; set; } = DateTimeOffset.UtcNow;
}
