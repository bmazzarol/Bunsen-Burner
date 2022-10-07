using System.Collections.Immutable;

namespace BunsenBurner.Http;

using KeyValuePair = KeyValuePair<string, string>;
using Dictionary = IImmutableDictionary<string, string>;

/// <summary>
/// Extension methods on key value
/// </summary>
public static class KeyValuePairExt
{
    /// <summary>
    /// Adds a value to key value
    /// </summary>
    /// <param name="kv">key value</param>
    /// <param name="value">extra value</param>
    /// <returns>modified key value</returns>
    public static KeyValuePair AddValue(this KeyValuePair kv, string value) =>
        new(
            kv.Key,
            string.Join(
                ",",
                (kv.Value ?? String.Empty)
                    .Split(",")
                    .Append(value)
                    .Select(x => x.Trim())
                    .Distinct(StringComparer.Ordinal)
                    .OrderBy(_ => _)
            )
        );

    /// <summary>
    /// Add or update the key value
    /// </summary>
    /// <param name="dictionary">dictionary</param>
    /// <param name="kv">key value</param>
    /// <returns>updated dictionary</returns>
    public static Dictionary AddOrUpdate(this Dictionary dictionary, KeyValuePair kv) =>
        dictionary.TryGetValue(kv.Key, out var value)
            ? dictionary.SetItem(kv.Key, kv.AddValue(value).Value)
            : dictionary.Add(kv.Key, kv.Value);

    /// <summary>
    /// Trys to get a value
    /// </summary>
    /// <param name="dictionary">dictionary</param>
    /// <param name="key">key</param>
    /// <returns>key value</returns>
    public static string? Get(this Dictionary dictionary, string key) =>
        dictionary.TryGetValue(key, out var value) ? value : default;
}
