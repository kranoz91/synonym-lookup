using FluentValidation;
using SynonymLookup.Api.Features.SynonymWriter.Models;

namespace SynonymLookup.Api.Features.SynonymWriter.Validators;

internal class WordValidator : AbstractValidator<Word>
{
	public const int MAX_LENGTH = 64;

    public static WordValidator Instance = new WordValidator();

	public WordValidator()
	{
		RuleFor(_ => _.Value)
			.Cascade(CascadeMode.Stop)
			.NotEmpty().WithErrorCode(nameof(Errors.SL201))
			.MaximumLength(MAX_LENGTH).WithErrorCode(nameof(Errors.SL202))
			.Matches("^[a-öA-Ö]+$").WithErrorCode(nameof(Errors.SL203));
	}
}
