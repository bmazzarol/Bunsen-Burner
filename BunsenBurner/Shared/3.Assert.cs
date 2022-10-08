using System.Linq.Expressions;

namespace BunsenBurner;

/// <summary>
/// Shared builder function implementations for assert steps
/// </summary>
internal static partial class Shared
{
    private static void RunExpressionAssertion<TResult>(
        this TResult result,
        Expression<Func<TResult, bool>> expression
    )
    {
        var fn = expression.Compile();
        if (!fn(result))
            throw new InvalidOperationException(
                $"{expression} is not true for the result {result}"
            );
    }

    private static void RunExpressionAssertion<TData, TResult>(
        TData data,
        TResult result,
        Expression<Func<TData, TResult, bool>> expression
    )
    {
        var fn = expression.Compile();
        if (!fn(data, result))
            throw new InvalidOperationException(
                $"{expression} is not true for the result {result} and data {data}"
            );
    }

    [Pure]
    internal static Scenario<TSyntax>.Asserted<TData, TResult> Assert<TData, TResult, TSyntax>(
        this Scenario<TSyntax>.Acted<TData, TResult> scenario,
        Func<TData, TResult, Task> fn
    ) where TSyntax : struct, Syntax =>
        new(scenario.Name, scenario.ArrangeScenario, scenario.ActOnScenario, fn);

    [Pure]
    internal static Scenario<TSyntax>.Asserted<TData, TResult> Assert<TData, TResult, TSyntax>(
        this Scenario<TSyntax>.Acted<TData, TResult> scenario,
        Func<TResult, Task> fn
    ) where TSyntax : struct, Syntax => scenario.Assert((_, r) => fn(r));

    [Pure]
    internal static Scenario<TSyntax>.Asserted<TData, TResult> Assert<TData, TResult, TSyntax>(
        this Scenario<TSyntax>.Acted<TData, TResult> scenario,
        Action<TData, TResult> fn
    ) where TSyntax : struct, Syntax =>
        scenario.Assert(
            (d, r) =>
            {
                fn(d, r);
                return Task.CompletedTask;
            }
        );

    [Pure]
    internal static Scenario<TSyntax>.Asserted<TData, TResult> Assert<TData, TResult, TSyntax>(
        this Scenario<TSyntax>.Acted<TData, TResult> scenario,
        Action<TResult> fn
    ) where TSyntax : struct, Syntax => scenario.Assert((_, r) => fn(r));

    [Pure]
    internal static Scenario<TSyntax>.Asserted<TData, TResult> Assert<TData, TResult, TSyntax>(
        this Scenario<TSyntax>.Acted<TData, TResult> scenario,
        Expression<Func<TResult, bool>> expression
    ) where TSyntax : struct, Syntax => scenario.Assert(r => r.RunExpressionAssertion(expression));

    [Pure]
    internal static Scenario<TSyntax>.Asserted<TData, TResult> Assert<TData, TResult, TSyntax>(
        this Scenario<TSyntax>.Acted<TData, TResult> scenario,
        Expression<Func<TData, TResult, bool>> expression
    ) where TSyntax : struct, Syntax =>
        scenario.Assert((d, r) => RunExpressionAssertion(d, r, expression));

    [Pure]
    internal static Scenario<TSyntax>.Asserted<TData, TException> AssertFailsWith<
        TData,
        TResult,
        TException,
        TSyntax
    >(this Scenario<TSyntax>.Acted<TData, TResult> scenario, Func<TData, TException, Task> fn)
        where TException : Exception
        where TSyntax : struct, Syntax =>
        new(
            scenario.Name,
            scenario.ArrangeScenario,
            async data =>
            {
                try
                {
                    await scenario.ActOnScenario(data);
                    throw new NoFailureException();
                }
                // ReSharper disable once RedundantCatchClause
                catch (NoFailureException)
                {
                    throw;
                }
                catch (TException e)
                {
                    return e;
                }
            },
            fn
        );

    [Pure]
    internal static Scenario<TSyntax>.Asserted<TData, TException> AssertFailsWith<
        TData,
        TResult,
        TException,
        TSyntax
    >(this Scenario<TSyntax>.Acted<TData, TResult> scenario, Func<TException, Task> fn)
        where TException : Exception
        where TSyntax : struct, Syntax =>
        scenario.AssertFailsWith<TData, TResult, TException, TSyntax>((_, e) => fn(e));

