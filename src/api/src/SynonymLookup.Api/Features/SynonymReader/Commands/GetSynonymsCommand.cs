using MediatR;
using ResultNet;
using SynonymLookup.Api.Features.SynonymReader.Models;

namespace SynonymLookup.Api.Features.SynonymReader.Commands;

public record GetSynonymsCommand(Word Word)
    : IRequest<Result<IReadOnlyCollection<string>>>;
