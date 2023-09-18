using FluentAssertions;
using Microsoft.AspNetCore.Http;
using Microsoft.AspNetCore.Mvc;
using SynonymLookup.Api.Features.SynonymReader;
using SynonymLookup.Api.Tests.Helpers;
using SynonymLookup.Database;
using System.Net.Http.Json;

namespace SynonymLookup.Api.Tests.Features.SynonymReader;

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
	public async Task GetSynonyms_Should_Return_Expected_Synonyms()
	{
		// arrange
		var word = "A";
		var group = new List<string>
		{
			word,
			"B",
			"C"
		};

		var writer = (IWordWriter)factory.Services.GetService(typeof(IWordWriter));
		writer.CreateNewGroup(group, Enumerable.Empty<string>());

		// act
		var response = await httpClient.GetAsync($"/v1/words/{word}/synonyms");

		// assert
		var result = await response.Content.ReadFromJsonAsync<IEnumerable<string>>();
		result.Should().Equal("B", "C");
	}

	[Fact]
	public async Task Word_Without_Group_Should_Return_Expected_Response()
	{
		// act
		var response = await httpClient.GetAsync($"/v1/words/ThisWordDoesNotExist/synonyms");

		// assert
		var result = await response.Content.ReadFromJsonAsync<ProblemDetails>();
		result.Status.Should().Be(404);
		result.Extensions[PredefinedErrorRegister.ErrorCodePropertyName].ToString().Should().Be(nameof(Errors.SL101));
		result.Title.Should().Be(Errors.SL101);
		result.Type.Should().Be(PredefinedErrorRegister.NotFoundTypeDocumentationLink);
	}
}
