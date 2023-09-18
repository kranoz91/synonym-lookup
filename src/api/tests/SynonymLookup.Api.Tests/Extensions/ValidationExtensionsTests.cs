using FluentAssertions;
using FluentValidation.Results;
using SynonymLookup.Api.Extensions;
using SynonymLookup.Api.Tests.Mock;

namespace SynonymLookup.Api.Tests.Extensions;

public class ValidationExtensionsTests
{
    [Fact]
    public void IsValid_Should_Set_Out_Var_Based_On_ValidationResult()
    {
        // arrange
        var expected = new ValidationResult(new List<ValidationFailure> { new ValidationFailure("someProp", "someMessage") });
        var sut = new MockValidatableObject();
        MockValidatableObjectValidator.Instance.SetupMockResult(expected);

        // act
        sut.IsValid(out var result);

        // assert
        result.Errors[0].ErrorCode.Should().Be(expected.Errors[0].ErrorCode);
        result.Errors[0].ErrorMessage.Should().Be(expected.Errors[0].ErrorMessage);
    }

    [Fact]
    public void IsValid_Should_Return_False_Based_On_ValidationResult_IsValid_Property()
    {
        // arrange
        var mockResult = new ValidationResult(new List<ValidationFailure> { new ValidationFailure("someProp", "someMessage") });
        var sut = new MockValidatableObject();
        MockValidatableObjectValidator.Instance.SetupMockResult(mockResult);

        // act
        var isValid = sut.IsValid(out var _);

        // assert
        isValid.Should().BeFalse();
    }

    [Fact]
    public void IsValid_Should_Return_True_Based_On_ValidationResult_IsValid_Property()
    {
        // arrange
        var mockResult = new ValidationResult();
        var sut = new MockValidatableObject();
        MockValidatableObjectValidator.Instance.SetupMockResult(mockResult);

        // act
        var isValid = sut.IsValid(out var _);

        // assert
        isValid.Should().BeTrue();
    }
}
