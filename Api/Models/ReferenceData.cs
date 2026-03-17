using AutoMapper;

namespace Stronghold.AppDashboard.Api.Models;

public class RefCompanyDto
{
    public Guid Id { get; set; }
    public string Code { get; set; } = null!;
    public string Name { get; set; } = null!;
    public bool IsActive { get; set; }
}

public class RefRegionDto
{
    public Guid Id { get; set; }
    public Guid? CompanyId { get; set; }
    public string Code { get; set; } = null!;
    public string Name { get; set; } = null!;
    public bool IsActive { get; set; }
}

public class RefSeverityDto
{
    public Guid Id { get; set; }
    public string Code { get; set; } = null!;
    public string Name { get; set; } = null!;
    public int Rank { get; set; }
}

public class RefOptionDto
{
    public Guid Id { get; set; }
    public string Code { get; set; } = null!;
    public string Name { get; set; } = null!;
    public string ReferenceTypeCode { get; set; } = null!;
}

public class RefWorkflowStateDto
{
    public Guid Id { get; set; }
    public string Domain { get; set; } = null!;
    public string Code { get; set; } = null!;
    public string Name { get; set; } = null!;
}

public class RefReferenceTypeDto
{
    public Guid Id { get; set; }
    public string Code { get; set; } = null!;
    public string Name { get; set; } = null!;
    public string AppliesTo { get; set; } = null!;
}

public class ReferenceDataProfile : Profile
{
    public ReferenceDataProfile()
    {
        CreateMap<Data.Models.Safety.RefCompany, RefCompanyDto>();
        CreateMap<Data.Models.Safety.RefRegion, RefRegionDto>();
        CreateMap<Data.Models.Safety.RefSeverity, RefSeverityDto>();
        CreateMap<Data.Models.Safety.RefWorkflowState, RefWorkflowStateDto>();
        CreateMap<Data.Models.Safety.RefReferenceType, RefReferenceTypeDto>();
        CreateMap<Data.Models.Safety.RefIncidentReportReference, RefOptionDto>()
            .ForMember(d => d.ReferenceTypeCode, opt => opt.MapFrom(s => s.ReferenceType != null ? s.ReferenceType.Code : null));
    }
}
