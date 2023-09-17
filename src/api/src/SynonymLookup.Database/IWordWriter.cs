using ResultNet;

namespace SynonymLookup.Database;

public interface IWordWriter
{
    Result<string> CreateNewGroup(IEnumerable<string> words, IEnumerable<string> oldGroupIds);
}
