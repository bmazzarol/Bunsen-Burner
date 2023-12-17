using System.Collections;
using System.Diagnostics.CodeAnalysis;

namespace BunsenBurner.Http;

/// <summary>
/// Models a JWT token part.
/// Either a single value or a sequence of values
/// </summary>
public readonly record struct TokenPartValue(object Value) : IEnumerable<object>
{
    /// <summary>
    /// Underlying value
    /// </summary>
    public object Value { get; } = Value;

    /// <summary>
    /// Flag that indicates the token as multiple values
    /// </summary>
    public bool HasMultipleValues { get; } = Value is not string && Value is IEnumerable;

    /// <inheritdoc />
    public IEnumerator<object> GetEnumerator()
    {
        if (HasMultipleValues)
        {
            foreach (var value in (Value as IEnumerable)!)
            {
                yield return value;
            }
        }
        else
        {
            yield return Value;
        }
    }

    [ExcludeFromCodeCoverage]
    IEnumerator IEnumerable.GetEnumerator()
    {
        return GetEnumerator();
    }

    /// <summary>
    /// Adds two <see cref="TokenPartValue"/> values
    /// </summary>
    /// <param name="first">first</param>
    /// <param name="second">second</param>
    /// <returns>combined <see cref="TokenPartValue"/></returns>
    public static TokenPartValue operator +(TokenPartValue first, TokenPartValue second)
    {
        return new TokenPartValue(first.Concat(second));
    }
}
