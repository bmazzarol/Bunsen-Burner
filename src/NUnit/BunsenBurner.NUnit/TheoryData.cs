using System.Collections;

namespace BunsenBurner.NUnit;

/// <summary>
/// Simple type that mimics the TheoryData type in Xunit
/// </summary>
/// <typeparam name="T">some T</typeparam>
public readonly struct TheoryData<T> : IEnumerable<T>
{
    private readonly IEnumerable<T> _enumerable;

    internal TheoryData(IEnumerable<T> enumerable)
    {
        _enumerable = enumerable;
    }

    /// <inheritdoc />
    public IEnumerator<T> GetEnumerator() => _enumerable.GetEnumerator();

    IEnumerator IEnumerable.GetEnumerator() => GetEnumerator();
}
