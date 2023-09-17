using FluentValidation;
using SynonymLookup.Api.Features.SynonymWriter.Validators;
using SynonymLookup.Api.Validation;

namespace SynonymLookup.Api.Features.SynonymWriter.Models;

public record Word : IValidatable<Word>
{
    public Word(string value)
    {
        Value = value;
    }

    public string Value { get; init; }

    public AbstractValidator<Word> Validator => WordValidator.Instance;
}
