using FluentValidation;
using FluentValidation.Results;

namespace SynonymLookup.Api.Tests.Mock;

internal class MockValidatableObjectValidator : AbstractValidator<MockValidatableObject>
{
    public static MockValidatableObjectValidator Instance = new();

    private ValidationResult mockResult;

    public void SetupMockResult(ValidationResult mockResult) => this.mockResult = mockResult;

    public override ValidationResult Validate(ValidationContext<MockValidatableObject> context) => mockResult;
}
