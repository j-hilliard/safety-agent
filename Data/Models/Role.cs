namespace Stronghold.AppDashboard.Data.Models;

public class Role
{
    public int RoleId { get; set; }
    public string Name { get; set; } = null!;
    public string Description { get; set; } = null!;
    public ICollection<UserRole>? UserRoles { get; set; } = new List<UserRole>();
}
