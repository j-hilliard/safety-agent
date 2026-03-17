using Microsoft.EntityFrameworkCore;
using Stronghold.AppDashboard.Data.Models;
using Stronghold.AppDashboard.Data.Models.Safety;

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

    // Safety schema
    public DbSet<IncidentReport> IncidentReports { get; set; } = null!;
    public DbSet<IncidentEmployeeInvolved> IncidentEmployeesInvolved { get; set; } = null!;
    public DbSet<IncidentAction> IncidentActions { get; set; } = null!;
    public DbSet<IncidentReportReference> IncidentReportReferences { get; set; } = null!;
    public DbSet<RefCompany> Companies { get; set; } = null!;
    public DbSet<RefRegion> Regions { get; set; } = null!;
    public DbSet<RefSeverity> SeveritiesActual { get; set; } = null!;
    public DbSet<RefSeverity> SeveritiesPotential { get; set; } = null!;
    public DbSet<RefReferenceType> ReferenceTypes { get; set; } = null!;
    public DbSet<InvestigationAttributeType> InvestigationAttributeTypes { get; set; } = null!;
    public DbSet<RefIncidentReportReference> IncidentReportReferenceOptions { get; set; } = null!;
    public DbSet<RefDocType> DocTypeOptions { get; set; } = null!;
    public DbSet<RefInvestigationReference> InvestigationReferenceOptions { get; set; } = null!;
    public DbSet<RefWorkflowState> WorkflowStates { get; set; } = null!;
    public DbSet<ProcessLog> ProcessLogs { get; set; } = null!;

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

        // Safety schema — mapped to ERD v1 table/column names

        modelBuilder.Entity<IncidentReport>(b =>
        {
            b.ToTable("Incident", "safety");
            b.HasKey(r => r.Id);
            b.Property(r => r.Id).HasColumnName("incident_id");
            b.Property(r => r.IncidentNumber).HasColumnName("incidentNumber").IsRequired();
            b.Property(r => r.Status).HasColumnName("status").IsRequired();
            b.Property(r => r.IncidentDate).HasColumnName("incidentDate");
            b.Property(r => r.CompanyId).HasColumnName("company_id");
            b.Property(r => r.RegionId).HasColumnName("region_id");
            b.Property(r => r.JobNumber).HasColumnName("jobNumber");
            b.Property(r => r.ClientCode).HasColumnName("customer");
            b.Property(r => r.PlantCode).HasColumnName("locaton");
            b.Property(r => r.WorkDescription).HasColumnName("workDescription");
            b.Property(r => r.IncidentSummary).HasColumnName("incidentSummary");
            b.Property(r => r.IncidentClass).HasColumnName("incidentClass");
            b.Property(r => r.SeverityActualCode).HasColumnName("severityActualCode");
            b.Property(r => r.SeverityPotentialCode).HasColumnName("severityPotentialCode");
            b.Property(r => r.HealthSafetyLeaderId).HasColumnName("healthSafetyLeader_id");
            b.Property(r => r.SeniorOpsLeaderId).HasColumnName("seniorOpsLeader_id");
            b.Property(r => r.CreatedAt).HasColumnName("createdAt");
            b.Property(r => r.UpdatedAt).HasColumnName("updatedAt");
            // Fields removed from new schema — ignored so EF doesn't try to find these columns
            b.Ignore(r => r.BodyPartsInjured);
            b.Ignore(r => r.NatureOfInjury);
            b.Ignore(r => r.TypeOfEquipment);
            b.Ignore(r => r.UnitNumbers);
            b.Ignore(r => r.Visibility);
            b.Ignore(r => r.InvestigationDetails);
            b.Ignore(r => r.FormalInvestigationRequired);
            b.Ignore(r => r.FullCauseMapRequired);
            b.HasOne(r => r.Company).WithMany().HasForeignKey(r => r.CompanyId).OnDelete(DeleteBehavior.Restrict);
            b.HasOne(r => r.Region).WithMany().HasForeignKey(r => r.RegionId).OnDelete(DeleteBehavior.Restrict);
            b.HasMany(r => r.EmployeesInvolved).WithOne(e => e.IncidentReport).HasForeignKey(e => e.IncidentReportId).OnDelete(DeleteBehavior.Cascade);
            b.HasMany(r => r.Actions).WithOne(a => a.IncidentReport).HasForeignKey(a => a.IncidentReportId).OnDelete(DeleteBehavior.Cascade);
            b.HasMany(r => r.References).WithOne(rf => rf.IncidentReport).HasForeignKey(rf => rf.IncidentReportId).OnDelete(DeleteBehavior.Cascade);
        });

        modelBuilder.Entity<IncidentEmployeeInvolved>(b =>
        {
            b.ToTable("Employee_Involved", "safety");
            b.HasKey(e => e.Id);
            b.Property(e => e.Id).HasColumnName("id");
            b.Property(e => e.IncidentReportId).HasColumnName("incident_id");
            b.Property(e => e.EmployeeIdentifier).HasColumnName("emp_id");
            b.Property(e => e.HoursWorked).HasColumnName("hrsWorked");
            b.Property(e => e.CreatedAt).HasColumnName("createdAt");
            b.Property(e => e.UpdatedAt).HasColumnName("updatedAt");
            // Fields removed from new schema
            b.Ignore(e => e.EmployeeName);
            b.Ignore(e => e.InjuryTypeCode);
            b.Ignore(e => e.Recordable);
        });

        modelBuilder.Entity<IncidentAction>(b =>
        {
            b.ToTable("Action_Taken", "safety");
            b.HasKey(a => a.Id);
            b.Property(a => a.Id).HasColumnName("action_id");
            b.Property(a => a.IncidentReportId).HasColumnName("incident_id");
            b.Property(a => a.ActionType).HasColumnName("actionType");
            b.Property(a => a.ActionDescription).HasColumnName("actionTaken");
            b.Property(a => a.AssignedTo).HasColumnName("Person");
            b.Property(a => a.DueDate).HasColumnName("dueDate");
            b.Property(a => a.CreatedAt).HasColumnName("createdAt");
            b.Property(a => a.UpdatedAt).HasColumnName("updatedAt");
            // Fields removed from new schema
            b.Ignore(a => a.Status);
            b.Ignore(a => a.ClosedAt);
        });

        modelBuilder.Entity<IncidentReportReference>(b =>
        {
            b.ToTable("IncidentAttribute_Assignment", "safety");
            b.HasKey(r => new { r.IncidentReportId, r.ReferenceId });
            b.Property(r => r.IncidentReportId).HasColumnName("incident_id");
            b.Property(r => r.ReferenceId).HasColumnName("incidentAttr_id");
            b.Property(r => r.CreatedAt).HasColumnName("createdAt");
            b.HasOne(r => r.Reference).WithMany().HasForeignKey(r => r.ReferenceId).OnDelete(DeleteBehavior.Restrict);
        });

        modelBuilder.Entity<RefCompany>(b =>
        {
            b.ToTable("SHC_Company", "safety");
            b.HasKey(c => c.Id);
            b.Property(c => c.Id).HasColumnName("company_id");
            b.Property(c => c.Code).HasColumnName("code").IsRequired();
            b.Property(c => c.Name).HasColumnName("name").IsRequired();
            b.Property(c => c.NextIncidentNumber).HasColumnName("next_incident_number");
            b.Property(c => c.IsActive).HasColumnName("active");
            b.Property(c => c.CreatedAt).HasColumnName("createdAt");
            b.Property(c => c.UpdatedAt).HasColumnName("updatedAt");
            b.HasMany(c => c.Regions).WithOne(r => r.Company).HasForeignKey(r => r.CompanyId).OnDelete(DeleteBehavior.Restrict);
        });

        modelBuilder.Entity<RefRegion>(b =>
        {
            b.ToTable("SHC_Region", "safety");
            b.HasKey(r => r.Id);
            b.Property(r => r.Id).HasColumnName("region_id");
            b.Property(r => r.CompanyId).HasColumnName("company_id");
            b.Property(r => r.Code).HasColumnName("code").IsRequired();
            b.Property(r => r.Name).HasColumnName("name").IsRequired();
            b.Property(r => r.IsActive).HasColumnName("active");
            b.Property(r => r.CreatedAt).HasColumnName("createdAt");
            b.Property(r => r.UpdatedAt).HasColumnName("updatedAt");
        });

        modelBuilder.SharedTypeEntity<RefSeverity>("SeveritiesActual", b =>
        {
            b.ToTable("ref_severity_actual", "safety");
            b.HasKey(s => s.Id);
            b.Property(s => s.Id).HasColumnName("id");
            b.Property(s => s.Code).HasColumnName("code").IsRequired();
            b.Property(s => s.Name).HasColumnName("name").IsRequired();
            b.Property(s => s.Rank).HasColumnName("rank");
            b.Property(s => s.IsActive).HasColumnName("is_active");
            b.Property(s => s.CreatedAt).HasColumnName("createdAt");
            b.Property(s => s.UpdatedAt).HasColumnName("updatedAt");
        });

        modelBuilder.SharedTypeEntity<RefSeverity>("SeveritiesPotential", b =>
        {
            b.ToTable("ref_severity_potential", "safety");
            b.HasKey(s => s.Id);
            b.Property(s => s.Id).HasColumnName("id");
            b.Property(s => s.Code).HasColumnName("code").IsRequired();
            b.Property(s => s.Name).HasColumnName("name").IsRequired();
            b.Property(s => s.Rank).HasColumnName("rank");
            b.Property(s => s.IsActive).HasColumnName("is_active");
            b.Property(s => s.CreatedAt).HasColumnName("createdAt");
            b.Property(s => s.UpdatedAt).HasColumnName("updatedAt");
        });

        modelBuilder.Entity<RefReferenceType>(b =>
        {
            b.ToTable("IncidentAttribute_Type", "safety");
            b.HasKey(r => r.Id);
            b.Property(r => r.Id).HasColumnName("incidentAttrType_id");
            b.Property(r => r.Code).HasColumnName("code").IsRequired();
            b.Property(r => r.Name).HasColumnName("name").IsRequired();
            b.Property(r => r.AppliesTo).HasColumnName("applies_to").IsRequired();
            b.Property(r => r.IsActive).HasColumnName("active");
            b.Property(r => r.CreatedAt).HasColumnName("createdAt");
            b.Property(r => r.UpdatedAt).HasColumnName("updatedAt");
            b.HasMany(r => r.IncidentReportReferences).WithOne(i => i.ReferenceType).HasForeignKey(i => i.ReferenceTypeId).OnDelete(DeleteBehavior.Restrict);
        });

        modelBuilder.Entity<InvestigationAttributeType>(b =>
        {
            b.ToTable("InvestigationAttribute_Type", "safety");
            b.HasKey(r => r.Id);
            b.Property(r => r.Id).HasColumnName("investAttrType_id");
            b.Property(r => r.Code).HasColumnName("code").IsRequired();
            b.Property(r => r.Name).HasColumnName("name").IsRequired();
            b.Property(r => r.AppliesTo).HasColumnName("applies_to").IsRequired();
            b.Property(r => r.IsActive).HasColumnName("active");
            b.Property(r => r.CreatedAt).HasColumnName("createdAt");
            b.Property(r => r.UpdatedAt).HasColumnName("updatedAt");
            b.HasMany(r => r.InvestigationReferences).WithOne(i => i.ReferenceType).HasForeignKey(i => i.ReferenceTypeId).OnDelete(DeleteBehavior.Restrict);
        });

        modelBuilder.Entity<RefIncidentReportReference>(b =>
        {
            b.ToTable("IncidentAttribute", "safety");
            b.HasKey(r => r.Id);
            b.Property(r => r.Id).HasColumnName("incidentAttr_id");
            b.Property(r => r.ReferenceTypeId).HasColumnName("incidentAttrType_id");
            b.Property(r => r.Code).HasColumnName("code").IsRequired();
            b.Property(r => r.Name).HasColumnName("name").IsRequired();
            b.Property(r => r.IsActive).HasColumnName("active");
            b.Property(r => r.CreatedAt).HasColumnName("createdAt");
            b.Property(r => r.UpdatedAt).HasColumnName("updatedAt");
        });

        modelBuilder.Entity<RefDocType>(b =>
        {
            b.ToTable("Document_Type", "safety");
            b.HasKey(r => r.Id);
            b.Property(r => r.Id).HasColumnName("docType_id");
            b.Property(r => r.Code).HasColumnName("code").IsRequired();
            b.Property(r => r.Name).HasColumnName("name").IsRequired();
            b.Property(r => r.IsActive).HasColumnName("active");
            b.Property(r => r.CreatedAt).HasColumnName("createdAt");
            b.Property(r => r.UpdatedAt).HasColumnName("updatedAt");
            b.Property(r => r.AcceptedFileExt).HasColumnName("acceptedFileExt");
        });

        modelBuilder.Entity<RefInvestigationReference>(b =>
        {
            b.ToTable("InvestigationAttribute", "safety");
            b.HasKey(r => r.Id);
            b.Property(r => r.Id).HasColumnName("investAttr_id");
            b.Property(r => r.ReferenceTypeId).HasColumnName("investAttrType_id");
            b.Property(r => r.Code).HasColumnName("code").IsRequired();
            b.Property(r => r.Name).HasColumnName("name").IsRequired();
            b.Property(r => r.IsActive).HasColumnName("active");
            b.Property(r => r.CreatedAt).HasColumnName("createdAt");
            b.Property(r => r.UpdatedAt).HasColumnName("updatedAt");
        });

        modelBuilder.Entity<RefWorkflowState>(b =>
        {
            b.ToTable("ref_workflow_state", "safety");
            b.HasKey(s => s.Id);
            b.Property(s => s.Id).HasColumnName("id");
            b.Property(s => s.Domain).HasColumnName("domain").IsRequired();
            b.Property(s => s.Code).HasColumnName("code").IsRequired();
            b.Property(s => s.Name).HasColumnName("name").IsRequired();
            b.Property(s => s.IsActive).HasColumnName("is_active");
            b.Property(s => s.CreatedAt).HasColumnName("createdAt");
            b.Property(s => s.UpdatedAt).HasColumnName("updatedAt");
        });

        modelBuilder.Entity<ProcessLog>(b =>
        {
            b.ToTable("Process_Log", "safety");
            b.HasKey(p => p.Id);
            b.Property(p => p.Id).HasColumnName("log_id");
            b.Property(p => p.IncidentReportId).HasColumnName("incident_id");
            b.Property(p => p.ProcessName).HasColumnName("processName").IsRequired();
            b.Property(p => p.ProcessType).HasColumnName("processType").IsRequired();
            b.Property(p => p.LogType).HasColumnName("logType").IsRequired();
            b.Property(p => p.Message).HasColumnName("message").IsRequired();
            b.Property(p => p.MessageDetail).HasColumnName("messageDetail");
            b.Property(p => p.RelatedObject).HasColumnName("relatedObject");
            b.Property(p => p.RunId).HasColumnName("runID").IsRequired();
            b.Property(p => p.LoggedAt).HasColumnName("timeStamp");
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
