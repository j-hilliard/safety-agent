using AutoMapper;
using FluentValidation;
using MediatR;
using Stronghold.AppDashboard.Shared.Attributes;

namespace Stronghold.AppDashboard.Api.Models;

public class SystemOwner
{
    public int SystemOwnerId { get; set; }
    public int ApplicationId { get; set; }

    [Sensitive]
    public string Name { get; set; } = null!;

    [Sensitive]
    public string Email { get; set; } = null!;

    [Sensitive]
    public string Phone { get; set; } = null!;

    [Sensitive]
    public string Title { get; set; } = null!;

    [Sensitive]
    public string Department { get; set; } = null!;
    public string Description { get; set; } = null!;
    public DateTimeOffset Created { get; set; }
    public DateTimeOffset LastUpdated { get; set; }

    public DateTimeOffset CreatedOn { get; set; } = DateTimeOffset.UtcNow;
    public DateTimeOffset ModifiedOn { get; set; } = DateTimeOffset.UtcNow;
    public int CreatedById { get; set; }
    public int ModifiedById { get; set; }
}

public class SystemOwnerProfile : Profile
{
    public SystemOwnerProfile()
    {
        CreateMap<Data.Models.SystemOwner, SystemOwner>();

        CreateMap<SystemOwner, Data.Models.SystemOwner>()
            .ForMember(user => user.SystemOwnerId, expression => expression.Ignore())
            .ReverseMap();
    }
}

public class SystemOwnerValidator : AbstractValidator<SystemOwner>
{
    public SystemOwnerValidator(IMediator mediator)
    {
        RuleLevelCascadeMode = CascadeMode.Stop;

        RuleFor(user => user.ApplicationId)
            .NotEmpty()
            .WithMessage("ApplicationId must not be empty.")
            .NotNull()
            .WithMessage("ApplicationId must not be null.");

        RuleFor(user => user.Name)
            .NotEmpty()
            .WithMessage("System Owner Name must not be empty.")
            .NotNull()
            .WithMessage("System Owner Name must not be null.");

        RuleFor(user => user.Email)
            .NotEmpty()
            .WithMessage("System Owner Email must not be empty.")
            .NotNull()
            .WithMessage("System Owner Email must not be null.");
    }
}
