using FluentAssertions;
using NSubstitute;
using ResultNet;
using SynonymLookup.Api.Features.SynonymReader.Commands.Handlers;
using SynonymLookup.Api.Features.SynonymWriter.Commands;
using SynonymLookup.Api.Features.SynonymWriter.Commands.Handlers;
using SynonymLookup.Api.Features.SynonymWriter.Models;
using SynonymLookup.Database;

namespace SynonymLookup.Api.Tests.Features.SynonymWriter.Commands.Handlers;

public class CreateWordCommandHandlerTests
{
    private readonly CreateWordCommandHandler sut;
    private readonly IWordReader reader;
    private readonly IWordWriter writer;

    public CreateWordCommandHandlerTests()
    {
        reader = Substitute.For<IWordReader>();
        writer = Substitute.For<IWordWriter>();
        sut = new CreateWordCommandHandler(reader, writer);
    }

    public static IEnumerable<object[]> CreateWordCommandTests => new List<object[]>
    {
        // All new words
        new object[]
        {
            new CreateWordCommand(new Word("A"), new Word("B"), new Word("C")),
            new Dictionary<string, string>(),
            new List<string> { "A", "B", "C" },
            new List<string>()
        },
        // Words in different groups
        new object[]
        {
            new CreateWordCommand(new Word("A"), new Word("B"), new Word("C")),
            new Dictionary<string, string> { { "A", "Group1" }, { "B", "Group2" }, { "C", "Group2" } },
            new List<string> { "A", "B", "C" },
            new List<string> { "Group1", "Group2" }
        },
        // Words in different groups with new word
        new object[]
        {
            new CreateWordCommand(new Word("A"), new Word("B"), new Word("C")),
            new Dictionary<string, string> { { "A", "Group1" }, { "B", "Group2" } },
            new List<string> { "A", "B", "C" },
            new List<string> { "Group1", "Group2" }
        },
        // Transitive dependency
        new object[]
        {
            new CreateWordCommand(new Word("A"), new Word("B"), new Word("C")),
            new Dictionary<string, string> { { "A", "Group1" }, { "B", "Group2" }, { "C", "Group2" }, { "D", "Group2" } },
            new List<string> { "A", "B", "C", "D" },
            new List<string> { "Group1", "Group2" }
        },
        // Transitive dependency with new word
        new object[]
        {
            new CreateWordCommand(new Word("A"), new Word("B"), new Word("C")),
            new Dictionary<string, string> { { "A", "Group1" }, { "B", "Group2" }, { "D", "Group2" } },
            new List<string> { "A", "B", "C", "D" },
            new List<string> { "Group1", "Group2" }
        },
        // Complex test
        new object[]
        {
            new CreateWordCommand(new Word("A"), new Word("B"), new Word("c"), new Word("C"), new Word("D")),
            new Dictionary<string, string> { { "A", "Group1" }, { "B", "Group2" }, { "D", "Group3" }, { "C", "Group4" }, { "E", "Group4" }, { "F", "Group5" } },
            new List<string> { "A", "B", "C", "D", "E" },
            new List<string> { "Group1", "Group2", "Group3", "Group4" }
        }
    };

    [Theory]
    [MemberData(nameof(CreateWordCommandTests))]
    public async Task DoCreateWordCommands(
        CreateWordCommand command, Dictionary<string, string> seed, IEnumerable<string> expectedGroup, IEnumerable<string> expectedOldGroupIds)
    {
        // arrange
        var groups = new Dictionary<string, string[]>();
        foreach (var pair in seed)
        {
            reader.GetSynonymGroupId(Arg.Is(pair.Key)).Returns(pair.Value);
            if (groups.ContainsKey(pair.Value))
            {
                var temp = groups[pair.Value].ToList();
                temp.Add(pair.Key);
                groups[pair.Value] = temp.ToArray();
            }
            else
            {
                groups.Add(pair.Value, new string[] { pair.Key });
            }
        }

        foreach (var group in groups)
        {
            reader.GetSynonymGroup(Arg.Is(group.Key)).Returns(group.Value);
        }

        // act
        await sut.Handle(command, CancellationToken.None);

        // assert
        var args = writer.ReceivedCalls().Single().GetArguments();
        args[0].As<IEnumerable<string>>().Select(word => word.ToUpper()).Should().BeEquivalentTo(expectedGroup.Select(word => word.ToUpper()));
        args[1].As<IEnumerable<string>>().Select(word => word.ToUpper()).Should().BeEquivalentTo(expectedOldGroupIds.Select(word => word.ToUpper()));
    }
}
