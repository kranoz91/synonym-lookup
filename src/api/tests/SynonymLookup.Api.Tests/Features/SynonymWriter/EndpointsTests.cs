using FluentAssertions;
using Microsoft.AspNetCore.Mvc;
using SynonymLookup.Api.Features.SynonymWriter;
using SynonymLookup.Api.Features.SynonymWriter.Models;
using SynonymLookup.Api.Tests.Helpers;
using System.Net;
using System.Net.Http.Json;

namespace SynonymLookup.Api.Tests.Features.SynonymWriter;

public class EndpointsTests : IClassFixture<TestWebApplicationFactory<Program>>
{
    private readonly TestWebApplicationFactory<Program> factory;
    private readonly HttpClient httpClient;

    public EndpointsTests(TestWebApplicationFactory<Program> factory)
    {
        this.factory = factory;
        httpClient = factory.CreateClient();
    }

    [Fact]
    public async Task PostWord_Should_Return_201_With_Expected_Url()
    {
        // arrange
        var input = new CreateWordDTO
        {
            Word = "abc",
            Synonyms = new string[] { "cba" }
        };

        // act
        var response = await httpClient.PostAsJsonAsync<CreateWordDTO>("/v1/words", input, CancellationToken.None);

        // assert
        response.IsSuccessStatusCode.Should().BeTrue();
        response.StatusCode.Should().Be(HttpStatusCode.Created);
        response.Headers.Location.Should().Be($"/v1/words/{input.Word}/synonyms");
    }

    [Fact]
    public async Task PostWord_Should_Return_400_With_Expected_ErrorCode_When_Word_Is_Empty()
    {
        // arrange
        var input = new CreateWordDTO
        {
            Word = "",
            Synonyms = new string[] { "cba" }
        };

        // act
        var response = await httpClient.PostAsJsonAsync<CreateWordDTO>("/v1/words", input, CancellationToken.None);

        // assert
        response.IsSuccessStatusCode.Should().BeFalse();
        response.StatusCode.Should().Be(HttpStatusCode.BadRequest);

        var result = await response.Content.ReadFromJsonAsync<ProblemDetails>();
        result.Status.Should().Be(400);
        result.Extensions[PredefinedErrorRegister.ErrorCodePropertyName].ToString().Should().Be(nameof(Errors.SL201));
        result.Title.Should().Be(Errors.SL201);
        result.Type.Should().Be(PredefinedErrorRegister.BadRequestTypeDocumentationLink);
    }

    [Fact]
    public async Task PostWord_Should_Return_400_With_Expected_ErrorCode_When_Synonym_Is_Empty()
    {
        // arrange
        var input = new CreateWordDTO
        {
            Word = "abc",
            Synonyms = new string[] { "" }
        };

        // act
        var response = await httpClient.PostAsJsonAsync<CreateWordDTO>("/v1/words", input, CancellationToken.None);

        // assert
        response.IsSuccessStatusCode.Should().BeFalse();
        response.StatusCode.Should().Be(HttpStatusCode.BadRequest);

        var result = await response.Content.ReadFromJsonAsync<ProblemDetails>();
        result.Status.Should().Be(400);
        result.Extensions[PredefinedErrorRegister.ErrorCodePropertyName].ToString().Should().Be(nameof(Errors.SL201));
        result.Title.Should().Be(Errors.SL201);
        result.Type.Should().Be(PredefinedErrorRegister.BadRequestTypeDocumentationLink);
    }

    [Fact]
    public async Task PostWord_Should_Return_400_With_Expected_ErrorCode_When_Word_Exceeds_Max_Length()
    {
        // arrange
        var longWord = "";
        for (int i = 0; i < 66; i++)
        {
            longWord += "a";
        }
        var input = new CreateWordDTO
        {
            Word = longWord,
            Synonyms = new string[] { "cba" }
        };

        // act
        var response = await httpClient.PostAsJsonAsync<CreateWordDTO>("/v1/words", input, CancellationToken.None);

        // assert
        response.IsSuccessStatusCode.Should().BeFalse();
        response.StatusCode.Should().Be(HttpStatusCode.BadRequest);

        var result = await response.Content.ReadFromJsonAsync<ProblemDetails>();
        result.Status.Should().Be(400);
        result.Extensions[PredefinedErrorRegister.ErrorCodePropertyName].ToString().Should().Be(nameof(Errors.SL202));
        result.Title.Should().Be(Errors.SL202);
        result.Type.Should().Be(PredefinedErrorRegister.BadRequestTypeDocumentationLink);
    }

    [Fact]
    public async Task PostWord_Should_Return_400_With_Expected_ErrorCode_When_Synonym_Exceeds_Max_Length()
    {
        // arrange
        var longWord = "";
        for (int i = 0; i < 66; i++)
        {
            longWord += "a";
        }
        var input = new CreateWordDTO
        {
            Word = "abc",
            Synonyms = new string[] { longWord }
        };

        // act
        var response = await httpClient.PostAsJsonAsync<CreateWordDTO>("/v1/words", input, CancellationToken.None);

        // assert
        response.IsSuccessStatusCode.Should().BeFalse();
        response.StatusCode.Should().Be(HttpStatusCode.BadRequest);

        var result = await response.Content.ReadFromJsonAsync<ProblemDetails>();
        result.Status.Should().Be(400);
        result.Extensions[PredefinedErrorRegister.ErrorCodePropertyName].ToString().Should().Be(nameof(Errors.SL202));
        result.Title.Should().Be(Errors.SL202);
        result.Type.Should().Be(PredefinedErrorRegister.BadRequestTypeDocumentationLink);
    }

    [Fact]
    public async Task PostWord_Should_Return_400_With_Expected_ErrorCode_When_Word_Is_In_Unsupported_Format()
    {
        // arrange
        var input = new CreateWordDTO
        {
            Word = "..//casd",
            Synonyms = new string[] { "cba" }
        };

        // act
        var response = await httpClient.PostAsJsonAsync<CreateWordDTO>("/v1/words", input, CancellationToken.None);

        // assert
        response.IsSuccessStatusCode.Should().BeFalse();
        response.StatusCode.Should().Be(HttpStatusCode.BadRequest);

        var result = await response.Content.ReadFromJsonAsync<ProblemDetails>();
        result.Status.Should().Be(400);
        result.Extensions[PredefinedErrorRegister.ErrorCodePropertyName].ToString().Should().Be(nameof(Errors.SL203));
        result.Title.Should().Be(Errors.SL203);
        result.Type.Should().Be(PredefinedErrorRegister.BadRequestTypeDocumentationLink);
    }

    [Fact]
    public async Task PostWord_Should_Return_400_With_Expected_ErrorCode_When_Synonym_Is_In_Unsupported_Format()
    {
        // arrange
        var input = new CreateWordDTO
        {
            Word = "abc",
            Synonyms = new string[] { "..//casd" }
        };

        // act
        var response = await httpClient.PostAsJsonAsync<CreateWordDTO>("/v1/words", input, CancellationToken.None);

        // assert
        response.IsSuccessStatusCode.Should().BeFalse();
        response.StatusCode.Should().Be(HttpStatusCode.BadRequest);

        var result = await response.Content.ReadFromJsonAsync<ProblemDetails>();
        result.Status.Should().Be(400);
        result.Extensions[PredefinedErrorRegister.ErrorCodePropertyName].ToString().Should().Be(nameof(Errors.SL203));
        result.Title.Should().Be(Errors.SL203);
        result.Type.Should().Be(PredefinedErrorRegister.BadRequestTypeDocumentationLink);
    }
}
