namespace Stronghold.AppDashboard.Data.Models.Safety;

public class IncidentReport
{
    public Guid Id { get; set; }
    public string IncidentNumber { get; set; } = null!;
    public string Status { get; set; } = null!;
    public DateTime IncidentDate { get; set; }
    public Guid? CompanyId { get; set; }
    public Guid? RegionId { get; set; }
    public string? JobNumber { get; set; }
    public string? ClientCode { get; set; }
    public string? PlantCode { get; set; }
    public string? WorkDescription { get; set; }
    public string? IncidentSummary { get; set; }
    public string? IncidentClass { get; set; }
    public string? SeverityActualCode { get; set; }
    public string? SeverityPotentialCode { get; set; }
    public Guid? HealthSafetyLeaderId { get; set; }
    public Guid? SeniorOpsLeaderId { get; set; }
    // Form fields from ERD — injury / equipment / MVA detail
    public string? BodyPartsInjured { get; set; }
    public string? NatureOfInjury { get; set; }
    public string? TypeOfEquipment { get; set; }
    public string? UnitNumbers { get; set; }
    public string? Visibility { get; set; }
    // Investigation fields
    public string? InvestigationDetails { get; set; }
    public bool FormalInvestigationRequired { get; set; }
    public bool FullCauseMapRequired { get; set; }
    public DateTime CreatedAt { get; set; }
    public DateTime UpdatedAt { get; set; }

    public RefCompany? Company { get; set; }
    public RefRegion? Region { get; set; }
    public ICollection<IncidentEmployeeInvolved> EmployeesInvolved { get; set; } = new List<IncidentEmployeeInvolved>();
    public ICollection<IncidentAction> Actions { get; set; } = new List<IncidentAction>();
    public ICollection<IncidentReportReference> References { get; set; } = new List<IncidentReportReference>();
}
