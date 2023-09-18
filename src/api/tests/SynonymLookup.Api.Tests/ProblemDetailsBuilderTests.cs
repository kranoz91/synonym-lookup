using FluentAssertions;
using Microsoft.AspNetCore.Mvc;
using System.Net;

namespace SynonymLookup.Api.Tests;

public class ProblemDetailsBuilderTests
{
    [Fact]
    public void Ctor_With_ErrorCode_Parameter_Should_Build_As_Expected()
    {
        // arrange
        var errorCode = "SomeCode";
        var expected = new ProblemDetails();
        expected.Extensions.Add(new("errorCode", errorCode));

        // act
        var result = new ProblemDetailsBuilder(errorCode).Build();

        // assert
        result.Extensions.Single().Key.Should().Be("errorCode");
        result.Extensions.Single().Value.Should().Be(errorCode);
    }

    [Fact]
    public void Ctor_With_ProblemDetails_Parameter_Should_Build_As_Expected()
    {
        // arrange
        var errorCode = "SomeCode";
        var expected = new ProblemDetails
        {
            Title = "SomeTitle",
            Status = (int)HttpStatusCode.OK,
            Type = "SomeType"
        };
        expected.Extensions.Add(new("errorCode", errorCode));

        // act
        var result = new ProblemDetailsBuilder(expected).Build();

        // assert
        result.Title.Should().Be(expected.Title);
        result.Status.Should().Be(expected.Status);
        result.Type.Should().Be(expected.Type);
        result.Extensions.Single().Key.Should().Be("errorCode");
        result.Extensions.Single().Value.Should().Be(errorCode);
    }

    [Fact]
    public void WithTitle_Should_Build_As_Expected()
    {
        // arrange
        var title = "SomeTitle";

        // act
        var builder = new ProblemDetailsBuilder("SomeError");
        builder.WithTitle(title);

        var result = builder.Build();

        // assert
        result.Title.Should().Be(title);
    }

    [Fact]
    public void WithStatusCode_Should_Build_As_Expected()
    {
        // arrange
        var statusCode = (int)HttpStatusCode.OK;

        // act
        var builder = new ProblemDetailsBuilder("SomeError");
        builder.WithStatusCode(statusCode);

        var result = builder.Build();

        // assert
        result.Status.Should().Be(statusCode);
    }

    [Fact]
    public void WithType_Should_Build_As_Expected()
    {
        // arrange
        var type = "SomeType";

        // act
        var builder = new ProblemDetailsBuilder("SomeError");
        builder.WithType(type);

        var result = builder.Build();

        // assert
        result.Type.Should().Be(type);
    }

    [Fact]
    public void Full_Based_On_Ctor_With_ErrorCode_Should_Build_As_Expected()
    {
        // arrange
        var errorCode = "SomeCode";
        var expected = new ProblemDetails
        {
            Title = "SomeTitle",
            Status = (int)HttpStatusCode.OK,
            Type = "SomeType"
        };
        expected.Extensions.Add(new("errorCode", errorCode));

        // act
        var builder = new ProblemDetailsBuilder(errorCode);
        builder.WithTitle(expected.Title);
        builder.WithStatusCode(expected.Status.Value);
        builder.WithType(expected.Type);

        var result = builder.Build();

        // assert
        result.Title.Should().Be(expected.Title);
        result.Status.Should().Be(expected.Status);
        result.Type.Should().Be(expected.Type);
        result.Extensions.Single().Key.Should().Be("errorCode");
        result.Extensions.Single().Value.Should().Be(errorCode);
    }

    [Fact]
    public void Ctor_With_ProblemDetails_WithTitle_Should_Override_Existing_Value()
    {
        // arrange
        var expectedTitle = "NewTitle";
        var input = new ProblemDetails
        {
            Title = "SomeTitle"
        };

        // act
        var builder = new ProblemDetailsBuilder(input);
        builder.WithTitle(expectedTitle);

        var result = builder.Build();

        // assert
        result.Title.Should().Be(expectedTitle);
    }

    [Fact]
    public void Ctor_With_ProblemDetails_WithStatusCode_Should_Override_Existing_Value()
    {
        // arrange
        var expectedStatus = (int)HttpStatusCode.InternalServerError;
        var input = new ProblemDetails
        {
            Status = (int)HttpStatusCode.OK
        };

        // act
        var builder = new ProblemDetailsBuilder(input);
        builder.WithStatusCode(expectedStatus);

        var result = builder.Build();

        // assert
        result.Status.Should().Be(expectedStatus);
    }

    [Fact]
    public void Ctor_With_ProblemDetails_WithType_Should_Override_Existing_Value()
    {
        // arrange
        var expectedType = "NewType";
        var input = new ProblemDetails
        {
            Type = "SomeType"
        };

        // act
        var builder = new ProblemDetailsBuilder(input);
        builder.WithType(expectedType);

        var result = builder.Build();

        // assert
        result.Type.Should().Be(expectedType);
    }
}
