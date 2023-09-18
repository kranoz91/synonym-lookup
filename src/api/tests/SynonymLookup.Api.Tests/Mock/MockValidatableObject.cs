using FluentValidation;
using SynonymLookup.Api.Validation;

namespace SynonymLookup.Api.Tests.Mock;

internal class MockValidatableObject : IValidatable<MockValidatableObject>
{
    public AbstractValidator<MockValidatableObject> Validator => MockValidatableObjectValidator.Instance;
}
