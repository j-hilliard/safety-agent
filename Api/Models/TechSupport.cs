using AutoMapper;
using FluentValidation;
using MediatR;
using Stronghold.AppDashboard.Shared.Attributes;

namespace Stronghold.AppDashboard.Api.Models;

public class TechSupport
{
    public required int TechSupportId { get; set; }
    public required int ApplicationId { get; set; }
    public required string Name { get; set; } = null!;

    [Sensitive]
    public required string Email { get; set; } = null!;

    [Sensitive]
    public required string Phone { get; set; } = null!;

    [Sensitive]
    public required string Title { get; set; } = null!;

    [Sensitive]
    public required string Department { get; set; } = null!;
    public required string Description { get; set; } = null!;
    public DateTimeOffset Created { get; set; }
    public DateTimeOffset LastUpdated { get; set; }

    public DateTimeOffset CreatedOn { get; set; } = DateTimeOffset.UtcNow;
    public DateTimeOffset ModifiedOn { get; set; } = DateTimeOffset.UtcNow;
    public int CreatedById { get; set; }
    public int ModifiedById { get; set; }
}

public class TechSupportProfile : Profile
{
    public TechSupportProfile()
    {
        CreateMap<TechSupport, Data.Models.TechSupport>()
            .ForMember(ts => ts.TechSupportId, expression => expression.Ignore());

        CreateMap<Data.Models.TechSupport, TechSupport>();
    }
}

public class TechSupportValidator : AbstractValidator<TechSupport>
{
    public TechSupportValidator(IMediator mediator)
    {
        RuleLevelCascadeMode = CascadeMode.Stop;

        RuleFor(user => user.ApplicationId)
            .NotEmpty()
            .WithMessage("ApplicationId must not be empty.")
            .NotNull()
            .WithMessage("ApplicationId must not be null.");

        RuleFor(user => user.Name)
            .NotEmpty()
            .WithMessage("Tech Support Name must not be empty.")
            .NotNull()
            .WithMessage("Tech Support Name must not be null.");

        RuleFor(user => user.Email)
            .NotEmpty()
            .WithMessage("Tech Support Email must not be empty.")
            .NotNull()
            .WithMessage("Tech Support Email must not be null.");
    }
}
