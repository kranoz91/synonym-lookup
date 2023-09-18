using MediatR;
using ResultNet;
using SynonymLookup.Database;

namespace SynonymLookup.Api.Features.SynonymWriter.Commands.Handlers;

internal class CreateWordCommandHandler :
    IRequestHandler<CreateWordCommand, Result<string>>
{
    private readonly IWordReader reader;
    private readonly IWordWriter writer;

    public CreateWordCommandHandler(IWordReader reader, IWordWriter writer)
    {
        this.reader = reader;
        this.writer = writer;
    }

    public Task<Result<string>> Handle(
        CreateWordCommand request, CancellationToken cancellationToken)
    {
        var newWords = new List<string>();
        var groupIds = new List<string>();

        var wordGroupId = reader.GetSynonymGroupId(request.Word.Value);
        if (string.IsNullOrEmpty(wordGroupId))
        {
            newWords.Add(request.Word.Value);
        }
        else
        {
            groupIds.Add(wordGroupId);
        }

        foreach (var synonym in request.Synonyms)
        {
            var synonymGroupId = reader.GetSynonymGroupId(synonym.Value);
            if (string.IsNullOrEmpty(synonymGroupId))
            {
                newWords.Add(synonym.Value);
            }
            else
            {
                groupIds.Add(synonymGroupId);
            }
        }

        var distinctGroupIds = groupIds.Distinct(StringComparer.OrdinalIgnoreCase);
        var newGroup = new List<string>(newWords);

        foreach (var groupId in distinctGroupIds)
        {
            newGroup.AddRange(reader.GetSynonymGroup(groupId));
        }

        var result = writer.CreateNewGroup(newGroup.Distinct(StringComparer.OrdinalIgnoreCase), distinctGroupIds);

        return Task.FromResult(result);
    }
}
