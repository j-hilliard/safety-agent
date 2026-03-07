using Microsoft.EntityFrameworkCore;
using Stronghold.AppDashboard.Data.Models;

namespace Stronghold.AppDashboard.Data;

public class AppDbContext : DbContext
{
    public AppDbContext(DbContextOptions<AppDbContext> options)
        : base(options) { }

    public DbSet<User> Users { get; set; } = null!;
    public DbSet<Role> Roles { get; set; } = null!;
    public DbSet<UserRole> UserRoles { get; set; } = null!;
    public DbSet<Settings> Settings { get; set; } = null!;
    public DbSet<SystemOwner> SystemOwners { get; set; } = null!;
    public DbSet<TechSupport> TechSupports { get; set; } = null!;

    protected override void OnModelCreating(ModelBuilder modelBuilder)
    {
        modelBuilder.Entity<User>(typeBuilder =>
        {
            typeBuilder.HasKey(user => user.UserId);
            typeBuilder.HasIndex(user => user.AzureAdObjectId).IsUnique();
            typeBuilder.Property(user => user.AzureAdObjectId).IsRequired();
            typeBuilder
                .HasOne<User>()
                .WithMany()
                .HasForeignKey(user => user.DisabledById)
                .OnDelete(DeleteBehavior.Restrict);
        });

        modelBuilder.Entity<Role>(typeBuilder =>
        {
            typeBuilder.HasKey(role => role.RoleId);
            typeBuilder.Property(role => role.Name).IsRequired();
            typeBuilder.Property(role => role.Description).IsRequired();
        });

        modelBuilder.Entity<UserRole>(typeBuilder =>
        {
            typeBuilder.HasKey(userRole => new { userRole.UserId, userRole.RoleId });
            typeBuilder.HasIndex(userRole => new { userRole.UserId, userRole.RoleId }).IsUnique();

            // Relationships are inferred, no need to specify foreign keys
            typeBuilder
                .HasOne(userRole => userRole.User)
                .WithMany(user => user.UserRoles)
                .OnDelete(DeleteBehavior.Restrict);

            typeBuilder
                .HasOne(userRole => userRole.Role)
                .WithMany(role => role.UserRoles)
                .OnDelete(DeleteBehavior.Restrict);
        });

        modelBuilder.Entity<Settings>(typeBuilder =>
        {
            typeBuilder.HasKey(settings => settings.SettingsId);
            typeBuilder.HasIndex(s => s.ADUsersGroupGUID).IsUnique();
            typeBuilder.HasIndex(s => s.ADAdminsGroupGUID).IsUnique();

            typeBuilder.Property(s => s.ADUsersGroupName).IsRequired();
            typeBuilder.Property(s => s.ADUsersGroupGUID).IsRequired();
            typeBuilder.Property(s => s.ADAdminsGroupName).IsRequired();
            typeBuilder.Property(s => s.ADAdminsGroupGUID).IsRequired();
        });

        modelBuilder.Entity<SystemOwner>(typeBuilder =>
        {
            typeBuilder.HasKey(systemOwner => systemOwner.SystemOwnerId);
            typeBuilder.Property(systemOwner => systemOwner.Name).IsRequired();
        });

        modelBuilder.Entity<TechSupport>(typeBuilder =>
        {
            typeBuilder.HasKey(techSupport => techSupport.TechSupportId);
            typeBuilder.Property(techSupport => techSupport.Name).IsRequired();
        });
    }

    private void ApplyAuditInfo()
    {
        var now = DateTimeOffset.UtcNow;
        var addedModifiedEntries = ChangeTracker
            .Entries()
            .Where(e =>
                (
                    e.Entity is Settings
                    || e.Entity is SystemOwner
                    || e.Entity is TechSupport
                    || e.Entity is User
                ) && (e.State == EntityState.Added || e.State == EntityState.Modified)
            );

        foreach (var entry in addedModifiedEntries)
        {
            if (entry.Entity is SystemOwner systemOwnerEntry)
            {
                systemOwnerEntry.ModifiedOn = now;

                if (entry.State == EntityState.Added)
                    systemOwnerEntry.CreatedOn = now;
            }
            else if (entry.Entity is TechSupport techSupportEntry)
            {
                techSupportEntry.ModifiedOn = now;

                if (entry.State == EntityState.Added)
                    techSupportEntry.CreatedOn = now;
            }
            else if (entry.Entity is User userEntry)
            {
                userEntry.ModifiedOn = now;

                if (entry.State == EntityState.Added)
                    userEntry.CreatedOn = now;
            }
            else if (entry.Entity is Settings settingsEntry)
            {
                settingsEntry.ModifiedOn = now;

                if (entry.State == EntityState.Added)
                    settingsEntry.CreatedOn = now;
            }
        }
    }

    public override int SaveChanges()
    {
        ApplyAuditInfo();
        return base.SaveChanges();
    }

    public override int SaveChanges(bool acceptAllChangesOnSuccess)
    {
        ApplyAuditInfo();
        return base.SaveChanges(acceptAllChangesOnSuccess);
    }

    public override Task<int> SaveChangesAsync(
        bool acceptAllChangesOnSuccess,
        CancellationToken cancellationToken = new()
    )
    {
        ApplyAuditInfo();
        return base.SaveChangesAsync(acceptAllChangesOnSuccess, cancellationToken);
    }

    public override Task<int> SaveChangesAsync(CancellationToken cancellationToken = new())
    {
        ApplyAuditInfo();
        return base.SaveChangesAsync(cancellationToken);
    }
}
