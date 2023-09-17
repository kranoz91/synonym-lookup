using FluentValidation.Results;
using SynonymLookup.Api.Validation;

namespace SynonymLookup.Api.Extensions;

public static class ValidationExtensions
{
    public static bool IsValid<T>(this IValidatable<T> obj, out ValidationResult result)
    {
        result = obj.Validator.Validate((T)obj);
        return result.IsValid;
    }
}
