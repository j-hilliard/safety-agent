using Stronghold.AppDashboard.Data.Models;
using Stronghold.AppDashboard.Data.Models.Safety;

namespace Stronghold.AppDashboard.Data;

public static class DbInitializer
{
    public static void Initialize(AppDbContext dbContext, bool isProductionEnvironment = false)
    {
        SeedUserRoles(dbContext);
        SeedDefaultLocalUser(dbContext);
        SeedReferenceData(dbContext);
        SeedCompaniesRegionsAndSeverities(dbContext);
        SeedTestIncidents(dbContext);
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

    private static void SeedReferenceData(AppDbContext dbContext)
    {
        if (dbContext.ReferenceTypes.Any())
            return;

        var now = DateTime.UtcNow;

        // IncidentAttribute_Type — incident report reference types only
        var refType_environmentaltype = new RefReferenceType { Id = new Guid("8069613e-50e5-4436-9bc5-788117d94780"), Code = "environmentaltype", Name = "environmentaltype", AppliesTo = "incident_report", IsActive = true, CreatedAt = now, UpdatedAt = now };
        dbContext.ReferenceTypes.Add(refType_environmentaltype);
        var refType_equipmentdamage = new RefReferenceType { Id = new Guid("cf5cc886-fbc0-40f5-a771-ffe3f7b1e7df"), Code = "equipmentdamage", Name = "equipmentdamage", AppliesTo = "incident_report", IsActive = true, CreatedAt = now, UpdatedAt = now };
        dbContext.ReferenceTypes.Add(refType_equipmentdamage);
        var refType_incidenttype = new RefReferenceType { Id = new Guid("02513600-eb20-41e7-b2ff-e485d41c8e9c"), Code = "incidenttype", Name = "incidenttype", AppliesTo = "incident_report", IsActive = true, CreatedAt = now, UpdatedAt = now };
        dbContext.ReferenceTypes.Add(refType_incidenttype);
        var refType_injurydetail = new RefReferenceType { Id = new Guid("3f35c242-5a38-4614-88bb-f843e1334f9b"), Code = "injurydetail", Name = "injurydetail", AppliesTo = "incident_report", IsActive = true, CreatedAt = now, UpdatedAt = now };
        dbContext.ReferenceTypes.Add(refType_injurydetail);
        var refType_injurytype = new RefReferenceType { Id = new Guid("eac8e144-c4c9-4387-be53-37e0b557b232"), Code = "injurytype", Name = "injurytype", AppliesTo = "incident_report", IsActive = true, CreatedAt = now, UpdatedAt = now };
        dbContext.ReferenceTypes.Add(refType_injurytype);
        var refType_motorvehicleaccident = new RefReferenceType { Id = new Guid("4ce0c493-50b3-487f-b8a6-d5a211c2da21"), Code = "motorvehicleaccident", Name = "motorvehicleaccident", AppliesTo = "incident_report", IsActive = true, CreatedAt = now, UpdatedAt = now };
        dbContext.ReferenceTypes.Add(refType_motorvehicleaccident);
        var refType_policydeviation = new RefReferenceType { Id = new Guid("d39c182a-1f3b-4a5f-924f-c366e2d245ae"), Code = "policydeviation", Name = "policydeviation", AppliesTo = "incident_report", IsActive = true, CreatedAt = now, UpdatedAt = now };
        dbContext.ReferenceTypes.Add(refType_policydeviation);

        // InvestigationAttribute_Type — investigation reference types only
        var refType_jobfactors = new InvestigationAttributeType { Id = new Guid("83e8734f-d800-403f-8e0d-33331f167940"), Code = "jobfactors", Name = "jobfactors", AppliesTo = "investigation", IsActive = true, CreatedAt = now, UpdatedAt = now };
        dbContext.InvestigationAttributeTypes.Add(refType_jobfactors);
        var refType_lifesupport = new InvestigationAttributeType { Id = new Guid("6bf33e94-2937-4b6c-98b5-ac8a4aaa15ac"), Code = "lifesupport", Name = "lifesupport", AppliesTo = "investigation", IsActive = true, CreatedAt = now, UpdatedAt = now };
        dbContext.InvestigationAttributeTypes.Add(refType_lifesupport);
        var refType_personnelfactors = new InvestigationAttributeType { Id = new Guid("f310d5de-d770-4111-9e0c-47f9643902d2"), Code = "personnelfactors", Name = "personnelfactors", AppliesTo = "investigation", IsActive = true, CreatedAt = now, UpdatedAt = now };
        dbContext.InvestigationAttributeTypes.Add(refType_personnelfactors);
        var refType_rootcause = new InvestigationAttributeType { Id = new Guid("a48e8f61-3c00-4689-b60a-d0d9aa3bf542"), Code = "rootcause", Name = "rootcause", AppliesTo = "investigation", IsActive = true, CreatedAt = now, UpdatedAt = now };
        dbContext.InvestigationAttributeTypes.Add(refType_rootcause);
        var refType_security = new InvestigationAttributeType { Id = new Guid("447cce39-54b3-4395-b1f1-83c3f95c57b3"), Code = "security", Name = "security", AppliesTo = "investigation", IsActive = true, CreatedAt = now, UpdatedAt = now };
        dbContext.InvestigationAttributeTypes.Add(refType_security);
        var refType_transportcompliance = new InvestigationAttributeType { Id = new Guid("d457b16e-f653-461e-b637-c9813d55d949"), Code = "transportcompliance", Name = "transportcompliance", AppliesTo = "investigation", IsActive = true, CreatedAt = now, UpdatedAt = now };
        dbContext.InvestigationAttributeTypes.Add(refType_transportcompliance);
        var refType_unsafeacts = new InvestigationAttributeType { Id = new Guid("33b83da8-76dd-4deb-9a74-10621a61f360"), Code = "unsafeacts", Name = "unsafeacts", AppliesTo = "investigation", IsActive = true, CreatedAt = now, UpdatedAt = now };
        dbContext.InvestigationAttributeTypes.Add(refType_unsafeacts);
        var refType_unsafeconditions = new InvestigationAttributeType { Id = new Guid("5b8243b4-1389-433a-b019-df8823953582"), Code = "unsafeconditions", Name = "unsafeconditions", AppliesTo = "investigation", IsActive = true, CreatedAt = now, UpdatedAt = now };
        dbContext.InvestigationAttributeTypes.Add(refType_unsafeconditions);

        // Document_Type — standalone, no reference type FK
        dbContext.DocTypeOptions.Add(new RefDocType { Id = new Guid("9a6b5543-7ec9-4061-8226-3059e961910a"), Code = "plan_permit", Name = "Plan Permit", IsActive = true, CreatedAt = now, UpdatedAt = now });
        dbContext.DocTypeOptions.Add(new RefDocType { Id = new Guid("2954c111-ec3a-48b5-8ed5-9374ab7e260f"), Code = "equipment_inspection", Name = "Equipment Inspection", IsActive = true, CreatedAt = now, UpdatedAt = now });
        dbContext.DocTypeOptions.Add(new RefDocType { Id = new Guid("1ebc5b6f-72b4-4d53-a5ed-6d9b48cdeebd"), Code = "citation", Name = "Citation", IsActive = true, CreatedAt = now, UpdatedAt = now });
        dbContext.DocTypeOptions.Add(new RefDocType { Id = new Guid("59a20afe-bdca-479a-b5e8-113ad284fa8e"), Code = "strongcardflha", Name = "Strongcard/FLHA", IsActive = true, CreatedAt = now, UpdatedAt = now });
        dbContext.DocTypeOptions.Add(new RefDocType { Id = new Guid("ecc86105-041d-4812-adec-b7a67c6affae"), Code = "policy_report", Name = "Policy Report", IsActive = true, CreatedAt = now, UpdatedAt = now });
        dbContext.DocTypeOptions.Add(new RefDocType { Id = new Guid("234addc7-b2f0-4b5c-9158-ce7d3908a23e"), Code = "statement_of_all_involved", Name = "Statement of all Involved", IsActive = true, CreatedAt = now, UpdatedAt = now });
        dbContext.DocTypeOptions.Add(new RefDocType { Id = new Guid("6daa79ed-3d39-456e-b7f1-a5385774cd3c"), Code = "daily_preshifttoolbox", Name = "Daily Pre-Shift/Toolbox", IsActive = true, CreatedAt = now, UpdatedAt = now });
        dbContext.DocTypeOptions.Add(new RefDocType { Id = new Guid("91278156-c6bf-43c3-90d0-674cb72c5fd6"), Code = "third_party_data", Name = "Third Party Data", IsActive = true, CreatedAt = now, UpdatedAt = now });
        dbContext.DocTypeOptions.Add(new RefDocType { Id = new Guid("41614eaa-b2dc-4e86-b931-e2b2b213d90e"), Code = "photograph", Name = "Photograph", IsActive = true, CreatedAt = now, UpdatedAt = now });
        dbContext.DocTypeOptions.Add(new RefDocType { Id = new Guid("53492743-c1f8-47bd-a1ed-2b5914eb5a24"), Code = "jha", Name = "JHA", IsActive = true, CreatedAt = now, UpdatedAt = now });
        dbContext.DocTypeOptions.Add(new RefDocType { Id = new Guid("d79efbe2-e1f4-4086-a890-9eba992519da"), Code = "training_records", Name = "Training Records", IsActive = true, CreatedAt = now, UpdatedAt = now });
        dbContext.DocTypeOptions.Add(new RefDocType { Id = new Guid("95fbf531-4719-4760-b722-60a8a2ac7832"), Code = "project_inspection", Name = "Project Inspection", IsActive = true, CreatedAt = now, UpdatedAt = now });
        dbContext.IncidentReportReferenceOptions.Add(new RefIncidentReportReference { Id = new Guid("e88801cf-c1d6-4364-9877-bf61a5210191"), ReferenceTypeId = new Guid("8069613e-50e5-4436-9bc5-788117d94780"), Code = "emission", Name = "Emission", IsActive = true, CreatedAt = now, UpdatedAt = now });
        dbContext.IncidentReportReferenceOptions.Add(new RefIncidentReportReference { Id = new Guid("71a87aac-2bf4-4fb6-9683-bf1497649627"), ReferenceTypeId = new Guid("8069613e-50e5-4436-9bc5-788117d94780"), Code = "release", Name = "Release", IsActive = true, CreatedAt = now, UpdatedAt = now });
        dbContext.IncidentReportReferenceOptions.Add(new RefIncidentReportReference { Id = new Guid("b7f04df4-c29c-482c-9c46-c81ea0976dbf"), ReferenceTypeId = new Guid("cf5cc886-fbc0-40f5-a771-ffe3f7b1e7df"), Code = "backing_up", Name = "Backing Up", IsActive = true, CreatedAt = now, UpdatedAt = now });
        dbContext.IncidentReportReferenceOptions.Add(new RefIncidentReportReference { Id = new Guid("1b0bcdae-9aea-4190-b7be-7af3613fc74e"), ReferenceTypeId = new Guid("cf5cc886-fbc0-40f5-a771-ffe3f7b1e7df"), Code = "fire", Name = "Fire", IsActive = true, CreatedAt = now, UpdatedAt = now });
        dbContext.IncidentReportReferenceOptions.Add(new RefIncidentReportReference { Id = new Guid("55b9f877-b4d6-4a4d-bc8e-acd5d9109b70"), ReferenceTypeId = new Guid("cf5cc886-fbc0-40f5-a771-ffe3f7b1e7df"), Code = "malfunction", Name = "Malfunction", IsActive = true, CreatedAt = now, UpdatedAt = now });
        dbContext.IncidentReportReferenceOptions.Add(new RefIncidentReportReference { Id = new Guid("af79c9c2-4f03-4ec2-b9fe-5e0a555db18d"), ReferenceTypeId = new Guid("cf5cc886-fbc0-40f5-a771-ffe3f7b1e7df"), Code = "unreported_damage", Name = "Unreported Damage", IsActive = true, CreatedAt = now, UpdatedAt = now });
        dbContext.IncidentReportReferenceOptions.Add(new RefIncidentReportReference { Id = new Guid("ab725423-b3fc-46f8-a9ba-4cc7bd5ea61e"), ReferenceTypeId = new Guid("cf5cc886-fbc0-40f5-a771-ffe3f7b1e7df"), Code = "windshield", Name = "Windshield", IsActive = true, CreatedAt = now, UpdatedAt = now });
        dbContext.IncidentReportReferenceOptions.Add(new RefIncidentReportReference { Id = new Guid("eccb5499-d750-4d24-a260-fb8afb382d9d"), ReferenceTypeId = new Guid("cf5cc886-fbc0-40f5-a771-ffe3f7b1e7df"), Code = "explosion", Name = "Explosion", IsActive = true, CreatedAt = now, UpdatedAt = now });
        dbContext.IncidentReportReferenceOptions.Add(new RefIncidentReportReference { Id = new Guid("3354b92f-054a-4f8e-a81f-07158765ca1e"), ReferenceTypeId = new Guid("cf5cc886-fbc0-40f5-a771-ffe3f7b1e7df"), Code = "struck_by", Name = "Struck By", IsActive = true, CreatedAt = now, UpdatedAt = now });
        dbContext.IncidentReportReferenceOptions.Add(new RefIncidentReportReference { Id = new Guid("0832fc3f-f7da-4092-b773-b2307bd97d4e"), ReferenceTypeId = new Guid("02513600-eb20-41e7-b2ff-e485d41c8e9c"), Code = "transportation_compliance", Name = "Transportation Compliance", IsActive = true, CreatedAt = now, UpdatedAt = now });
        dbContext.IncidentReportReferenceOptions.Add(new RefIncidentReportReference { Id = new Guid("ce2931ba-a56f-46fc-adc3-eb022b6d7bdf"), ReferenceTypeId = new Guid("02513600-eb20-41e7-b2ff-e485d41c8e9c"), Code = "injury", Name = "Injury", IsActive = true, CreatedAt = now, UpdatedAt = now });
        dbContext.IncidentReportReferenceOptions.Add(new RefIncidentReportReference { Id = new Guid("173ea622-4bdb-4495-bb48-41836de2574a"), ReferenceTypeId = new Guid("02513600-eb20-41e7-b2ff-e485d41c8e9c"), Code = "environmental", Name = "Environmental", IsActive = true, CreatedAt = now, UpdatedAt = now });
        dbContext.IncidentReportReferenceOptions.Add(new RefIncidentReportReference { Id = new Guid("5120d8a3-3f1d-497f-a0bc-b840dc32618e"), ReferenceTypeId = new Guid("02513600-eb20-41e7-b2ff-e485d41c8e9c"), Code = "policy_deviation", Name = "Policy Deviation", IsActive = true, CreatedAt = now, UpdatedAt = now });
        dbContext.IncidentReportReferenceOptions.Add(new RefIncidentReportReference { Id = new Guid("f6e27cff-8a25-4308-946b-d33e2d482d55"), ReferenceTypeId = new Guid("02513600-eb20-41e7-b2ff-e485d41c8e9c"), Code = "security", Name = "Security", IsActive = true, CreatedAt = now, UpdatedAt = now });
        dbContext.IncidentReportReferenceOptions.Add(new RefIncidentReportReference { Id = new Guid("341584af-5349-499b-835f-96ab199af78a"), ReferenceTypeId = new Guid("02513600-eb20-41e7-b2ff-e485d41c8e9c"), Code = "equipment_damage", Name = "Equipment Damage", IsActive = true, CreatedAt = now, UpdatedAt = now });
        dbContext.IncidentReportReferenceOptions.Add(new RefIncidentReportReference { Id = new Guid("451944b5-d133-470f-a2d0-02e21995d4f0"), ReferenceTypeId = new Guid("02513600-eb20-41e7-b2ff-e485d41c8e9c"), Code = "motor_vehicle_accident", Name = "Motor Vehicle Accident", IsActive = true, CreatedAt = now, UpdatedAt = now });
        dbContext.IncidentReportReferenceOptions.Add(new RefIncidentReportReference { Id = new Guid("14bdad8f-b742-49a2-a24b-492e676d6c39"), ReferenceTypeId = new Guid("02513600-eb20-41e7-b2ff-e485d41c8e9c"), Code = "life_support", Name = "Life Support", IsActive = true, CreatedAt = now, UpdatedAt = now });
        dbContext.IncidentReportReferenceOptions.Add(new RefIncidentReportReference { Id = new Guid("09c3a52d-286e-42ac-b480-f243c91e1b3e"), ReferenceTypeId = new Guid("3f35c242-5a38-4614-88bb-f843e1334f9b"), Code = "modified_work", Name = "Modified Work", IsActive = true, CreatedAt = now, UpdatedAt = now });
        dbContext.IncidentReportReferenceOptions.Add(new RefIncidentReportReference { Id = new Guid("68faa42e-22bf-46e7-bf40-94c6345217fc"), ReferenceTypeId = new Guid("3f35c242-5a38-4614-88bb-f843e1334f9b"), Code = "alleged", Name = "Alleged", IsActive = true, CreatedAt = now, UpdatedAt = now });
        dbContext.IncidentReportReferenceOptions.Add(new RefIncidentReportReference { Id = new Guid("d78b6777-9c71-4319-b83f-b4ece0c32410"), ReferenceTypeId = new Guid("3f35c242-5a38-4614-88bb-f843e1334f9b"), Code = "lost_time", Name = "Lost Time", IsActive = true, CreatedAt = now, UpdatedAt = now });
        dbContext.IncidentReportReferenceOptions.Add(new RefIncidentReportReference { Id = new Guid("df42a15c-5a14-49ff-ad21-3271f3935a36"), ReferenceTypeId = new Guid("3f35c242-5a38-4614-88bb-f843e1334f9b"), Code = "medical_aid", Name = "Medical Aid", IsActive = true, CreatedAt = now, UpdatedAt = now });
        dbContext.IncidentReportReferenceOptions.Add(new RefIncidentReportReference { Id = new Guid("6ed4c383-4f2d-46e8-854e-5a101cf488c4"), ReferenceTypeId = new Guid("3f35c242-5a38-4614-88bb-f843e1334f9b"), Code = "report_only", Name = "Report Only", IsActive = true, CreatedAt = now, UpdatedAt = now });
        dbContext.IncidentReportReferenceOptions.Add(new RefIncidentReportReference { Id = new Guid("4aad711e-eadb-41a8-92a7-39d12aae3782"), ReferenceTypeId = new Guid("3f35c242-5a38-4614-88bb-f843e1334f9b"), Code = "firstaid", Name = "First-Aid", IsActive = true, CreatedAt = now, UpdatedAt = now });
        dbContext.IncidentReportReferenceOptions.Add(new RefIncidentReportReference { Id = new Guid("1f0a1ffa-9cac-4f9f-9430-0d1ef10c1d2d"), ReferenceTypeId = new Guid("eac8e144-c4c9-4387-be53-37e0b557b232"), Code = "fall_lower_level", Name = "Fall Lower Level", IsActive = true, CreatedAt = now, UpdatedAt = now });
        dbContext.IncidentReportReferenceOptions.Add(new RefIncidentReportReference { Id = new Guid("21530f92-3481-41b5-8d85-7da22321895b"), ReferenceTypeId = new Guid("eac8e144-c4c9-4387-be53-37e0b557b232"), Code = "caught_betweenunder", Name = "Caught Between/Under", IsActive = true, CreatedAt = now, UpdatedAt = now });
        dbContext.IncidentReportReferenceOptions.Add(new RefIncidentReportReference { Id = new Guid("86c9960b-e857-4993-881d-b7da40975016"), ReferenceTypeId = new Guid("eac8e144-c4c9-4387-be53-37e0b557b232"), Code = "contact_with", Name = "Contact With", IsActive = true, CreatedAt = now, UpdatedAt = now });
        dbContext.IncidentReportReferenceOptions.Add(new RefIncidentReportReference { Id = new Guid("8e564f79-da80-48ab-9d1b-a67d40145076"), ReferenceTypeId = new Guid("eac8e144-c4c9-4387-be53-37e0b557b232"), Code = "caught_in", Name = "Caught In", IsActive = true, CreatedAt = now, UpdatedAt = now });
        dbContext.IncidentReportReferenceOptions.Add(new RefIncidentReportReference { Id = new Guid("0c5416e8-556a-4512-8f39-e0815195d6a6"), ReferenceTypeId = new Guid("eac8e144-c4c9-4387-be53-37e0b557b232"), Code = "caught_on", Name = "Caught On", IsActive = true, CreatedAt = now, UpdatedAt = now });
        dbContext.IncidentReportReferenceOptions.Add(new RefIncidentReportReference { Id = new Guid("22bd26ac-8e85-4235-9051-a87a3e54a61f"), ReferenceTypeId = new Guid("eac8e144-c4c9-4387-be53-37e0b557b232"), Code = "exposure", Name = "Exposure", IsActive = true, CreatedAt = now, UpdatedAt = now });
        dbContext.IncidentReportReferenceOptions.Add(new RefIncidentReportReference { Id = new Guid("9889f4bc-64d5-4be5-9d67-ffd796b80c3e"), ReferenceTypeId = new Guid("eac8e144-c4c9-4387-be53-37e0b557b232"), Code = "overstress_ergonomics", Name = "Overstress Ergonomics", IsActive = true, CreatedAt = now, UpdatedAt = now });
        dbContext.IncidentReportReferenceOptions.Add(new RefIncidentReportReference { Id = new Guid("60ace0bd-392e-4197-ba11-695d7bf2ea0c"), ReferenceTypeId = new Guid("eac8e144-c4c9-4387-be53-37e0b557b232"), Code = "fall_same_level", Name = "Fall Same Level", IsActive = true, CreatedAt = now, UpdatedAt = now });
        dbContext.IncidentReportReferenceOptions.Add(new RefIncidentReportReference { Id = new Guid("21f7b549-ef08-4586-97e0-4a6a5409ab5f"), ReferenceTypeId = new Guid("eac8e144-c4c9-4387-be53-37e0b557b232"), Code = "struck_by", Name = "Struck By", IsActive = true, CreatedAt = now, UpdatedAt = now });
        dbContext.IncidentReportReferenceOptions.Add(new RefIncidentReportReference { Id = new Guid("81443c22-b1ab-4de4-a945-be16a513b565"), ReferenceTypeId = new Guid("eac8e144-c4c9-4387-be53-37e0b557b232"), Code = "struck_against", Name = "Struck Against", IsActive = true, CreatedAt = now, UpdatedAt = now });
        // ref_investigation_reference — jobfactors
        dbContext.InvestigationReferenceOptions.Add(new RefInvestigationReference { Id = new Guid("9035ed55-2299-4920-9185-e780528f77c3"), ReferenceTypeId = new Guid("83e8734f-d800-403f-8e0d-33331f167940"), Code = "bulletin_reviews", Name = "Bulletin Reviews", IsActive = true, CreatedAt = now, UpdatedAt = now });
        dbContext.InvestigationReferenceOptions.Add(new RefInvestigationReference { Id = new Guid("c481e299-6585-46b7-aaed-3328db33193e"), ReferenceTypeId = new Guid("83e8734f-d800-403f-8e0d-33331f167940"), Code = "equipment_inspections", Name = "Equipment Inspections", IsActive = true, CreatedAt = now, UpdatedAt = now });
        dbContext.InvestigationReferenceOptions.Add(new RefInvestigationReference { Id = new Guid("b2634fd5-e368-4dd7-9e51-1536e02b7d4e"), ReferenceTypeId = new Guid("83e8734f-d800-403f-8e0d-33331f167940"), Code = "dvirpretrip", Name = "DVIR/Pre-Trip", IsActive = true, CreatedAt = now, UpdatedAt = now });
        dbContext.InvestigationReferenceOptions.Add(new RefInvestigationReference { Id = new Guid("5cf84437-edd9-4b1e-8221-819cb84821bf"), ReferenceTypeId = new Guid("83e8734f-d800-403f-8e0d-33331f167940"), Code = "strong_card", Name = "Strong Card", IsActive = true, CreatedAt = now, UpdatedAt = now });
        dbContext.InvestigationReferenceOptions.Add(new RefInvestigationReference { Id = new Guid("61a1065f-37cb-4c00-86ec-10b67dcb50d6"), ReferenceTypeId = new Guid("83e8734f-d800-403f-8e0d-33331f167940"), Code = "policies", Name = "Policies", IsActive = true, CreatedAt = now, UpdatedAt = now });
        dbContext.InvestigationReferenceOptions.Add(new RefInvestigationReference { Id = new Guid("2d6ae108-e4ba-438f-862a-e5f09493ea9c"), ReferenceTypeId = new Guid("83e8734f-d800-403f-8e0d-33331f167940"), Code = "site_inspection", Name = "Site Inspection", IsActive = true, CreatedAt = now, UpdatedAt = now });
        dbContext.InvestigationReferenceOptions.Add(new RefInvestigationReference { Id = new Guid("52cc4b83-5d45-40da-ac6b-48a96f24cf33"), ReferenceTypeId = new Guid("83e8734f-d800-403f-8e0d-33331f167940"), Code = "master_jha", Name = "Master JHA", IsActive = true, CreatedAt = now, UpdatedAt = now });
        dbContext.InvestigationReferenceOptions.Add(new RefInvestigationReference { Id = new Guid("3494567e-6a34-4e87-bb14-0f45b3cc1452"), ReferenceTypeId = new Guid("83e8734f-d800-403f-8e0d-33331f167940"), Code = "communicationreinforcement", Name = "Communication/Reinforcement", IsActive = true, CreatedAt = now, UpdatedAt = now });
        dbContext.InvestigationReferenceOptions.Add(new RefInvestigationReference { Id = new Guid("59b10038-03aa-4134-b329-6d87cd29e4e8"), ReferenceTypeId = new Guid("83e8734f-d800-403f-8e0d-33331f167940"), Code = "standardcop", Name = "Standard/COP", IsActive = true, CreatedAt = now, UpdatedAt = now });
        dbContext.InvestigationReferenceOptions.Add(new RefInvestigationReference { Id = new Guid("002c58de-4ebf-48e0-a073-19c97ade1972"), ReferenceTypeId = new Guid("83e8734f-d800-403f-8e0d-33331f167940"), Code = "sop", Name = "SOP", IsActive = true, CreatedAt = now, UpdatedAt = now });
        dbContext.InvestigationReferenceOptions.Add(new RefInvestigationReference { Id = new Guid("ebf73681-fbff-4fc2-a0ea-f247c221cf48"), ReferenceTypeId = new Guid("83e8734f-d800-403f-8e0d-33331f167940"), Code = "employee_engagements", Name = "Employee Engagements", IsActive = true, CreatedAt = now, UpdatedAt = now });
        // ref_investigation_reference — lifesupport
        dbContext.InvestigationReferenceOptions.Add(new RefInvestigationReference { Id = new Guid("93d88681-d9eb-408f-b451-a762aa0192e8"), ReferenceTypeId = new Guid("6bf33e94-2937-4b6c-98b5-ac8a4aaa15ac"), Code = "pigtail", Name = "Pigtail", IsActive = true, CreatedAt = now, UpdatedAt = now });
        dbContext.InvestigationReferenceOptions.Add(new RefInvestigationReference { Id = new Guid("e2d107c4-4852-4d88-9bf7-559be3119691"), ReferenceTypeId = new Guid("6bf33e94-2937-4b6c-98b5-ac8a4aaa15ac"), Code = "helmet_malfunction", Name = "Helmet Malfunction", IsActive = true, CreatedAt = now, UpdatedAt = now });
        dbContext.InvestigationReferenceOptions.Add(new RefInvestigationReference { Id = new Guid("4fcdd151-3a14-4644-857b-47430c66fde6"), ReferenceTypeId = new Guid("6bf33e94-2937-4b6c-98b5-ac8a4aaa15ac"), Code = "harness", Name = "Harness", IsActive = true, CreatedAt = now, UpdatedAt = now });
        dbContext.InvestigationReferenceOptions.Add(new RefInvestigationReference { Id = new Guid("83b8d91f-e0b8-4618-9277-6f3685bd9b62"), ReferenceTypeId = new Guid("6bf33e94-2937-4b6c-98b5-ac8a4aaa15ac"), Code = "trailercube", Name = "Trailer/Cube", IsActive = true, CreatedAt = now, UpdatedAt = now });
        dbContext.InvestigationReferenceOptions.Add(new RefInvestigationReference { Id = new Guid("a9ad56f2-1c3e-4433-a972-49a1b3d9b56e"), ReferenceTypeId = new Guid("6bf33e94-2937-4b6c-98b5-ac8a4aaa15ac"), Code = "gas_detection", Name = "Gas Detection", IsActive = true, CreatedAt = now, UpdatedAt = now });
        dbContext.InvestigationReferenceOptions.Add(new RefInvestigationReference { Id = new Guid("bc8c08e8-60b3-4e34-83de-0f1eb98f2f61"), ReferenceTypeId = new Guid("6bf33e94-2937-4b6c-98b5-ac8a4aaa15ac"), Code = "helmet_damage", Name = "Helmet Damage", IsActive = true, CreatedAt = now, UpdatedAt = now });
        dbContext.InvestigationReferenceOptions.Add(new RefInvestigationReference { Id = new Guid("aca2b7a2-d2aa-485b-b73e-3f57234dc90f"), ReferenceTypeId = new Guid("6bf33e94-2937-4b6c-98b5-ac8a4aaa15ac"), Code = "egress_cylinder", Name = "Egress Cylinder", IsActive = true, CreatedAt = now, UpdatedAt = now });
        dbContext.InvestigationReferenceOptions.Add(new RefInvestigationReference { Id = new Guid("1e1f128a-33da-4013-9c52-36e7d8cc6fad"), ReferenceTypeId = new Guid("6bf33e94-2937-4b6c-98b5-ac8a4aaa15ac"), Code = "module", Name = "Module", IsActive = true, CreatedAt = now, UpdatedAt = now });
        dbContext.InvestigationReferenceOptions.Add(new RefInvestigationReference { Id = new Guid("043d0fde-3411-466e-9e61-794f52da015f"), ReferenceTypeId = new Guid("6bf33e94-2937-4b6c-98b5-ac8a4aaa15ac"), Code = "egress_regulator", Name = "Egress Regulator", IsActive = true, CreatedAt = now, UpdatedAt = now });
        dbContext.InvestigationReferenceOptions.Add(new RefInvestigationReference { Id = new Guid("2dd13699-0fc9-4673-b584-36dd4561da80"), ReferenceTypeId = new Guid("6bf33e94-2937-4b6c-98b5-ac8a4aaa15ac"), Code = "umbilical", Name = "Umbilical", IsActive = true, CreatedAt = now, UpdatedAt = now });
        // ref_incident_report_reference — motorvehicleaccident (stays here)
        dbContext.IncidentReportReferenceOptions.Add(new RefIncidentReportReference { Id = new Guid("c1f41d3b-7c7a-435b-87b0-ec41517b1587"), ReferenceTypeId = new Guid("4ce0c493-50b3-487f-b8a6-d5a211c2da21"), Code = "third_party", Name = "Third Party", IsActive = true, CreatedAt = now, UpdatedAt = now });
        dbContext.IncidentReportReferenceOptions.Add(new RefIncidentReportReference { Id = new Guid("c3f2a273-548f-4e4c-b5c9-698ed98e9cbf"), ReferenceTypeId = new Guid("4ce0c493-50b3-487f-b8a6-d5a211c2da21"), Code = "single_vehicle", Name = "Single Vehicle", IsActive = true, CreatedAt = now, UpdatedAt = now });
        dbContext.IncidentReportReferenceOptions.Add(new RefIncidentReportReference { Id = new Guid("cedc2702-c2fa-4ea4-82e1-2ba94935e431"), ReferenceTypeId = new Guid("4ce0c493-50b3-487f-b8a6-d5a211c2da21"), Code = "animal_strike", Name = "Animal Strike", IsActive = true, CreatedAt = now, UpdatedAt = now });
        // ref_investigation_reference — personnelfactors
        dbContext.InvestigationReferenceOptions.Add(new RefInvestigationReference { Id = new Guid("1350b049-f7c5-446d-bd81-1d4a9aa8ac18"), ReferenceTypeId = new Guid("f310d5de-d770-4111-9e0c-47f9643902d2"), Code = "poor_brothers_sisters_keeper", Name = "Poor Brothers' & Sisters' Keeper", IsActive = true, CreatedAt = now, UpdatedAt = now });
        dbContext.InvestigationReferenceOptions.Add(new RefInvestigationReference { Id = new Guid("1ae51939-d9af-44dc-b57c-a8c7ecc4210e"), ReferenceTypeId = new Guid("f310d5de-d770-4111-9e0c-47f9643902d2"), Code = "inadequate_policies", Name = "Inadequate Policies", IsActive = true, CreatedAt = now, UpdatedAt = now });
        dbContext.InvestigationReferenceOptions.Add(new RefInvestigationReference { Id = new Guid("f23b428c-55ee-4887-8ef2-500a9ae50ccd"), ReferenceTypeId = new Guid("f310d5de-d770-4111-9e0c-47f9643902d2"), Code = "attitude", Name = "Attitude", IsActive = true, CreatedAt = now, UpdatedAt = now });
        dbContext.InvestigationReferenceOptions.Add(new RefInvestigationReference { Id = new Guid("5bda5e02-8bb3-4d10-9257-57e5cb3384b5"), ReferenceTypeId = new Guid("f310d5de-d770-4111-9e0c-47f9643902d2"), Code = "missing_training", Name = "Missing Training", IsActive = true, CreatedAt = now, UpdatedAt = now });
        dbContext.InvestigationReferenceOptions.Add(new RefInvestigationReference { Id = new Guid("2cd65880-3272-40d9-af93-c43791375948"), ReferenceTypeId = new Guid("f310d5de-d770-4111-9e0c-47f9643902d2"), Code = "missing_policy", Name = "Missing Policy", IsActive = true, CreatedAt = now, UpdatedAt = now });
        dbContext.InvestigationReferenceOptions.Add(new RefInvestigationReference { Id = new Guid("df8b360e-194c-4dc0-9a00-fc5a04f20101"), ReferenceTypeId = new Guid("f310d5de-d770-4111-9e0c-47f9643902d2"), Code = "inadequate_culture_building", Name = "Inadequate Culture Building", IsActive = true, CreatedAt = now, UpdatedAt = now });
        dbContext.InvestigationReferenceOptions.Add(new RefInvestigationReference { Id = new Guid("52bd5883-6213-4701-9399-44220383f5dc"), ReferenceTypeId = new Guid("f310d5de-d770-4111-9e0c-47f9643902d2"), Code = "inadequate_training", Name = "Inadequate Training", IsActive = true, CreatedAt = now, UpdatedAt = now });
        dbContext.InvestigationReferenceOptions.Add(new RefInvestigationReference { Id = new Guid("1960e7b8-d9d8-4001-a873-378e64b69d25"), ReferenceTypeId = new Guid("f310d5de-d770-4111-9e0c-47f9643902d2"), Code = "inadequate_job_planning", Name = "Inadequate Job Planning", IsActive = true, CreatedAt = now, UpdatedAt = now });
        dbContext.InvestigationReferenceOptions.Add(new RefInvestigationReference { Id = new Guid("2eec739f-ed26-45d1-9614-1e99e58af11f"), ReferenceTypeId = new Guid("f310d5de-d770-4111-9e0c-47f9643902d2"), Code = "rogue_employeeroot", Name = "Rogue EmployeeRoot", IsActive = true, CreatedAt = now, UpdatedAt = now });
        dbContext.InvestigationReferenceOptions.Add(new RefInvestigationReference { Id = new Guid("25415c22-82df-4c9f-8b5f-ebedfdf8b7d6"), ReferenceTypeId = new Guid("f310d5de-d770-4111-9e0c-47f9643902d2"), Code = "poor_communication", Name = "Poor Communication", IsActive = true, CreatedAt = now, UpdatedAt = now });
        dbContext.InvestigationReferenceOptions.Add(new RefInvestigationReference { Id = new Guid("337785bf-21e9-41df-9d02-bbcd49827a4e"), ReferenceTypeId = new Guid("f310d5de-d770-4111-9e0c-47f9643902d2"), Code = "inadequate_motivation", Name = "Inadequate Motivation", IsActive = true, CreatedAt = now, UpdatedAt = now });
        // ref_incident_report_reference — policydeviation (stays here)
        dbContext.IncidentReportReferenceOptions.Add(new RefIncidentReportReference { Id = new Guid("24b28fdf-a611-4b5e-8109-3e20038f8f10"), ReferenceTypeId = new Guid("d39c182a-1f3b-4a5f-924f-c366e2d245ae"), Code = "shc_policy", Name = "SHC Policy", IsActive = true, CreatedAt = now, UpdatedAt = now });
        dbContext.IncidentReportReferenceOptions.Add(new RefIncidentReportReference { Id = new Guid("0f696618-c07f-4536-b27d-9c17763f0993"), ReferenceTypeId = new Guid("d39c182a-1f3b-4a5f-924f-c366e2d245ae"), Code = "client_policy", Name = "Client Policy", IsActive = true, CreatedAt = now, UpdatedAt = now });
        dbContext.IncidentReportReferenceOptions.Add(new RefIncidentReportReference { Id = new Guid("47c89df4-d63c-4aef-9e5b-5babff8ecb83"), ReferenceTypeId = new Guid("d39c182a-1f3b-4a5f-924f-c366e2d245ae"), Code = "lifesaving_rule_violation", Name = "Life-Saving Rule Violation", IsActive = true, CreatedAt = now, UpdatedAt = now });
        // ref_investigation_reference — rootcause
        dbContext.InvestigationReferenceOptions.Add(new RefInvestigationReference { Id = new Guid("4d9edb85-2e54-4ab2-bcfe-34daea8054c7"), ReferenceTypeId = new Guid("a48e8f61-3c00-4689-b60a-d0d9aa3bf542"), Code = "inadequate_job_planning", Name = "Inadequate Job Planning", IsActive = true, CreatedAt = now, UpdatedAt = now });
        dbContext.InvestigationReferenceOptions.Add(new RefInvestigationReference { Id = new Guid("bd79592c-a44f-4120-9f9e-d99437d66c3e"), ReferenceTypeId = new Guid("a48e8f61-3c00-4689-b60a-d0d9aa3bf542"), Code = "poor_brothers_sisters_keeper_attitude", Name = "Poor Brothers' & Sisters' Keeper Attitude", IsActive = true, CreatedAt = now, UpdatedAt = now });
        dbContext.InvestigationReferenceOptions.Add(new RefInvestigationReference { Id = new Guid("7d97c0c9-b472-4a07-9bfa-341827e6ecff"), ReferenceTypeId = new Guid("a48e8f61-3c00-4689-b60a-d0d9aa3bf542"), Code = "inadequate_culture_building", Name = "Inadequate Culture Building", IsActive = true, CreatedAt = now, UpdatedAt = now });
        dbContext.InvestigationReferenceOptions.Add(new RefInvestigationReference { Id = new Guid("7096481f-5260-4174-9b8d-caeca5aea9b5"), ReferenceTypeId = new Guid("a48e8f61-3c00-4689-b60a-d0d9aa3bf542"), Code = "rogue_employee", Name = "Rogue Employee", IsActive = true, CreatedAt = now, UpdatedAt = now });
        dbContext.InvestigationReferenceOptions.Add(new RefInvestigationReference { Id = new Guid("c44f5e9a-d7aa-4851-8e8a-5f9edcb507df"), ReferenceTypeId = new Guid("a48e8f61-3c00-4689-b60a-d0d9aa3bf542"), Code = "poor_communication", Name = "Poor Communication", IsActive = true, CreatedAt = now, UpdatedAt = now });
        dbContext.InvestigationReferenceOptions.Add(new RefInvestigationReference { Id = new Guid("67040270-00b1-4172-9c30-1add7c0f4bd2"), ReferenceTypeId = new Guid("a48e8f61-3c00-4689-b60a-d0d9aa3bf542"), Code = "inadequate_motivation", Name = "Inadequate Motivation", IsActive = true, CreatedAt = now, UpdatedAt = now });
        dbContext.InvestigationReferenceOptions.Add(new RefInvestigationReference { Id = new Guid("d59b776c-26d2-4ec9-b6cc-90697d2bd71f"), ReferenceTypeId = new Guid("a48e8f61-3c00-4689-b60a-d0d9aa3bf542"), Code = "missing_training", Name = "Missing Training", IsActive = true, CreatedAt = now, UpdatedAt = now });
        dbContext.InvestigationReferenceOptions.Add(new RefInvestigationReference { Id = new Guid("cc8c392c-9bd5-402e-a715-bca9ce0066ff"), ReferenceTypeId = new Guid("a48e8f61-3c00-4689-b60a-d0d9aa3bf542"), Code = "inadequate_training", Name = "Inadequate Training", IsActive = true, CreatedAt = now, UpdatedAt = now });
        dbContext.InvestigationReferenceOptions.Add(new RefInvestigationReference { Id = new Guid("2d74f671-0f29-45b1-b1de-d77cde4518b1"), ReferenceTypeId = new Guid("a48e8f61-3c00-4689-b60a-d0d9aa3bf542"), Code = "inadequate_policies", Name = "Inadequate Policies", IsActive = true, CreatedAt = now, UpdatedAt = now });
        // ref_investigation_reference — security
        dbContext.InvestigationReferenceOptions.Add(new RefInvestigationReference { Id = new Guid("d990fde6-bd78-4742-8e7f-d8d4a6bc586f"), ReferenceTypeId = new Guid("447cce39-54b3-4395-b1f1-83c3f95c57b3"), Code = "break_in", Name = "Break In", IsActive = true, CreatedAt = now, UpdatedAt = now });
        dbContext.InvestigationReferenceOptions.Add(new RefInvestigationReference { Id = new Guid("67e1a826-a14f-48de-8684-411dafb22456"), ReferenceTypeId = new Guid("447cce39-54b3-4395-b1f1-83c3f95c57b3"), Code = "theft", Name = "Theft", IsActive = true, CreatedAt = now, UpdatedAt = now });
        dbContext.InvestigationReferenceOptions.Add(new RefInvestigationReference { Id = new Guid("d1761c7f-ffcd-4a1d-83b7-71c477304087"), ReferenceTypeId = new Guid("447cce39-54b3-4395-b1f1-83c3f95c57b3"), Code = "vandelism", Name = "Vandelism", IsActive = true, CreatedAt = now, UpdatedAt = now });
        dbContext.InvestigationReferenceOptions.Add(new RefInvestigationReference { Id = new Guid("29eeb928-3fdb-4363-bf0a-e2a7432e4364"), ReferenceTypeId = new Guid("447cce39-54b3-4395-b1f1-83c3f95c57b3"), Code = "arson", Name = "Arson", IsActive = true, CreatedAt = now, UpdatedAt = now });
        // ref_investigation_reference — transportcompliance
        dbContext.InvestigationReferenceOptions.Add(new RefInvestigationReference { Id = new Guid("ba86312e-be90-4222-9e82-c051cdcb1c4f"), ReferenceTypeId = new Guid("d457b16e-f653-461e-b637-c9813d55d949"), Code = "citations", Name = "Citations", IsActive = true, CreatedAt = now, UpdatedAt = now });
        dbContext.InvestigationReferenceOptions.Add(new RefInvestigationReference { Id = new Guid("62f67505-78e4-40ad-aea4-909e552c43fe"), ReferenceTypeId = new Guid("d457b16e-f653-461e-b637-c9813d55d949"), Code = "public_driving_complaint", Name = "Public Driving Complaint", IsActive = true, CreatedAt = now, UpdatedAt = now });
        dbContext.InvestigationReferenceOptions.Add(new RefInvestigationReference { Id = new Guid("0ec30c15-1ddb-4433-97da-a524cd5b23a5"), ReferenceTypeId = new Guid("d457b16e-f653-461e-b637-c9813d55d949"), Code = "roadside_inspection", Name = "Roadside Inspection", IsActive = true, CreatedAt = now, UpdatedAt = now });
        // ref_investigation_reference — unsafeacts
        dbContext.InvestigationReferenceOptions.Add(new RefInvestigationReference { Id = new Guid("9c3e02bf-3f74-4de0-8a3d-d0f8bd9e4102"), ReferenceTypeId = new Guid("33b83da8-76dd-4deb-9a74-10621a61f360"), Code = "bypassing_safety_controls", Name = "Bypassing Safety Controls", IsActive = true, CreatedAt = now, UpdatedAt = now });
        dbContext.InvestigationReferenceOptions.Add(new RefInvestigationReference { Id = new Guid("11f50532-2ff7-4338-8502-e471ae998724"), ReferenceTypeId = new Guid("33b83da8-76dd-4deb-9a74-10621a61f360"), Code = "horseplay", Name = "Horseplay", IsActive = true, CreatedAt = now, UpdatedAt = now });
        dbContext.InvestigationReferenceOptions.Add(new RefInvestigationReference { Id = new Guid("e5f58c68-831e-49b8-84eb-92a0b78cfb06"), ReferenceTypeId = new Guid("33b83da8-76dd-4deb-9a74-10621a61f360"), Code = "cse_violation", Name = "CSE Violation", IsActive = true, CreatedAt = now, UpdatedAt = now });
        dbContext.InvestigationReferenceOptions.Add(new RefInvestigationReference { Id = new Guid("2e3b3b26-3d41-41dc-9a4b-e46464e142d3"), ReferenceTypeId = new Guid("33b83da8-76dd-4deb-9a74-10621a61f360"), Code = "energy_isolation", Name = "Energy Isolation", IsActive = true, CreatedAt = now, UpdatedAt = now });
        dbContext.InvestigationReferenceOptions.Add(new RefInvestigationReference { Id = new Guid("cedc34fa-f2a4-47b0-b6fe-4227022c1bc7"), ReferenceTypeId = new Guid("33b83da8-76dd-4deb-9a74-10621a61f360"), Code = "failure_to_secure_equipment", Name = "Failure to Secure Equipment", IsActive = true, CreatedAt = now, UpdatedAt = now });
        dbContext.InvestigationReferenceOptions.Add(new RefInvestigationReference { Id = new Guid("1c3e55f5-dcb7-4765-b57c-baa4cfa112da"), ReferenceTypeId = new Guid("33b83da8-76dd-4deb-9a74-10621a61f360"), Code = "not_driving_to_road_conditions", Name = "Not Driving to Road Conditions", IsActive = true, CreatedAt = now, UpdatedAt = now });
        dbContext.InvestigationReferenceOptions.Add(new RefInvestigationReference { Id = new Guid("e4ceb179-861e-431c-b8fe-5cab8fa1e2e6"), ReferenceTypeId = new Guid("33b83da8-76dd-4deb-9a74-10621a61f360"), Code = "overconfident", Name = "Overconfident", IsActive = true, CreatedAt = now, UpdatedAt = now });
        dbContext.InvestigationReferenceOptions.Add(new RefInvestigationReference { Id = new Guid("05166e52-2a35-4ab1-94aa-f8118ff54f52"), ReferenceTypeId = new Guid("33b83da8-76dd-4deb-9a74-10621a61f360"), Code = "failure_to_inspect", Name = "Failure to Inspect", IsActive = true, CreatedAt = now, UpdatedAt = now });
        dbContext.InvestigationReferenceOptions.Add(new RefInvestigationReference { Id = new Guid("017e1f33-b0ef-4b50-8af6-a130f9708673"), ReferenceTypeId = new Guid("33b83da8-76dd-4deb-9a74-10621a61f360"), Code = "failure_to_identify_or_control_a_hazard", Name = "Failure to Identify or Control a Hazard", IsActive = true, CreatedAt = now, UpdatedAt = now });
        dbContext.InvestigationReferenceOptions.Add(new RefInvestigationReference { Id = new Guid("a3497535-360f-437a-88d2-a0c7d8c7c389"), ReferenceTypeId = new Guid("33b83da8-76dd-4deb-9a74-10621a61f360"), Code = "fall_protection_violation", Name = "Fall Protection Violation", IsActive = true, CreatedAt = now, UpdatedAt = now });
        dbContext.InvestigationReferenceOptions.Add(new RefInvestigationReference { Id = new Guid("c943bee7-8a8a-431d-938b-24570e08e7a6"), ReferenceTypeId = new Guid("33b83da8-76dd-4deb-9a74-10621a61f360"), Code = "improper_use_of_toolsequipment", Name = "Improper use of tools/equipment", IsActive = true, CreatedAt = now, UpdatedAt = now });
        dbContext.InvestigationReferenceOptions.Add(new RefInvestigationReference { Id = new Guid("a3dad72d-e6fd-40a8-861d-9ba86241be31"), ReferenceTypeId = new Guid("33b83da8-76dd-4deb-9a74-10621a61f360"), Code = "inattentive_to_hazards", Name = "Inattentive to Hazards", IsActive = true, CreatedAt = now, UpdatedAt = now });
        dbContext.InvestigationReferenceOptions.Add(new RefInvestigationReference { Id = new Guid("f5af9145-f444-440c-8e5d-9aff6fb74b36"), ReferenceTypeId = new Guid("33b83da8-76dd-4deb-9a74-10621a61f360"), Code = "line_of_fire_positioning", Name = "Line of Fire Positioning", IsActive = true, CreatedAt = now, UpdatedAt = now });
        dbContext.InvestigationReferenceOptions.Add(new RefInvestigationReference { Id = new Guid("3c19fd33-528e-422d-8c66-a7b5c1da6585"), ReferenceTypeId = new Guid("33b83da8-76dd-4deb-9a74-10621a61f360"), Code = "not_fit_for_duty", Name = "Not Fit for Duty", IsActive = true, CreatedAt = now, UpdatedAt = now });
        dbContext.InvestigationReferenceOptions.Add(new RefInvestigationReference { Id = new Guid("c738adfa-af3a-4df5-82fe-88b48e3e9167"), ReferenceTypeId = new Guid("33b83da8-76dd-4deb-9a74-10621a61f360"), Code = "trying_to_gain_or_save_time", Name = "Trying to Gain or Save Time", IsActive = true, CreatedAt = now, UpdatedAt = now });
        dbContext.InvestigationReferenceOptions.Add(new RefInvestigationReference { Id = new Guid("c82024e5-7b87-490d-94a2-4017984c9a9b"), ReferenceTypeId = new Guid("33b83da8-76dd-4deb-9a74-10621a61f360"), Code = "using_uninspected_equipment", Name = "Using Uninspected Equipment", IsActive = true, CreatedAt = now, UpdatedAt = now });
        dbContext.InvestigationReferenceOptions.Add(new RefInvestigationReference { Id = new Guid("60972263-b278-49cd-9ffe-963ad674edcd"), ReferenceTypeId = new Guid("33b83da8-76dd-4deb-9a74-10621a61f360"), Code = "workplace_violencesystemic", Name = "Workplace ViolenceSystemic", IsActive = true, CreatedAt = now, UpdatedAt = now });
        dbContext.InvestigationReferenceOptions.Add(new RefInvestigationReference { Id = new Guid("c00afdd1-43e3-4ead-a8c0-df39eab820bb"), ReferenceTypeId = new Guid("33b83da8-76dd-4deb-9a74-10621a61f360"), Code = "working_without_authority_permit_incorrect_permit", Name = "Working without Authority / Permit /Incorrect Permit", IsActive = true, CreatedAt = now, UpdatedAt = now });
        // ref_investigation_reference — unsafeconditions
        dbContext.InvestigationReferenceOptions.Add(new RefInvestigationReference { Id = new Guid("6ffd6fdc-8b43-45a0-9456-3de79744c717"), ReferenceTypeId = new Guid("5b8243b4-1389-433a-b019-df8823953582"), Code = "inadequate_procedure", Name = "Inadequate Procedure", IsActive = true, CreatedAt = now, UpdatedAt = now });
        dbContext.InvestigationReferenceOptions.Add(new RefInvestigationReference { Id = new Guid("07a6d38c-11c5-44e1-914a-e05d427fa948"), ReferenceTypeId = new Guid("5b8243b4-1389-433a-b019-df8823953582"), Code = "inadequate_training", Name = "Inadequate Training", IsActive = true, CreatedAt = now, UpdatedAt = now });
        dbContext.InvestigationReferenceOptions.Add(new RefInvestigationReference { Id = new Guid("001bcfa0-dc50-476c-98f6-010386ee056f"), ReferenceTypeId = new Guid("5b8243b4-1389-433a-b019-df8823953582"), Code = "poor_housekeeping", Name = "Poor Housekeeping", IsActive = true, CreatedAt = now, UpdatedAt = now });
        dbContext.InvestigationReferenceOptions.Add(new RefInvestigationReference { Id = new Guid("5d40f430-bff1-470c-8529-4433c7cbce0e"), ReferenceTypeId = new Guid("5b8243b4-1389-433a-b019-df8823953582"), Code = "poor_ventilation", Name = "Poor Ventilation", IsActive = true, CreatedAt = now, UpdatedAt = now });
        dbContext.InvestigationReferenceOptions.Add(new RefInvestigationReference { Id = new Guid("d2d0d248-c4c7-4678-b384-6aaa23a60ff3"), ReferenceTypeId = new Guid("5b8243b4-1389-433a-b019-df8823953582"), Code = "improper_storage", Name = "Improper Storage", IsActive = true, CreatedAt = now, UpdatedAt = now });
        dbContext.InvestigationReferenceOptions.Add(new RefInvestigationReference { Id = new Guid("7404b945-1048-4c26-a1c5-b999a354a2f5"), ReferenceTypeId = new Guid("5b8243b4-1389-433a-b019-df8823953582"), Code = "congestion", Name = "Congestion", IsActive = true, CreatedAt = now, UpdatedAt = now });
        dbContext.InvestigationReferenceOptions.Add(new RefInvestigationReference { Id = new Guid("d2954179-d009-418d-8ab6-a51ac70a221d"), ReferenceTypeId = new Guid("5b8243b4-1389-433a-b019-df8823953582"), Code = "inadequate_warning", Name = "Inadequate Warning", IsActive = true, CreatedAt = now, UpdatedAt = now });
        dbContext.InvestigationReferenceOptions.Add(new RefInvestigationReference { Id = new Guid("1c257126-a6c1-4a54-954c-732a308e7be7"), ReferenceTypeId = new Guid("5b8243b4-1389-433a-b019-df8823953582"), Code = "inadequate_guard", Name = "Inadequate Guard", IsActive = true, CreatedAt = now, UpdatedAt = now });
        dbContext.InvestigationReferenceOptions.Add(new RefInvestigationReference { Id = new Guid("12366309-cb9d-45c8-8c46-988f873da539"), ReferenceTypeId = new Guid("5b8243b4-1389-433a-b019-df8823953582"), Code = "fire_hazard", Name = "Fire Hazard", IsActive = true, CreatedAt = now, UpdatedAt = now });
        dbContext.InvestigationReferenceOptions.Add(new RefInvestigationReference { Id = new Guid("2845dfaf-aebd-40cb-a844-674b89762cf5"), ReferenceTypeId = new Guid("5b8243b4-1389-433a-b019-df8823953582"), Code = "poor_lighting", Name = "Poor Lighting", IsActive = true, CreatedAt = now, UpdatedAt = now });
        dbContext.InvestigationReferenceOptions.Add(new RefInvestigationReference { Id = new Guid("7ee4356f-9523-49ca-afa7-52ace29bd259"), ReferenceTypeId = new Guid("5b8243b4-1389-433a-b019-df8823953582"), Code = "road_conditions", Name = "Road Conditions", IsActive = true, CreatedAt = now, UpdatedAt = now });
        dbContext.InvestigationReferenceOptions.Add(new RefInvestigationReference { Id = new Guid("03456359-3a6f-49bf-a34f-5a58aae026e6"), ReferenceTypeId = new Guid("5b8243b4-1389-433a-b019-df8823953582"), Code = "weather", Name = "Weather", IsActive = true, CreatedAt = now, UpdatedAt = now });
        dbContext.InvestigationReferenceOptions.Add(new RefInvestigationReference { Id = new Guid("d0761c03-550e-416c-9c4d-daf0fb96301b"), ReferenceTypeId = new Guid("5b8243b4-1389-433a-b019-df8823953582"), Code = "tripping_hazards", Name = "Tripping Hazards", IsActive = true, CreatedAt = now, UpdatedAt = now });
        dbContext.InvestigationReferenceOptions.Add(new RefInvestigationReference { Id = new Guid("1c535d8c-fecf-47be-9df5-e9f2eb2bbe04"), ReferenceTypeId = new Guid("5b8243b4-1389-433a-b019-df8823953582"), Code = "condition_change", Name = "Condition Change", IsActive = true, CreatedAt = now, UpdatedAt = now });

        // --- Investigation Required options ---
        var refType_investigationRequired = new RefReferenceType { Id = new Guid("a9f1c3d2-0011-4e88-b000-222200000001"), Code = "investigation_required", Name = "investigation_required", AppliesTo = "incident_report", IsActive = true, CreatedAt = now, UpdatedAt = now };
        dbContext.ReferenceTypes.Add(refType_investigationRequired);
        dbContext.IncidentReportReferenceOptions.Add(new RefIncidentReportReference { Id = new Guid("a9f1c3d2-0011-4e88-b000-222200000002"), ReferenceTypeId = refType_investigationRequired.Id, Code = "formal_investigation", Name = "Formal Investigation", IsActive = true, CreatedAt = now, UpdatedAt = now });
        dbContext.IncidentReportReferenceOptions.Add(new RefIncidentReportReference { Id = new Guid("a9f1c3d2-0011-4e88-b000-222200000003"), ReferenceTypeId = refType_investigationRequired.Id, Code = "full_cause_map",       Name = "Full Cause Map",       IsActive = true, CreatedAt = now, UpdatedAt = now });

        dbContext.SaveChanges();
    }

    private static void SeedCompaniesRegionsAndSeverities(AppDbContext dbContext)
    {
        if (dbContext.Companies.Any())
            return;

        var now = DateTime.UtcNow;

        // --- Companies (real SHC companies — codes are derived, update when official codes are provided) ---
        var compCCI = new RefCompany { Id = new Guid("a1000000-0000-0000-0000-000000000001"), Code = "CCI", Name = "Catalyst Changers Inc.",              NextIncidentNumber = 1, IsActive = true, CreatedAt = now, UpdatedAt = now };
        var compCSL = new RefCompany { Id = new Guid("a1000000-0000-0000-0000-000000000002"), Code = "CSL", Name = "Cat-Spec Ltd.",                       NextIncidentNumber = 1, IsActive = true, CreatedAt = now, UpdatedAt = now };
        var compETS = new RefCompany { Id = new Guid("a1000000-0000-0000-0000-000000000003"), Code = "ETS", Name = "Elite Turnaround Specialists Ltd.",    NextIncidentNumber = 1, IsActive = true, CreatedAt = now, UpdatedAt = now };
        var compSHU = new RefCompany { Id = new Guid("a1000000-0000-0000-0000-000000000004"), Code = "SHU", Name = "Stronghold University",                NextIncidentNumber = 1, IsActive = true, CreatedAt = now, UpdatedAt = now };
        var compSIL = new RefCompany { Id = new Guid("a1000000-0000-0000-0000-000000000005"), Code = "SIL", Name = "Stronghold Inspections Ltd.",          NextIncidentNumber = 1, IsActive = true, CreatedAt = now, UpdatedAt = now };
        var compSTG = new RefCompany { Id = new Guid("a1000000-0000-0000-0000-000000000006"), Code = "STG", Name = "Stronghold Tower Group",               NextIncidentNumber = 1, IsActive = true, CreatedAt = now, UpdatedAt = now };
        var compSTS = new RefCompany { Id = new Guid("a1000000-0000-0000-0000-000000000007"), Code = "STS", Name = "Stronghold Tank Services Ltd.",         NextIncidentNumber = 3, IsActive = true, CreatedAt = now, UpdatedAt = now };
        var compTIE = new RefCompany { Id = new Guid("a1000000-0000-0000-0000-000000000008"), Code = "TIE", Name = "Turnkey I&E Ltd.",                     NextIncidentNumber = 1, IsActive = true, CreatedAt = now, UpdatedAt = now };
        dbContext.Companies.AddRange(compCCI, compCSL, compETS, compSHU, compSIL, compSTG, compSTS, compTIE);

        // --- Regions (real SHC regions) ---
        // CCI (Catalyst Changers Inc.) operates only in Canada — Edmonton and Sundre confirmed.
        // All other company-region mappings are pending from Thad; those regions stay CompanyId=null
        // and the API fallback shows all regions for unmapped companies until the full mapping arrives.
        var compIdCCI = new Guid("a1000000-0000-0000-0000-000000000001");
        dbContext.Regions.AddRange(
            new RefRegion { Id = new Guid("b1000000-0000-0000-0000-000000000001"), CompanyId = null,      Code = "BAO", Name = "Broken Arrow, OK",      IsActive = true, CreatedAt = now, UpdatedAt = now },
            new RefRegion { Id = new Guid("b1000000-0000-0000-0000-000000000002"), CompanyId = null,      Code = "CFW", Name = "Chippewa Falls, WI",     IsActive = true, CreatedAt = now, UpdatedAt = now },
            new RefRegion { Id = new Guid("b1000000-0000-0000-0000-000000000003"), CompanyId = null,      Code = "CCA", Name = "Concord, CA",             IsActive = true, CreatedAt = now, UpdatedAt = now },
            new RefRegion { Id = new Guid("b1000000-0000-0000-0000-000000000004"), CompanyId = compIdCCI, Code = "EAB", Name = "Edmonton, AB",            IsActive = true, CreatedAt = now, UpdatedAt = now },
            new RefRegion { Id = new Guid("b1000000-0000-0000-0000-000000000005"), CompanyId = null,      Code = "LTX", Name = "Laporte, TX",             IsActive = true, CreatedAt = now, UpdatedAt = now },
            new RefRegion { Id = new Guid("b1000000-0000-0000-0000-000000000006"), CompanyId = null,      Code = "LBC", Name = "Long Beach, CA",          IsActive = true, CreatedAt = now, UpdatedAt = now },
            new RefRegion { Id = new Guid("b1000000-0000-0000-0000-000000000007"), CompanyId = null,      Code = "LMS", Name = "Lucedale, MS",            IsActive = true, CreatedAt = now, UpdatedAt = now },
            new RefRegion { Id = new Guid("b1000000-0000-0000-0000-000000000008"), CompanyId = null,      Code = "MNJ", Name = "Mickleton, NJ",           IsActive = true, CreatedAt = now, UpdatedAt = now },
            new RefRegion { Id = new Guid("b1000000-0000-0000-0000-000000000009"), CompanyId = null,      Code = "NTX", Name = "Nederland, TX",           IsActive = true, CreatedAt = now, UpdatedAt = now },
            new RefRegion { Id = new Guid("b1000000-0000-0000-0000-000000000010"), CompanyId = null,      Code = "OUT", Name = "Ogden, UT",               IsActive = true, CreatedAt = now, UpdatedAt = now },
            new RefRegion { Id = new Guid("b1000000-0000-0000-0000-000000000011"), CompanyId = null,      Code = "OOH", Name = "Oregon, OH",              IsActive = true, CreatedAt = now, UpdatedAt = now },
            new RefRegion { Id = new Guid("b1000000-0000-0000-0000-000000000012"), CompanyId = null,      Code = "PTT", Name = "Point Lisas, TT",         IsActive = true, CreatedAt = now, UpdatedAt = now },
            new RefRegion { Id = new Guid("b1000000-0000-0000-0000-000000000013"), CompanyId = null,      Code = "RLA", Name = "Reserve, LA",             IsActive = true, CreatedAt = now, UpdatedAt = now },
            new RefRegion { Id = new Guid("b1000000-0000-0000-0000-000000000014"), CompanyId = null,      Code = "RTX", Name = "Robstown, TX",            IsActive = true, CreatedAt = now, UpdatedAt = now },
            new RefRegion { Id = new Guid("b1000000-0000-0000-0000-000000000015"), CompanyId = null,      Code = "SON", Name = "Sarnia, ON",              IsActive = true, CreatedAt = now, UpdatedAt = now },
            new RefRegion { Id = new Guid("b1000000-0000-0000-0000-000000000016"), CompanyId = null,      Code = "SLA", Name = "Sulphur, LA",             IsActive = true, CreatedAt = now, UpdatedAt = now },
            new RefRegion { Id = new Guid("b1000000-0000-0000-0000-000000000017"), CompanyId = compIdCCI, Code = "SAB", Name = "Sundre, AB",              IsActive = true, CreatedAt = now, UpdatedAt = now }
        );

        // --- Severities (actual) — Minor / Serious / Major per SHC034 form ---
        dbContext.Set<RefSeverity>("SeveritiesActual").AddRange(
            new RefSeverity { Id = new Guid("c1000000-0000-0000-0000-000000000001"), Code = "MINOR",   Name = "Minor",   Rank = 1, IsActive = true },
            new RefSeverity { Id = new Guid("c1000000-0000-0000-0000-000000000002"), Code = "SERIOUS", Name = "Serious", Rank = 2, IsActive = true },
            new RefSeverity { Id = new Guid("c1000000-0000-0000-0000-000000000003"), Code = "MAJOR",   Name = "Major",   Rank = 3, IsActive = true }
        );

        // --- Severities (potential) ---
        dbContext.Set<RefSeverity>("SeveritiesPotential").AddRange(
            new RefSeverity { Id = new Guid("d1000000-0000-0000-0000-000000000001"), Code = "MINOR",   Name = "Minor",   Rank = 1, IsActive = true },
            new RefSeverity { Id = new Guid("d1000000-0000-0000-0000-000000000002"), Code = "SERIOUS", Name = "Serious", Rank = 2, IsActive = true },
            new RefSeverity { Id = new Guid("d1000000-0000-0000-0000-000000000003"), Code = "MAJOR",   Name = "Major",   Rank = 3, IsActive = true }
        );

        dbContext.SaveChanges();
    }

    private static void SeedTestIncidents(AppDbContext dbContext)
    {
        if (dbContext.IncidentReports.Any())
            return;

        var now = DateTime.UtcNow;
        // Stronghold Tank Services (STS) — a1000000-...007; Laporte TX — b1000000-...005; Nederland TX — b1000000-...009
        var compSTS = new Guid("a1000000-0000-0000-0000-000000000007");
        var regionLTX = new Guid("b1000000-0000-0000-0000-000000000005");
        var regionNTX = new Guid("b1000000-0000-0000-0000-000000000009");

        // ------------------------------------------------------------------
        // Incident 1 — Recordable Injury: Slip/Fall on wet grating
        // ------------------------------------------------------------------
        var incident1 = new IncidentReport
        {
            Id = new Guid("e1000000-0000-0000-0000-000000000001"),
            IncidentNumber = "STS-2025-0001",
            Status = "SUBMIT",
            IncidentDate = new DateTime(2025, 3, 4, 8, 30, 0, DateTimeKind.Utc),
            CompanyId = compSTS,
            RegionId = regionLTX,
            JobNumber = "25-3847",
            ClientCode = "ATMOS",
            PlantCode = "BPT-DIST",
            WorkDescription = "Scheduled meter set inspection and pressure verification on 2-inch distribution main. Crew of three performing routine monthly compliance checks at customer delivery points along Beaumont industrial corridor.",
            IncidentClass = "Actual",
            SeverityActualCode = "SERIOUS",
            SeverityPotentialCode = "MAJOR",
            NatureOfInjury = "Laceration",
            BodyPartsInjured = "Right forearm",
            FormalInvestigationRequired = true,
            FullCauseMapRequired = false,
            IncidentSummary = "Employee M. Thompson slipped on wet diamond-plate grating adjacent to a meter set assembly while descending from the meter deck during a scheduled pipeline inspection. Light rain had been falling since approximately 0700 hrs, creating slick surface conditions. Employee fell approximately 18 inches to the ground and sustained a 3 cm laceration to the right forearm. Wound required 4 sutures at Christus St. Elizabeth urgent care. Employee was wearing all required PPE including cut-resistant gloves, hard hat, and safety glasses at the time of incident. Crew supervisor immediately rendered first aid and transported employee for medical evaluation. Work was suspended pending area inspection and corrective action.",
            CreatedAt = now,
            UpdatedAt = now,
        };
        incident1.References = new List<IncidentReportReference>
        {
            // incidenttype: Injury
            new() { IncidentReportId = incident1.Id, ReferenceId = new Guid("ce2931ba-a56f-46fc-adc3-eb022b6d7bdf") },
            // injurytype: Fall Same Level
            new() { IncidentReportId = incident1.Id, ReferenceId = new Guid("60ace0bd-392e-4197-ba11-695d7bf2ea0c") },
            // injurydetail: Medical Aid
            new() { IncidentReportId = incident1.Id, ReferenceId = new Guid("df42a15c-5a14-49ff-ad21-3271f3935a36") },
        };
        incident1.EmployeesInvolved = new List<IncidentEmployeeInvolved>
        {
            new()
            {
                Id = new Guid("f1000000-0000-0000-0000-000000000001"),
                IncidentReportId = incident1.Id,
                EmployeeIdentifier = "T-10482",
                EmployeeName = "Marcus D. Thompson",
                InjuryTypeCode = "Fall Same Level",
                Recordable = true,
                HoursWorked = 6.5m,
                CreatedAt = now,
                UpdatedAt = now,
            }
        };

        // ------------------------------------------------------------------
        // Incident 2 — Near Miss: MVA, third-party lane departure on IH-37
        // ------------------------------------------------------------------
        var incident2 = new IncidentReport
        {
            Id = new Guid("e1000000-0000-0000-0000-000000000002"),
            IncidentNumber = "STS-2025-0002",
            Status = "FIRSTREPORT",
            IncidentDate = new DateTime(2025, 3, 11, 14, 15, 0, DateTimeKind.Utc),
            CompanyId = compSTS,
            RegionId = regionNTX,
            JobNumber = "25-4106",
            ClientCode = "CNPT",
            PlantCode = "CRP-SVC",
            WorkDescription = "Driving to customer site to perform gas service restoration following a planned system outage. Employee was traveling to the Nederland Service Center yard to pick up replacement regulators before proceeding to the job site.",
            IncidentClass = "NearMiss",
            SeverityActualCode = "MINOR",
            SeverityPotentialCode = "SERIOUS",
            TypeOfEquipment = "2023 Ford F-250 Super Duty",
            UnitNumbers = "SES-T-0228",
            Visibility = "Clear, dry conditions, mid-afternoon",
            FormalInvestigationRequired = false,
            FullCauseMapRequired = false,
            IncidentSummary = "Employee R. Garza was traveling southbound on IH-37 near mile marker 14 at approximately 65 mph when a third-party passenger vehicle drifted from the center lane into the company vehicle's right lane without signaling. Employee took immediate evasive action — braking and steering onto the right shoulder — and successfully avoided contact. No collision occurred and no injuries were sustained. The third-party vehicle did not stop. Minor scuff damage to the right rear tire sidewall from road debris on the shoulder was noted during post-incident vehicle inspection. Vehicle was removed from service, inspected by a certified technician, and returned to service the following morning. Incident reported to supervisor within 30 minutes per policy.",
            CreatedAt = now,
            UpdatedAt = now,
        };
        incident2.References = new List<IncidentReportReference>
        {
            // incidenttype: Motor Vehicle Accident
            new() { IncidentReportId = incident2.Id, ReferenceId = new Guid("451944b5-d133-470f-a2d0-02e21995d4f0") },
            // motorvehicleaccident: Third Party
            new() { IncidentReportId = incident2.Id, ReferenceId = new Guid("c1f41d3b-7c7a-435b-87b0-ec41517b1587") },
        };
        incident2.EmployeesInvolved = new List<IncidentEmployeeInvolved>
        {
            new()
            {
                Id = new Guid("f1000000-0000-0000-0000-000000000002"),
                IncidentReportId = incident2.Id,
                EmployeeIdentifier = "G-07715",
                EmployeeName = "Roberto J. Garza",
                Recordable = false,
                HoursWorked = 4.25m,
                CreatedAt = now,
                UpdatedAt = now,
            }
        };

        dbContext.IncidentReports.AddRange(incident1, incident2);
        dbContext.SaveChanges();
    }
}
