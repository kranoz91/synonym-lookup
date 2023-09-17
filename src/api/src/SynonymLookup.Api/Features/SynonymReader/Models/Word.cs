namespace SynonymLookup.Api.Features.SynonymReader.Models;

public record Word
{
	public Word(string value)
	{
		Value = value;
	}

	public string Value { get; init; }
}
