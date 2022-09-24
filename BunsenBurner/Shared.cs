namespace BunsenBurner;

/// <summary>
/// Shared builder function implementations
/// </summary>
internal static class Shared
{
    [Pure]
    internal static Scenario<TSyntax>.Arranged<TData> Arrange<TData, TSyntax>(Func<Task<TData>> fn)
        where TSyntax : struct, Syntax => new(default, fn);

    [Pure]
    internal static Scenario<TSyntax>.Arranged<TData> Arrange<TData, TSyntax>(Func<TData> fn)
        where TSyntax : struct, Syntax => Arrange<TData, TSyntax>(() => Task.FromResult(fn()));

    [Pure]
    internal static Scenario<TSyntax>.Arranged<TData> Arrange<TData, TSyntax>(
        this string name,
        Func<Task<TData>> fn
    ) where TSyntax : struct, Syntax => new(name, fn);

    [Pure]
    internal static Scenario<TSyntax>.Arranged<TData> Arrange<TData, TSyntax>(
        this string name,
        Func<TData> fn
    ) where TSyntax : struct, Syntax => name.Arrange<TData, TSyntax>(() => Task.FromResult(fn()));

    [Pure]
    internal static Scenario<TSyntax>.Arranged<TDataNext> And<TData, TDataNext, TSyntax>(
        this Scenario<TSyntax>.Arranged<TData> scenario,
        Func<TData, Task<TDataNext>> fn
    ) where TSyntax : struct, Syntax =>
        new(scenario.Name, async () => await fn(await scenario.ArrangeScenario()));

    [Pure]
    internal static Scenario<TSyntax>.Arranged<TDataNext> And<TData, TDataNext, TSyntax>(
        this Scenario<TSyntax>.Arranged<TData> scenario,
        Func<TData, TDataNext> fn
    ) where TSyntax : struct, Syntax => scenario.And(x => Task.FromResult(fn(x)));

    [Pure]
    internal static Scenario<TSyntax>.Acted<TData, TResult> Act<TData, TResult, TSyntax>(
        this Scenario<TSyntax>.Arranged<TData> scenario,
        Func<TData, Task<TResult>> fn
    ) where TSyntax : struct, Syntax => new(scenario.Name, scenario.ArrangeScenario, fn);

    [Pure]
    internal static Scenario<TSyntax>.Acted<TData, TResult> Act<TData, TResult, TSyntax>(
        this Scenario<TSyntax>.Arranged<TData> scenario,
        Func<TData, TResult> fn
    ) where TSyntax : struct, Syntax => scenario.Act(x => Task.FromResult(fn(x)));

    [Pure]
    internal static Scenario<TSyntax>.Acted<TData, TResultNext> And<
        TData,
        TResult,
        TResultNext,
        TSyntax
    >(
        this Scenario<TSyntax>.Acted<TData, TResult> scenario,
        Func<TData, TResult, Task<TResultNext>> fn
    ) where TSyntax : struct, Syntax =>
        new(
            scenario.Name,
            scenario.ArrangeScenario,
            async data => await fn(data, await scenario.ActOnScenario(data))
        );

    [Pure]
    internal static Scenario<TSyntax>.Acted<TData, TResultNext> And<
        TData,
        TResult,
        TResultNext,
        TSyntax
    >(this Scenario<TSyntax>.Acted<TData, TResult> scenario, Func<TData, TResult, TResultNext> fn)
        where TSyntax : struct, Syntax => scenario.And((d, r) => Task.FromResult(fn(d, r)));

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
                    await scenario.ActOnScenario(data);
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
}