    [Pure]
    internal static Scenario<TSyntax>.Asserted<TData, TException> AssertFailsWith<
        TData,
        TResult,
        TException,
        TSyntax
    >(this Scenario<TSyntax>.Acted<TData, TResult> scenario, Action<TData, TException> fn)
        where TException : Exception
        where TSyntax : struct, Syntax =>
        scenario.AssertFailsWith<TData, TResult, TException, TSyntax>(
            (d, e) =>
            {
                fn(d, e);
                return Task.CompletedTask;
            }
        );

    [Pure]
    internal static Scenario<TSyntax>.Asserted<TData, TException> AssertFailsWith<
        TData,
        TResult,
        TException,
        TSyntax
    >(this Scenario<TSyntax>.Acted<TData, TResult> scenario, Action<TException> fn)
        where TException : Exception
        where TSyntax : struct, Syntax =>
        scenario.AssertFailsWith<TData, TResult, TException, TSyntax>((_, e) => fn(e));

    [Pure]
    internal static Scenario<TSyntax>.Asserted<TData, TException> AssertFailsWith<
        TData,
        TResult,
        TException,
        TSyntax
    >(
        this Scenario<TSyntax>.Acted<TData, TResult> scenario,
        Expression<Func<TData, TException, bool>> fn
    )
        where TException : Exception
        where TSyntax : struct, Syntax =>
        scenario.AssertFailsWith<TData, TResult, TException, TSyntax>(
            (d, e) => RunExpressionAssertion(d, e, fn)
        );

    [Pure]
    internal static Scenario<TSyntax>.Asserted<TData, TException> AssertFailsWith<
        TData,
        TResult,
        TException,
        TSyntax
    >(this Scenario<TSyntax>.Acted<TData, TResult> scenario, Expression<Func<TException, bool>> fn)
        where TException : Exception
        where TSyntax : struct, Syntax =>
        scenario.AssertFailsWith<TData, TResult, TException, TSyntax>(
            (_, e) => RunExpressionAssertion(e, fn)
        );

    [Pure]
    internal static Scenario<TSyntax>.Asserted<TData, TResult> And<TData, TResult, TSyntax>(
        this Scenario<TSyntax>.Asserted<TData, TResult> scenario,
        Func<TData, TResult, Task> fn
    ) where TSyntax : struct, Syntax =>
        new(
            scenario.Name,
            scenario.ArrangeScenario,
            scenario.ActOnScenario,
            async (data, result) =>
            {
                await scenario.AssertAgainstResult(data, result);
                await fn(data, result);
            }
        );

    [Pure]
    internal static Scenario<TSyntax>.Asserted<TData, TResult> And<TData, TResult, TSyntax>(
        this Scenario<TSyntax>.Asserted<TData, TResult> scenario,
        Func<TResult, Task> fn
    ) where TSyntax : struct, Syntax => scenario.And((_, r) => fn(r));

    [Pure]
    internal static Scenario<TSyntax>.Asserted<TData, TResult> And<TData, TResult, TSyntax>(
        this Scenario<TSyntax>.Asserted<TData, TResult> scenario,
        Action<TData, TResult> fn
    ) where TSyntax : struct, Syntax =>
        scenario.And(
            (d, r) =>
            {
                fn(d, r);
                return Task.CompletedTask;
            }
        );

    [Pure]
    internal static Scenario<TSyntax>.Asserted<TData, TResult> And<TData, TResult, TSyntax>(
        this Scenario<TSyntax>.Asserted<TData, TResult> scenario,
        Action<TResult> fn
    ) where TSyntax : struct, Syntax => scenario.And((_, r) => fn(r));

    [Pure]
    internal static Scenario<TSyntax>.Asserted<TData, TResult> And<TData, TResult, TSyntax>(
        this Scenario<TSyntax>.Asserted<TData, TResult> scenario,
        Expression<Func<TResult, bool>> expression
    ) where TSyntax : struct, Syntax => scenario.And(r => r.RunExpressionAssertion(expression));

    [Pure]
    internal static Scenario<TSyntax>.Asserted<TData, TResult> And<TData, TResult, TSyntax>(
        this Scenario<TSyntax>.Asserted<TData, TResult> scenario,
        Expression<Func<TData, TResult, bool>> expression
    ) where TSyntax : struct, Syntax =>
        scenario.And((d, r) => RunExpressionAssertion(d, r, expression));
}
