using ResultNet;

namespace SynonymLookup.Database;

internal class WordDatabase : IWordReader, IWordWriter
{
    private readonly Dictionary<string, string> wordToGroupMappingTable = new(StringComparer.OrdinalIgnoreCase);
    private readonly Dictionary<string, string[]> synonymGroupTable = new(StringComparer.OrdinalIgnoreCase);

    public Result<string> CreateNewGroup(IEnumerable<string> words, IEnumerable<string> oldGroupIds)
    {
        var groupId = Guid.NewGuid().ToString();

        foreach (var word in words)
        {
            if (wordToGroupMappingTable.ContainsKey(word))
            {
                wordToGroupMappingTable[word] = groupId;
            }
            else
            {
                wordToGroupMappingTable.Add(word, groupId);
            }
        }

        synonymGroupTable.Add(groupId, words.ToArray());

        foreach (var oldGroupId in oldGroupIds)
        {
            synonymGroupTable.Remove(oldGroupId);
        }

        return Result.Success<string>(groupId);
    }

    public IReadOnlyCollection<string> GetSynonymGroup(string groupId) =>
        synonymGroupTable.GetValueOrDefault(groupId) ?? Array.Empty<string>();

    public string GetSynonymGroupId(string word) =>
        wordToGroupMappingTable.GetValueOrDefault(word) ?? string.Empty;
}