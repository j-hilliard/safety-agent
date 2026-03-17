using AutoMapper;
using FluentValidation;

namespace Stronghold.AppDashboard.Api.Models;

public class IncidentReport
{
    public Guid Id { get; set; }
    public string? IncidentNumber { get; set; }
    public string? Status { get; set; }
    public DateTime IncidentDate { get; set; }
    public Guid? CompanyId { get; set; }
    public string? CompanyName { get; set; }
    public Guid? RegionId { get; set; }
    public string? RegionName { get; set; }
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
    public string? BodyPartsInjured { get; set; }
    public string? NatureOfInjury { get; set; }
    public string? TypeOfEquipment { get; set; }
    public string? UnitNumbers { get; set; }
    public string? Visibility { get; set; }
    public string? InvestigationDetails { get; set; }
    public bool FormalInvestigationRequired { get; set; }
    public bool FullCauseMapRequired { get; set; }
    public DateTime CreatedAt { get; set; }
    public DateTime UpdatedAt { get; set; }

    public List<IncidentEmployeeInvolved> EmployeesInvolved { get; set; } = new();
    public List<IncidentAction> Actions { get; set; } = new();
    public List<Guid> ReferenceIds { get; set; } = new();
}

public class IncidentReportListItem
{
    public Guid Id { get; set; }
    public string? IncidentNumber { get; set; }
    public DateTime IncidentDate { get; set; }
    public string? CompanyName { get; set; }
    public string? RegionName { get; set; }
    public string? JobNumber { get; set; }
    public string? ClientCode { get; set; }
    public string? Status { get; set; }
    public string? SeverityActualCode { get; set; }
    public DateTime CreatedAt { get; set; }
}

public class IncidentEmployeeInvolved
{
    public Guid Id { get; set; }
    public string? EmployeeIdentifier { get; set; }
    public string? EmployeeName { get; set; }
    public string? InjuryTypeCode { get; set; }
    public bool? Recordable { get; set; }
    public decimal? HoursWorked { get; set; }
}

public class IncidentAction
{
    public Guid Id { get; set; }
    public string? ActionType { get; set; }
    public string? ActionDescription { get; set; }
    public string? AssignedTo { get; set; }
    public DateTime? DueDate { get; set; }
    public string? Status { get; set; }
    public DateTime? ClosedAt { get; set; }
}

public class IncidentReportProfile : Profile
{
    public IncidentReportProfile()
    {
        CreateMap<Data.Models.Safety.IncidentReport, IncidentReport>()
            .ForMember(d => d.CompanyName, opt => opt.MapFrom(s => s.Company != null ? s.Company.Name : null))
            .ForMember(d => d.RegionName, opt => opt.MapFrom(s => s.Region != null ? s.Region.Name : null))
            .ForMember(d => d.ReferenceIds, opt => opt.MapFrom(s => s.References.Select(r => r.ReferenceId).ToList()));

        CreateMap<IncidentReport, Data.Models.Safety.IncidentReport>()
            .ForMember(d => d.Company, opt => opt.Ignore())
            .ForMember(d => d.Region, opt => opt.Ignore())
            .ForMember(d => d.References, opt => opt.Ignore())
            .ForMember(d => d.EmployeesInvolved, opt => opt.Ignore())
            .ForMember(d => d.Actions, opt => opt.Ignore());

        CreateMap<Data.Models.Safety.IncidentReport, IncidentReportListItem>()
            .ForMember(d => d.CompanyName, opt => opt.MapFrom(s => s.Company != null ? s.Company.Name : null))
            .ForMember(d => d.RegionName, opt => opt.MapFrom(s => s.Region != null ? s.Region.Name : null));

        CreateMap<Data.Models.Safety.IncidentEmployeeInvolved, IncidentEmployeeInvolved>();
        CreateMap<IncidentEmployeeInvolved, Data.Models.Safety.IncidentEmployeeInvolved>()
            .ForMember(d => d.IncidentReportId, opt => opt.Ignore())
            .ForMember(d => d.IncidentReport, opt => opt.Ignore())
            .ForMember(d => d.CreatedAt, opt => opt.Ignore())
            .ForMember(d => d.UpdatedAt, opt => opt.Ignore());

        CreateMap<Data.Models.Safety.IncidentAction, IncidentAction>();
        CreateMap<IncidentAction, Data.Models.Safety.IncidentAction>()
            .ForMember(d => d.IncidentReportId, opt => opt.Ignore())
            .ForMember(d => d.IncidentReport, opt => opt.Ignore())
            .ForMember(d => d.CreatedAt, opt => opt.Ignore())
            .ForMember(d => d.UpdatedAt, opt => opt.Ignore());
    }
}

public class IncidentReportValidator : AbstractValidator<IncidentReport>
{
    public IncidentReportValidator()
    {
        RuleLevelCascadeMode = CascadeMode.Stop;

        RuleFor(r => r.IncidentDate)
            .NotEmpty()
            .WithMessage("Incident date is required.");

        RuleFor(r => r.CompanyId)
            .NotEmpty()
            .WithMessage("Company is required.");

        RuleFor(r => r.IncidentClass)
            .NotEmpty()
            .WithMessage("Incident classification is required.");
    }
}
