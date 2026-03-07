using AutoMapper;
using FluentValidation;
using MediatR;
using Stronghold.AppDashboard.Shared.Attributes;

namespace Stronghold.AppDashboard.Api.Models;

public class User
{
    public int UserId { get; set; }
    public Guid AzureAdObjectId { get; set; }

    [Sensitive]
    public string? FirstName { get; set; }

    [Sensitive]
    public string? LastName { get; set; }

    [Sensitive]
    public string? Email { get; set; }

    [Sensitive]
    public string? Company { get; set; }

    [Sensitive]
    public string? Department { get; set; }

    [Sensitive]
    public string? Title { get; set; }
    public bool Active { get; set; } = true;
    public DateTimeOffset? DisabledOn { get; set; }
    public int? DisabledBy { get; set; }
    public string? DisabledReason { get; set; }

    public DateTimeOffset? LastLogin { get; set; }

    public List<UserRole> Roles { get; set; } = new();

    public DateTimeOffset CreatedOn { get; set; } = DateTimeOffset.UtcNow;
    public DateTimeOffset ModifiedOn { get; set; } = DateTimeOffset.UtcNow;
}

public class UserProfile : Profile
{
    public UserProfile()
    {
        CreateMap<User, Data.Models.User>()
            .ForMember(user => user.UserId, expression => expression.Ignore())
            .ForMember(user => user.UserRoles, expression => expression.Ignore());

        CreateMap<Data.Models.User, User>()
            .ForMember(user => user.Roles, expression => expression.Ignore());

        CreateMap<UserRole, Data.Models.UserRole>();
        CreateMap<Data.Models.UserRole, UserRole>();

        CreateMap<Role, Data.Models.Role>();
        CreateMap<Data.Models.Role, Role>();
    }
}

public class UserValidator : AbstractValidator<User>
{
    public UserValidator(IMediator mediator)
    {
        RuleLevelCascadeMode = CascadeMode.Stop;

        RuleFor(user => user.AzureAdObjectId)
            .NotEmpty()
            .WithMessage("AzureAdObjectId must not be empty.")
            .NotNull()
            .WithMessage("AzureAdObjectId must not be null.");

        RuleFor(user => user.Active).NotNull().WithMessage("Active status must be specified.");
    }
}
