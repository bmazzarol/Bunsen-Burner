using System.Collections;
using System.Diagnostics.CodeAnalysis;
using Flurl;
using Microsoft.AspNetCore.Http;
using Microsoft.Extensions.Primitives;

namespace BunsenBurner.FunctionApp.Models;

[ExcludeFromCodeCoverage]
internal static class DummyQueryCollectionExt
{
    [Pure]
    internal static StringValues ToStringValues(this object? value) =>
        value switch
        {
            string s => new StringValues(s),
            string?[] strings
                => strings.Length == 1 ? new StringValues(strings[0]) : new StringValues(strings),
#pragma warning disable S6603
            object?[] objects when objects.All(x => x != null)
#pragma warning restore S6603
                => objects.Length == 1
                    ? new StringValues(objects[0]?.ToString())
                    : new StringValues(objects.Select(x => x?.ToString()).ToArray()),
            _ => new StringValues(value?.ToString())
        };
}

[ExcludeFromCodeCoverage]
internal sealed record DummyQueryCollection : IQueryCollection
{
    private IReadOnlyDictionary<string, StringValues> Store { get; }

    public DummyQueryCollection(Url url) =>
        Store = url.QueryParams.GroupBy(x => x.Name, StringComparer.Ordinal)
            .Select(x => (x.Key, Value: x.Select(y => y.Value).ToArray().ToStringValues()))
            .ToDictionary(x => x.Key, x => x.Value, StringComparer.Ordinal);

    public StringValues this[string key] =>
        Store.TryGetValue(key, out var values) ? values : default;

    public int Count => Store.Count;

    public ICollection<string> Keys => Store.Keys.ToStringValues();

    public bool ContainsKey(string key) => Store.ContainsKey(key);

    public IEnumerator<KeyValuePair<string, StringValues>> GetEnumerator() => Store.GetEnumerator();

    public bool TryGetValue(string key, out StringValues value) =>
        Store.TryGetValue(key, out value);

    IEnumerator IEnumerable.GetEnumerator() => Store.GetEnumerator();
}
