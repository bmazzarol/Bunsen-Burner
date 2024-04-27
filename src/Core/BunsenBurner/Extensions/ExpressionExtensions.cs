using System.Linq.Expressions;
using BunsenBurner.Exceptions;

namespace BunsenBurner.Extensions;

internal static class ExpressionExtensions
{
    internal static void RunExpressionAssertion<TResult>(
        this Expression<Func<TResult, bool>> expression,
        TResult result
    )
    {
        var fn = expression.Compile();
        if (!fn(result))
        {
            throw new ExpressionAssertionFailureException(expression, result);
        }
    }

    internal static void RunExpressionAssertion<TData, TResult>(
        this Expression<Func<TData, TResult, bool>> expression,
        TData data,
        TResult result
    )
    {
        var fn = expression.Compile();
        if (!fn(data, result))
        {
            throw new ExpressionAssertionFailureException(expression, result, data);
        }
    }
}
