namespace SynonymLookup.Database;

public interface IWordReader
{
    string GetSynonymGroupId(string word);

    IReadOnlyCollection<string> GetSynonymGroup(string groupId);
}
