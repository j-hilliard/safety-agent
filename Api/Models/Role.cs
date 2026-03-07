using AutoMapper;
using FluentValidation;
using MediatR;

namespace Stronghold.AppDashboard.Api.Models;

public class Role
{
    public int RoleId { get; set; }
    public string Name { get; set; } = null!;
    public string Description { get; set; } = null!;
}

public class RoleProfile : Profile
{
    public RoleProfile()
    {
        CreateMap<Data.Models.Role, Role>()
            .ReverseMap()
            .ForMember(role => role.RoleId, expression => expression.Ignore())
            .ForMember(role => role.UserRoles, expression => expression.Ignore());
    }
}

public class RoleValidator : AbstractValidator<Role>
{
    public RoleValidator(IMediator mediator)
    {
        RuleLevelCascadeMode = CascadeMode.Stop;

        RuleFor(role => role.Name).NotEmpty();
        RuleFor(role => role.Description).NotEmpty();
    }
}
