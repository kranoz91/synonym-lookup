using FluentAssertions;
using SynonymLookup.Api.Extensions;
using SynonymLookup.Api.Features.SynonymWriter;
using SynonymLookup.Api.Features.SynonymWriter.Models;

namespace SynonymLookup.Api.Tests.Features.SynonymWriter.Validators;

public class WordValidatorTests
{
    [Fact]
    public void Value_Should_Not_Be_Empty()
    {
        // arrange
        var word = new Word(string.Empty);

        // act
        var isValid = word.IsValid(out var result);

        // assert
        isValid.Should().BeFalse();
        result.Errors.Should().ContainSingle();
        result.Errors.Single().ErrorCode.Should().Be(nameof(Errors.SL201));
    }

    [Fact]
    public void Value_Should_Not_Exceed_Max_Length()
    {
        // arrange
        var longWord = "";
        for (int i = 0; i < 66; i++)
        {
            longWord += "a";
        }
        var word = new Word(longWord);

        // act
        var isValid = word.IsValid(out var result);

        // assert
        isValid.Should().BeFalse();
        result.Errors.Should().ContainSingle();
        result.Errors.Single().ErrorCode.Should().Be(nameof(Errors.SL202));
    }

    [Fact]
    public void Value_Should_Not_Be_In_Unsupported_Format()
    {
        // arrange
        var word = new Word(".../asdm!!");

        // act
        var isValid = word.IsValid(out var result);

        // assert
        isValid.Should().BeFalse();
        result.Errors.Should().ContainSingle();
        result.Errors.Single().ErrorCode.Should().Be(nameof(Errors.SL203));
    }

    [Fact]
    public void Value_Should_Be_Valid_For_Expected_Allowed_Format()
    {
        // arrange
        var word = new Word("abcdefghijklmnopqrstuvwxyzåäöABCDEGHIJKLMNOPQRSTUVWXYZÅÄÖ");

        // act
        var isValid = word.IsValid(out var _);

        // assert
        isValid.Should().BeTrue();
    }
}
