using FluentAssertions;
using NSubstitute;
using ResultNet;
using SynonymLookup.Api.Features.SynonymReader;
using SynonymLookup.Api.Features.SynonymReader.Commands;
using SynonymLookup.Api.Features.SynonymReader.Commands.Handlers;
using SynonymLookup.Api.Features.SynonymReader.Models;
using SynonymLookup.Database;

namespace SynonymLookup.Api.Tests.Features.SynonymReader.Commands.Handlers;

public class GetSynonymsCommandHandlerTests
{
    private readonly GetSynonymsCommandHandler sut;
    private readonly IWordReader reader;

    public GetSynonymsCommandHandlerTests()
    {
        reader = Substitute.For<IWordReader>();
        sut = new GetSynonymsCommandHandler(reader);
    }

    [Fact]
    public async Task Word_Without_Group_Should_Return_Expected_Error()
    {
        // arrange
        reader.GetSynonymGroupId(Arg.Any<string>()).Returns(string.Empty);

        // act
        var result = await sut.Handle(new GetSynonymsCommand(new Word("")), CancellationToken.None);

        // assert
        result.IsFailure().Should().BeTrue();
        result.Code.Should().Be(nameof(Errors.SL101));
    }

    [Fact]
    public async Task Word_With_Empty_Group_Should_Return_Expected_Error()
    {
        // arrange
        reader.GetSynonymGroupId(Arg.Any<string>()).Returns("groupId");
        reader.GetSynonymGroup(Arg.Any<string>()).Returns(Array.Empty<string>());

        // act
        var result = await sut.Handle(new GetSynonymsCommand(new Word("")), CancellationToken.None);

        // assert
        result.IsFailure().Should().BeTrue();
        result.Code.Should().Be(nameof(Errors.SL102));
    }

    [Fact]
    public async Task Synonyms_Should_Be_Returned_Without_Input_Word()
    {
        // arrange
        var word = "A";
        var group = new List<string>
        {
            word,
            "B",
            "C"
        };

        reader.GetSynonymGroupId(Arg.Any<string>()).Returns("groupId");
        reader.GetSynonymGroup(Arg.Any<string>()).Returns(group);

        // act
        var result = await sut.Handle(new GetSynonymsCommand(new Word(word)), CancellationToken.None);

        // assert
        result.IsSuccess().Should().BeTrue();
        result.Data.Should().Equal("B", "C");
    }
}
