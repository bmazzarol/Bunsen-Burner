using System.Collections;
using System.Diagnostics.CodeAnalysis;
using LanguageExt.ClassInstances;
using LanguageExt.TypeClasses;

namespace BunsenBurner.Http;

using InternalHeader = Set<OrdStringOrdinal, string>;

/// <summary>
/// Models a single header in and request/response
/// </summary>
public sealed record Header : IEnumerable<string>
{
    /// <summary>
    /// Header name
    /// </summary>
    public string Name { get; }
    private InternalHeader Values { get; init; }

    /// <inheritdoc />
    public IEnumerator<string> GetEnumerator() => Values.OrderBy(_ => _).GetEnumerator();

    [ExcludeFromCodeCoverage]
    IEnumerator IEnumerable.GetEnumerator() => GetEnumerator();

    /// <summary>
    /// Returns the string value of the header
    /// </summary>
    public string Value => string.Join(",", this);

    private Header(string name, InternalHeader values)
    {
        Name = name;
        Values = values;
    }

    /// <summary>
    /// Creates a new header
    /// </summary>
    /// <param name="name">name</param>
    /// <param name="values">values to add</param>
    /// <returns>header</returns>
    public static Header New(string name, params string[] values) =>
        new(name, InternalHeader.Empty.TryAddRange(values));

    /// <summary>
    /// Adds a value to the header and returns a new header
    /// </summary>
    /// <param name="value">value to add</param>
    /// <returns>new header with value added</returns>
    public Header WithValue(string value) => this with { Values = Values.TryAdd(value) };

    /// <summary>
    /// Adds a set of values to the header and returns a new header
    /// </summary>
    /// <param name="value">value to add</param>
    /// <returns>new header with value added</returns>
    public Header WithValues(params string[] value) =>
        this with
        {
            Values = Values.TryAddRange(value)
        };

    /// <summary>
    /// Clears the header of its values
    /// </summary>
    /// <returns>header with no values</returns>
    public Header Clear() => this with { Values = InternalHeader.Empty };

    /// <summary>
    /// Ord type class for header
    /// </summary>
    [ExcludeFromCodeCoverage]
    public readonly struct OrdHeader : Ord<Header>
    {
        /// <inheritdoc />
        public Task<int> GetHashCodeAsync(Header x) => Task.FromResult(x.GetHashCode());

        /// <inheritdoc />
        public int GetHashCode(Header x) => x.GetHashCode();

        /// <inheritdoc />
        public Task<bool> EqualsAsync(Header x, Header y) => Task.FromResult(x == y);

        /// <inheritdoc />
        public bool Equals(Header x, Header y) => x == y;

        /// <inheritdoc />
        public Task<int> CompareAsync(Header x, Header y) => Task.FromResult(Compare(x, y));

        /// <inheritdoc />
        public int Compare(Header x, Header y) => RecordType<Header>.Compare(x, y);
    }
}
