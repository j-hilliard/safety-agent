using System.Runtime.CompilerServices;
using MediatR;

namespace Stronghold.AppDashboard.Api.Domain
{
    public abstract class PipelineBehavior<TRequest, TResponse>
        : IPipelineBehavior<TRequest, TResponse>
        where TRequest : IRequest<TResponse>
    {
        private readonly ILogger _logger;

        protected PipelineBehavior(ILogger logger)
        {
            _logger = logger;
        }

        public abstract Task<TResponse> Handle(
            TRequest request,
            RequestHandlerDelegate<TResponse> next,
            CancellationToken cancellationToken
        );

        protected async Task<TResponse> TryGetAsync(
            Func<ILogger, Task<TResponse>> func,
            [CallerMemberName] string? callerMemberName = null
        )
        {
            try
            {
                _logger.LogInformation($"Starting execution of {callerMemberName}");
                var result = await func(_logger);
                _logger.LogInformation($"Finished execution of {callerMemberName}");
                return result;
            }
            catch (Exception ex)
            {
                _logger.LogError(ex, $"Exception in {callerMemberName}");
                throw;
            }
        }
    }
}
