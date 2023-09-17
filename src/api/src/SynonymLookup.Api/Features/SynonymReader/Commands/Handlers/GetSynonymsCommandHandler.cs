using MediatR;
using Microsoft.IdentityModel.Tokens;
using ResultNet;
using SynonymLookup.Database;

namespace SynonymLookup.Api.Features.SynonymReader.Commands.Handlers;

internal class GetSynonymsCommandHandler :
    IRequestHandler<GetSynonymsCommand, Result<IReadOnlyCollection<string>>>
{
    private readonly IWordReader reader;

    public GetSynonymsCommandHandler(IWordReader reader)
    {
        this.reader = reader;
    }

    public Task<Result<IReadOnlyCollection<string>>> Handle(
        GetSynonymsCommand request, CancellationToken cancellationToken)
    {
        var groupId = reader.GetSynonymGroupId(request.Word.Value);
        if (string.IsNullOrEmpty(groupId))
        {
            return Task.FromResult(
                Result.Failure<IReadOnlyCollection<string>>().WithCode(nameof(Errors.SL101)));
        }

        var synonyms = reader.GetSynonymGroup(groupId);
        if (synonyms.IsNullOrEmpty())
        {
            return Task.FromResult(
                Result.Failure<IReadOnlyCollection<string>>().WithCode(nameof(Errors.SL102)));
        }

        synonyms = synonyms
            .Where(synonym => !string.Equals(synonym, request.Word.Value, StringComparison.OrdinalIgnoreCase))
            .ToList();

        return Task.FromResult(Result.Success(synonyms));
    }
}
