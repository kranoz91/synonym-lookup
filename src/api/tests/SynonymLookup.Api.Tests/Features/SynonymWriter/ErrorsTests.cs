using FluentAssertions;
using FluentValidation.Results;
using Microsoft.AspNetCore.Http;
using Microsoft.AspNetCore.Mvc;
using Microsoft.Extensions.DependencyInjection;
using ResultNet;
using SynonymLookup.Api.Features.SynonymWriter;
using System.Text.Json;

namespace SynonymLookup.Api.Tests.Features.SynonymWriter;

public class ErrorsTests
{
    private static HttpContext CreateMockHttpContext() =>
        new DefaultHttpContext
        {
            // RequestServices needs to be set so the IResult implementation can log.
            RequestServices = new ServiceCollection().AddLogging().BuildServiceProvider(),
            Response =
            {
                // The default response body is Stream.Null which throws away anything that is written to it.
                Body = new MemoryStream(),
            },
        };

    [Fact]
    public void PredefinedErrors_Should_Contain_Expected_Errors()
    {
        // arrange
        var errorCodes = new string[] { "SL201", "SL202", "SL203" };

        // act
        var errors = Errors.PredefinedErrors;

        // assert
        errors.Count().Should().Be(errorCodes.Count());

        foreach (var error in errors)
        {
            errorCodes.Should().ContainMatch(error.Key);
        }
    }

    [Fact]
    public async void ResolveError_From_Result_Should_Return_Fallback_If_Code_Not_Found()
    {
        // arrange
        var result = Result.Failure().WithCode("NotSupportedCode");
        var mockHttpContext = CreateMockHttpContext();

        // act
        var mappedResult = Errors.ResolveError(result);
        await mappedResult.ExecuteAsync(mockHttpContext);

        // assert
        mockHttpContext.Response.Body.Position = 0;
        var jsonOptions = new JsonSerializerOptions(JsonSerializerDefaults.Web);
        var problemDetails = await JsonSerializer.DeserializeAsync<ProblemDetails>(mockHttpContext.Response.Body, jsonOptions);

        problemDetails.Status.Should().Be(500);
        problemDetails.Extensions[PredefinedErrorRegister.ErrorCodePropertyName].ToString().Should().Be(nameof(PredefinedErrorRegister.SL999));
        problemDetails.Title.Should().Be(PredefinedErrorRegister.SL999);
    }

    [Fact]
    public async void ResolveError_From_ValidationResult_Should_Return_Fallback_If_Code_Not_Found()
    {
        // arrange
        var result = new ValidationResult(
            new List<ValidationFailure>
            {
                new ValidationFailure()
                {
                    ErrorCode = "NotSupportedCode"
                }
            });
        var mockHttpContext = CreateMockHttpContext();

        // act
        var mappedResult = Errors.ResolveError(result);
        await mappedResult.ExecuteAsync(mockHttpContext);

        // assert
        mockHttpContext.Response.Body.Position = 0;
        var jsonOptions = new JsonSerializerOptions(JsonSerializerDefaults.Web);
        var problemDetails = await JsonSerializer.DeserializeAsync<ProblemDetails>(mockHttpContext.Response.Body, jsonOptions);

        problemDetails.Status.Should().Be(500);
        problemDetails.Extensions[PredefinedErrorRegister.ErrorCodePropertyName].ToString().Should().Be(nameof(PredefinedErrorRegister.SL999));
        problemDetails.Title.Should().Be(PredefinedErrorRegister.SL999);
    }
}
