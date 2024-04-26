﻿using System.Linq.Expressions;

namespace BunsenBurner;

internal static partial class Shared
{
    internal static void RunExpressionAssertion<TResult>(
        this TResult result,
        Expression<Func<TResult, bool>> expression
    )
    {
        var fn = expression.Compile();
        if (!fn(result))
        {
            throw new InvalidOperationException(
                $"{expression} is not true for the result {result}"
            );
        }
    }

    internal static void RunExpressionAssertion<TData, TResult>(
        this TData data,
        TResult result,
        Expression<Func<TData, TResult, bool>> expression
    )
    {
        var fn = expression.Compile();
        if (!fn(data, result))
        {
            throw new InvalidOperationException(
                $"{expression} is not true for the result {result} and data {data}"
            );
        }
    }

    [Pure]
    internal static TestBuilder<TSyntax>.Asserted<TData, TResult> Assert<TData, TResult, TSyntax>(
        this TestBuilder<TSyntax>.Acted<TData, TResult> test,
        Func<TData, TResult, Task> fn
    )
        where TSyntax : struct, Syntax => new(test.ArrangeStep, test.ActStep, fn, test.Disposables);

    [Pure]
    internal static TestBuilder<TSyntax>.Asserted<TData, TResult> Assert<TData, TResult, TSyntax>(
        this TestBuilder<TSyntax>.Acted<TData, TResult> test,
        Func<TResult, Task> fn
    )
        where TSyntax : struct, Syntax => test.Assert((_, r) => fn(r));

    [Pure]
    internal static TestBuilder<TSyntax>.Asserted<TData, TResult> Assert<TData, TResult, TSyntax>(
        this TestBuilder<TSyntax>.Acted<TData, TResult> test,
        Action<TData, TResult> fn
    )
        where TSyntax : struct, Syntax =>
        test.Assert(
            (d, r) =>
            {
                fn(d, r);
                return Task.CompletedTask;
            }
        );

    [Pure]
    internal static TestBuilder<TSyntax>.Asserted<TData, TResult> Assert<TData, TResult, TSyntax>(
        this TestBuilder<TSyntax>.Acted<TData, TResult> test,
        Action<TResult> fn
    )
        where TSyntax : struct, Syntax => test.Assert((_, r) => fn(r));

    [Pure]
    internal static TestBuilder<TSyntax>.Asserted<TData, TResult> Assert<TData, TResult, TSyntax>(
        this TestBuilder<TSyntax>.Acted<TData, TResult> test,
        Expression<Func<TResult, bool>> expression
    )
        where TSyntax : struct, Syntax => test.Assert(r => r.RunExpressionAssertion(expression));

    [Pure]
    internal static TestBuilder<TSyntax>.Asserted<TData, TResult> Assert<TData, TResult, TSyntax>(
        this TestBuilder<TSyntax>.Acted<TData, TResult> test,
        Expression<Func<TData, TResult, bool>> expression
    )
        where TSyntax : struct, Syntax =>
        test.Assert((d, r) => RunExpressionAssertion(d, r, expression));
}
