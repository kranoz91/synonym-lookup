using FluentValidation;
using SynonymLookup.Api.Features.SynonymWriter.Models;

namespace SynonymLookup.Api.Features.SynonymWriter.Validators;

internal class WordValidator : AbstractValidator<Word>
{
	public const int MAX_LENGTH = 64;

    public static WordValidator Instance = new WordValidator();

	public WordValidator()
	{
		RuleFor(_ => _.Value).NotEmpty().WithErrorCode(nameof(Errors.SL201));
		RuleFor(_ => _.Value).MaximumLength(MAX_LENGTH).WithErrorCode(nameof(Errors.SL202));
		RuleFor(_ => _.Value).Matches("[a-zA-Z]+").WithErrorCode(nameof(Errors.SL203));
	}
}
