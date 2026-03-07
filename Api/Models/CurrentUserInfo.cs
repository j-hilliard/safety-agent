using FluentValidation;
using MediatR;

namespace Stronghold.AppDashboard.Api.Models;

public class CurrentUserInfo
{
    public string UserId { get; }
    public string Username { get; }
    public string Email { get; }

    public CurrentUserInfo(string userId, string username, string email)
    {
        UserId = userId;
        Username = username;
        Email = email;
    }
}

public class CurrentUserInfoValidator : AbstractValidator<CurrentUserInfo>
{
    public CurrentUserInfoValidator(IMediator mediator)
    {
        RuleLevelCascadeMode = CascadeMode.Stop;

        RuleFor(currentUserInfo => currentUserInfo.UserId).NotEmpty();
        RuleFor(currentUserInfo => currentUserInfo.Username).NotEmpty();
        RuleFor(currentUserInfo => currentUserInfo.Email).NotEmpty();
    }
}
