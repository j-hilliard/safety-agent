using AutoMapper;
using FluentValidation;
using MediatR;

namespace Stronghold.AppDashboard.Api.Models;

public class Settings
{
    public int SettingsId { get; set; }
    public string ADUsersGroupName { get; set; } = null!;
    public Guid ADUsersGroupGUID { get; set; }
    public string ADAdminsGroupName { get; set; } = null!;
    public Guid ADAdminsGroupGUID { get; set; }

    public DateTimeOffset CreatedOn { get; set; } = DateTimeOffset.UtcNow;
    public DateTimeOffset ModifiedOn { get; set; } = DateTimeOffset.UtcNow;
    public int CreatedById { get; set; }
    public int ModifiedById { get; set; }
}

public class SettingsProfile : Profile
{
    public SettingsProfile()
    {
        CreateMap<Data.Models.Settings, Settings>()
            .ReverseMap()
            .ForMember(role => role.SettingsId, expression => expression.Ignore());
    }
}

public class SettingsValidator : AbstractValidator<Settings>
{
    public SettingsValidator(IMediator mediator)
    {
        RuleLevelCascadeMode = CascadeMode.Stop;

        RuleFor(settings => settings.ADUsersGroupName).NotEmpty();
        RuleFor(settings => settings.ADUsersGroupGUID).NotEmpty();
        RuleFor(settings => settings.ADAdminsGroupName).NotEmpty();
        RuleFor(settings => settings.ADAdminsGroupGUID).NotEmpty();
    }
}
