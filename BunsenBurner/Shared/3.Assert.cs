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
                $"{expression.ToString()} is not true for the result {result}"
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
    internal static Scenario<TSyntax>.Asserted<TData, Exception> AssertFailsWith<
        TData,
        TResult,
        TSyntax
    >(this Scenario<TSyntax>.Acted<TData, TResult> scenario, Func<TData, Exception, Task> fn)
        where TSyntax : struct, Syntax =>
        new(
            scenario.Name,
            scenario.ArrangeScenario,
            async data =>
            {
                try
                {
                    await scenario.ActOnScenario(data).ConfigureAwait(false);
                    throw new NoFailureException();
                }
                catch (NoFailureException)
                {
                    throw;
                }
                catch (Exception e)
                {
                    return e;
                }
            },
            fn
        );

    [Pure]
    internal static Scenario<TSyntax>.Asserted<TData, Exception> AssertFailsWith<
        TData,
        TResult,
        TSyntax
    >(this Scenario<TSyntax>.Acted<TData, TResult> scenario, Func<Exception, Task> fn)
        where TSyntax : struct, Syntax => scenario.AssertFailsWith((_, e) => fn(e));

    [Pure]
    internal static Scenario<TSyntax>.Asserted<TData, Exception> AssertFailsWith<
        TData,
        TResult,
        TSyntax
    >(this Scenario<TSyntax>.Acted<TData, TResult> scenario, Action<TData, Exception> fn)
        where TSyntax : struct, Syntax =>
        scenario.AssertFailsWith(
            (d, e) =>
            {
                fn(d, e);
                return Task.CompletedTask;
            }
        );

    [Pure]
    internal static Scenario<TSyntax>.Asserted<TData, Exception> AssertFailsWith<
        TData,
        TResult,
        TSyntax
    >(this Scenario<TSyntax>.Acted<TData, TResult> scenario, Action<Exception> fn)
        where TSyntax : struct, Syntax => scenario.AssertFailsWith((_, e) => fn(e));

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
                await scenario.AssertAgainstResult(data, result).ConfigureAwait(false);
                await fn(data, result).ConfigureAwait(false);
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
}
