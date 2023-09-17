namespace SynonymLookup.Api.Extensions;

internal static class DictionaryExtensions
{
    /// <summary>
    /// Add Dictionary of type (string, T) to another Dictionary of type (string, T).
    /// </summary>
    /// <typeparam name="T">Generic object.</typeparam>
    /// <param name="first">Target Dictionary.</param>
    /// <param name="second">Dictionary to add.</param>
    /// <exception cref="ArgumentException">Throws ArgumentException if key collides with existing key.</exception>
    /// <exception cref="ArgumentNullException">Throws ArgumentNullException if second is null.</exception>
    /// <returns>New combined Dictionary.</returns>
    internal static IDictionary<string, T> AddRange<T>(this IDictionary<string, T> first, IDictionary<string, T> second)
    {
        if (second == null)
        {
            throw new ArgumentNullException(nameof(second));
        }

        foreach (var item in second)
        {
            first.Add(item.Key, item.Value);
        }

        return first;
    }
}
