using MediatR;
using ResultNet;
using SynonymLookup.Api.Features.SynonymWriter.Models;

namespace SynonymLookup.Api.Features.SynonymWriter.Commands;

public record CreateWordCommand(Word Word, params Word[] Synonyms)
    : IRequest<Result<string>>;

