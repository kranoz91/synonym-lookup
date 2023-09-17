namespace SynonymLookup.Api.Features.SynonymWriter.Models;

public record CreateWordDTO
{
    public required string Word { get; init; }

    public required string[] Synonyms { get; init; }
}
