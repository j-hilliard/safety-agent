using FluentValidation.Results;

namespace Stronghold.AppDashboard.Api.Helpers;

public static class ValidationResultExtensions
{
    public static IEnumerable<ValidationError> ToValidationErrors(this ValidationResult? result)
    {
        if (result == null || result.IsValid)
            yield break;

        foreach (var failure in result.Errors)
        {
            yield return new ValidationError
            {
                FieldName = failure.PropertyName,
                Message = failure.ErrorMessage,
            };
        }
    }
}

public class ValidationError
{
    public string FieldName { get; set; } = string.Empty;
    public string Message { get; set; } = string.Empty;
}
