using FluentValidation;

namespace SynonymLookup.Api.Validation;

public interface IValidatable<T>
{
    public AbstractValidator<T> Validator { get; }
}
