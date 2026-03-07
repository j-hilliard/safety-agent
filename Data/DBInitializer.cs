using Stronghold.AppDashboard.Data.Models;

namespace Stronghold.AppDashboard.Data;

public static class DbInitializer
{
    public static void Initialize(AppDbContext dbContext, bool isProductionEnvironment = false)
    {
        SeedUserRoles(dbContext);
        SeedDefaultLocalUser(dbContext);
    }

    private const string AdministratorRoleDescription =
        "Users in this group are given administrator level access to the application. They can perform all actions within the application.  Administrators can manage all application listings as well as grant access within the Application Dashboard for other users.";
    private const string UserRoleDescription =
        "Users in this group are given general basic level access to the application. Users in this group are granted the lowest level of access within the application.  Users in this group can also authenticate via SSO to the application.";
    private const string ApplicationDirectoryManagerDescription =
        "Users in this group can manage the application directory. They can create, update, and delete application listings. They can also disable application listings.";
    private const string IntegratedApplicationManagerDescription =
        "Users in this group can manage the integrated applications list. They can create, update, and delete integrated applications. They can also disable integrated applications.";

    private static readonly List<(string RoleName, string Description)> Roles =
        new()
        {
            (Shared.Enumerations.AuthorizationRoles.Administrator, AdministratorRoleDescription),
            (Shared.Enumerations.AuthorizationRoles.User, UserRoleDescription),
            (
                Shared.Enumerations.AuthorizationRoles.ApplicationDirectoryManager,
                ApplicationDirectoryManagerDescription
            ),
            (
                Shared.Enumerations.AuthorizationRoles.IntegratedApplicationManager,
                IntegratedApplicationManagerDescription
            ),
        };

    private static void SeedUserRoles(AppDbContext dbContext)
    {
        if (dbContext.Roles.Any())
            return;

        foreach (var roleWithDescription in Roles)
        {
            var existingRole = dbContext.Roles.FirstOrDefault(r =>
                r.Name == roleWithDescription.RoleName
            );
            
            if (existingRole == null)
            {
                var role = new Role
                {
                    Name = roleWithDescription.RoleName,
                    Description = roleWithDescription.Description,
                };

                dbContext.Roles.Add(role);
                dbContext.SaveChanges();
            }
        }
    }

    private static void SeedDefaultLocalUser(AppDbContext dbContext)
    {
        if (dbContext.Users.Any())
            return;

        var newUser = new User
        {
            AzureAdObjectId = new Guid("00000000-0000-0000-0000-000000000000"),
            FirstName = "Local",
            LastName = "Dev Testing",
            Email = "LocalDevTesting@DevTesting.com",
            Company = "Stronghold",
            Department = "IT - App Dev",
            Title = "Software Developer",
            Active = true,
        };
        
        dbContext.Users.Add(newUser);
        dbContext.SaveChanges();

        var adminRole = dbContext.Roles.FirstOrDefault(r =>
            r.Name == Shared.Enumerations.AuthorizationRoles.Administrator
        );
        
        if (adminRole == null)
        {
            throw new Exception("Admin role not found");
        }

        var userRole = new UserRole { User = newUser, Role = adminRole };
        
        dbContext.UserRoles.Add(userRole);
        dbContext.SaveChanges();
    }
}
