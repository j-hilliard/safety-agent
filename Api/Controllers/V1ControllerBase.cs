using System.Runtime.CompilerServices;
using MediatR;
using Microsoft.AspNetCore.Authorization;
using Microsoft.AspNetCore.Mvc;
using Microsoft.EntityFrameworkCore;
using Stronghold.AppDashboard.Api.Domain.Users;
using Stronghold.AppDashboard.Api.Helpers;
using Stronghold.AppDashboard.Api.Models;

namespace Stronghold.AppDashboard.Api.Controllers
{
    [Authorize(AuthenticationSchemes = Constants.AuthenticationSchemes.AzureAd)]
    [ApiVersion(Constants.ApiVersions.V1)]
    [ApiController]
    public abstract class V1ControllerBase : ControllerBase
    {
        private ILogger Logger { get; }
        protected IMediator Mediator { get; }

        protected V1ControllerBase(IMediator mediator, ILogger<V1ControllerBase> logger)
        {
            Logger = logger;
            Mediator = mediator;
        }

        protected async Task<User> GetUser()
        {
            var user = await Mediator.Send(
                new GetUserByClaimsPrincipal { ClaimsPrincipal = HttpContext.User }
            );

            if (user == null || user.Active == false)
                throw new UnauthorizedAccessException();

            return user;
        }

        protected async Task<TResult> TryExecuteAsync<TResult>(
            Func<Task<TResult>> func,
            Func<Exception, Task<TResult>>? onException = null,
            [CallerMemberName] string? callerMemberName = null
        )
        {
            try
            {
                var result = await func();
                return result;
            }
            catch (Exception ex)
            {
                Logger.LogError(ex, "An error occurred in {CallerMemberName}", callerMemberName);

                if (onException != null)
                {
                    var exceptionResult = await onException!(ex);
                    if (exceptionResult != null)
                    {
                        return exceptionResult;
                    }
                }

                throw;
            }
        }

        protected Task<ActionResult<TResult>> Error<TResult>(
            Exception ex,
            ProblemDetails? details = null
        )
        {
            return Task.FromResult<ActionResult<TResult>>(
                ex switch
                {
                    ArgumentException => BadRequest(details),
                    UnauthorizedAccessException => Unauthorized(details),
                    DbUpdateException => Conflict(details),
                    _ => StatusCode(StatusCodes.Status500InternalServerError, details),
                }
            );
        }

        protected Task<IActionResult> Error(Exception ex, ProblemDetails? details = null)
        {
            return Task.FromResult<IActionResult>(
                ex switch
                {
                    ArgumentException => BadRequest(details),
                    UnauthorizedAccessException => Unauthorized(details),
                    DbUpdateException => Conflict(details),
                    _ => StatusCode(StatusCodes.Status500InternalServerError, details),
                }
            );
        }
    }
}
