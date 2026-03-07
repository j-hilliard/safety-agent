using AutoMapper;
using FluentValidation;
using MediatR;

namespace Stronghold.AppDashboard.Api.Models;

public class UserRole
{
    public int UserId { get; set; }
    public User User { get; set; } = null!;

    public int RoleId { get; set; }
    public Role Role { get; set; } = null!;
}

public class UserRoleProfile : Profile
{
    public UserRoleProfile()
    {
        CreateMap<UserRole, Data.Models.UserRole>();
        CreateMap<Data.Models.UserRole, UserRole>();
    }
}

public class UserRoleValidator : AbstractValidator<UserRole>
{
    public UserRoleValidator(IMediator mediator)
    {
        RuleLevelCascadeMode = CascadeMode.Stop;

        RuleFor(userRole => userRole.RoleId)
            .NotEmpty()
            .WithMessage("RoleId must not be empty.")
            .GreaterThan(0)
            .WithMessage("RoleId must be greater than 0.");

        RuleFor(userRole => userRole.UserId)
            .NotEmpty()
            .WithMessage("UserId must not be empty.")
            .GreaterThan(0)
            .WithMessage("UserId must be greater than 0.");
    }
}
