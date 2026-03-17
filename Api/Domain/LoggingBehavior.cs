using System.Collections;
using System.Diagnostics;
using System.Reflection;
using MediatR;
using Stronghold.AppDashboard.Api.Helpers;
using Stronghold.AppDashboard.Shared.Attributes;

namespace Stronghold.AppDashboard.Api.Domain
{
    public class LoggingBehavior<TRequest, TResponse> : PipelineBehavior<TRequest, TResponse>
        where TRequest : IRequest<TResponse>
    {
        public LoggingBehavior(ILogger<LoggingBehavior<TRequest, TResponse>> logger)
            : base(logger) { }

        public override async Task<TResponse> Handle(
            TRequest request,
            RequestHandlerDelegate<TResponse> next,
            CancellationToken cancellationToken
        )
        {
            return await TryGetAsync(async logger =>
            {
                var requestType = typeof(TRequest);
                var parametersString = BuildParameterString(requestType, request);

                logger.LogInformation(
                    "Handling {Name} [Parameters={Parameters}]",
                    requestType.Name,
                    parametersString
                );

                var stopwatch = new Stopwatch();

                try
                {
                    stopwatch.Start();

                    var response = await next();

                    stopwatch.Stop();

                    logger.LogInformation(
                        "Handled {RequestTypeName}: Returned {ResponseTypeName} ({Time}ms) ",
                        requestType.Name,
                        typeof(TResponse).Name,
                        stopwatch.ElapsedMilliseconds
                    );

                    return response;
                }
                catch (Exception ex)
                {
                    stopwatch.Stop();
                    logger.LogError(
                        ex,
                        "{Exception} thrown when handling {Name} ({Time}ms)",
                        ex.GetType().Name,
                        requestType.Name,
                        stopwatch.ElapsedMilliseconds
                    );

                    throw;
                }
            });
        }

        private string BuildParameterString(Type type, object? objectValue)
        {
            if (objectValue == null)
                return "null";

            var parameterStrings = new List<string>();
            var props = new List<PropertyInfo>(type.GetProperties());

            foreach (var prop in props)
            {
                if (prop.GetCustomAttributes(typeof(SensitiveAttribute), true).Any())
                {
                    parameterStrings.Add($"{prop.Name}=***");
                    continue;
                }

                var propType = prop.PropertyType;
                var propValue = prop.GetValue(objectValue, null);

                if (propType.IsSimpleType())
                {
                    parameterStrings.Add($"{prop.Name}='{propValue}'");
                }
                else if (propType.IsArray)
                {
                    parameterStrings.Add(
                        $"{prop.Name}={BuildArrayString(propType.GetElementType(), propValue)}"
                    );
                }
                else if (
                    propType.IsGenericType
                    && propType.GetGenericTypeDefinition() == typeof(List<>)
                )
                {
                    var elementType = propType.GetGenericArguments()[0];
                    parameterStrings.Add(
                        $"{prop.Name}={BuildArrayString(elementType, propValue)}"
                    );
                }
                else if (propType.IsClass)
                {
                    parameterStrings.Add(
                        $"{prop.Name}={BuildParameterString(propType, propValue)}"
                    );
                }
                else
                {
                    parameterStrings.Add($"{prop.Name}={propType.Name}");
                }
            }

            return $"[{string.Join(", ", parameterStrings)}]";
        }

        private string BuildArrayString(Type? elementType, object? arrayValue)
        {
            if (elementType == null)
                return "[...]";

            if (arrayValue is not IEnumerable enumerable)
                return "null";

            var values = new List<string?>();

            foreach (var element in enumerable)
            {
                if (elementType.IsSimpleType())
                {
                    values.Add(element == null ? "null" : $"'{element}'");
                }
                else if (elementType.IsArray)
                {
                    values.Add(BuildArrayString(elementType.GetElementType(), element));
                }
                else if (elementType.IsClass)
                {
                    values.Add($"{{{BuildParameterString(elementType, element)}}}");
                }
                else
                {
                    values.Add(element == null ? "null" : elementType.Name);
                }
            }

            return $"[{string.Join(", ", values)}]";
        }
    }
}
