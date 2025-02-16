using System.Diagnostics.CodeAnalysis;
using System.Linq.Expressions;

namespace BunsenBurner.Exceptions;

/// <summary>
/// Thrown when an expression assertion fails
/// </summary>
[SuppressMessage("Roslynator", "RCS1194:Implement exception constructors")]
public sealed class ExpressionAssertionFailureException : Exception, IAssertionException
{
    /// <summary>
    /// Predicate expression that failed
    /// </summary>
    public LambdaExpression Expression { get; }

    /// <summary>
    /// Data the test was run against
    /// </summary>
    public object? TestData { get; }

    /// <summary>
    /// Result the predicate was run against
    /// </summary>
    public object? Result { get; }

    internal ExpressionAssertionFailureException(
        LambdaExpression expression,
        object? result,
        object? data = null
    )
        : base(BuildMessage(expression, result, data))
    {
        Expression = expression;
        Result = result;
        TestData = data;
    }

    private static string BuildMessage(LambdaExpression expression, object? result, object? data)
    {
        return data is not null
            ? $"{expression} is not true for inputs '{data}' and '{result}'"
            : $"{expression} is not true for input '{result}'";
    }
}
