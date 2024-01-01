namespace BunsenBurner;

/// <summary>
/// Adds linq support to the arrange part of a scenario
/// </summary>
public static class Scenario
{
    /// <summary>
    /// Select (Map) for Scenario.Arrange
    /// </summary>
    /// <param name="scenario">arranged scenario of TA</param>
    /// <param name="fn">fn from TA to TB</param>
    /// <typeparam name="TSyntax">syntax</typeparam>
    /// <typeparam name="TA">some TA</typeparam>
    /// <typeparam name="TB">some TB</typeparam>
    /// <returns>arranged scenario of TB</returns>
    [Pure]
    public static Scenario<TSyntax>.Arranged<TB> Select<TSyntax, TA, TB>(
        this Scenario<TSyntax>.Arranged<TA> scenario,
        Func<TA, TB> fn
    )
        where TSyntax : struct, Syntax =>
        new(scenario.Name, async () => fn(await scenario.ArrangeScenario()), scenario.Disposables);

    /// <summary>
    /// Select (Map) for Scenario.Act
    /// </summary>
    /// <param name="scenario">acted scenario of TA</param>
    /// <param name="fn">fn from TA to TB</param>
    /// <typeparam name="TSyntax">syntax</typeparam>
    /// <typeparam name="TData">arranged data</typeparam>
    /// <typeparam name="TA">some TA</typeparam>
    /// <typeparam name="TB">some TB</typeparam>
    /// <returns>acted scenario of TB</returns>
    [Pure]
    public static Scenario<TSyntax>.Acted<TData, TB> Select<TSyntax, TData, TA, TB>(
        this Scenario<TSyntax>.Acted<TData, TA> scenario,
        Func<TA, TB> fn
    )
        where TSyntax : struct, Syntax =>
        new(
            scenario.Name,
            scenario.ArrangeScenario,
            async x => fn(await scenario.ActOnScenario(x)),
            scenario.Disposables
        );

    /// <summary>
    /// SelectMany (Bind) for Scenario.Arrange
    /// </summary>
    /// <param name="scenario">arranged scenario of TA</param>
    /// <param name="fn">fn from TA to arranged scenario of TB</param>
    /// <typeparam name="TSyntax">syntax</typeparam>
    /// <typeparam name="TA">some TA</typeparam>
    /// <typeparam name="TB">some TB</typeparam>
    /// <returns>arranged scenario of TB</returns>
    [Pure]
    public static Scenario<TSyntax>.Arranged<TB> SelectMany<TSyntax, TA, TB>(
        this Scenario<TSyntax>.Arranged<TA> scenario,
        Func<TA, Scenario<TSyntax>.Arranged<TB>> fn
    )
        where TSyntax : struct, Syntax =>
        new(
            scenario.Name,
            async () => await fn(await scenario.ArrangeScenario()).ArrangeScenario(),
            scenario.Disposables
        );

    /// <summary>
    /// SelectMany (Bind) for Scenario.Arrange
    /// </summary>
    /// <param name="scenario">arranged scenario of TA</param>
    /// <param name="mapFn">fn from TA to arranged scenario of TB</param>
    /// <param name="projectFn">projection function from TA, TB to TC</param>
    /// <typeparam name="TSyntax">syntax</typeparam>
    /// <typeparam name="TA">some TA</typeparam>
    /// <typeparam name="TB">some TB</typeparam>
    /// <typeparam name="TC">some TC</typeparam>
    /// <returns>arranged scenario of TC</returns>
    [Pure]
    public static Scenario<TSyntax>.Arranged<TC> SelectMany<TSyntax, TA, TB, TC>(
        this Scenario<TSyntax>.Arranged<TA> scenario,
        Func<TA, Scenario<TSyntax>.Arranged<TB>> mapFn,
        Func<TA, TB, TC> projectFn
    )
        where TSyntax : struct, Syntax =>
        new(
            scenario.Name,
            async () =>
            {
                var ta = await scenario.ArrangeScenario();
                var tb = await mapFn(ta).ArrangeScenario();
                return projectFn(ta, tb);
            },
            scenario.Disposables
        );

    /// <summary>
    /// SelectMany (Bind) for Scenario.Acted
    /// </summary>
    /// <param name="scenario">acted scenario of TA</param>
    /// <param name="fn">fn from TA to arranged scenario of TB</param>
    /// <typeparam name="TSyntax">syntax</typeparam>
    /// <typeparam name="TData">arranged data</typeparam>
    /// <typeparam name="TA">some TA</typeparam>
    /// <typeparam name="TB">some TB</typeparam>
    /// <returns>acted scenario of TB</returns>
    [Pure]
    public static Scenario<TSyntax>.Acted<TData, TB> SelectMany<TSyntax, TData, TA, TB>(
        this Scenario<TSyntax>.Acted<TData, TA> scenario,
        Func<TA, Scenario<TSyntax>.Acted<TData, TB>> fn
    )
        where TSyntax : struct, Syntax =>
        new(
            scenario.Name,
            scenario.ArrangeScenario,
            async x => await fn(await scenario.ActOnScenario(x)).ActOnScenario(x),
            scenario.Disposables
        );

    /// <summary>
    /// SelectMany (Bind) for Scenario.Acted
    /// </summary>
    /// <param name="scenario">acted scenario of TA</param>
    /// <param name="mapFn">fn from TA to arranged scenario of TB</param>
    /// <param name="projectFn">projection function from TA, TB to TC</param>
    /// <typeparam name="TSyntax">syntax</typeparam>
    /// <typeparam name="TData">arranged data</typeparam>
    /// <typeparam name="TA">some TA</typeparam>
    /// <typeparam name="TB">some TB</typeparam>
    /// <typeparam name="TC">some TC</typeparam>
    /// <returns>acted scenario of TC</returns>
    [Pure]
    public static Scenario<TSyntax>.Acted<TData, TC> SelectMany<TSyntax, TData, TA, TB, TC>(
        this Scenario<TSyntax>.Acted<TData, TA> scenario,
        Func<TA, Scenario<TSyntax>.Acted<TData, TB>> mapFn,
        Func<TA, TB, TC> projectFn
    )
        where TSyntax : struct, Syntax =>
        new(
            scenario.Name,
            scenario.ArrangeScenario,
            async x =>
            {
                var ta = await scenario.ActOnScenario(x);
                var tb = await mapFn(ta).ActOnScenario(x);
                return projectFn(ta, tb);
            },
            scenario.Disposables
        );

    /// <summary>
    /// Converts n number of arranged scenarios to a single scenario
    /// </summary>
    /// <param name="scenarios">n number of arranged scenarios</param>
    /// <typeparam name="TSyntax">syntax</typeparam>
    /// <typeparam name="TData">arranged data</typeparam>
    /// <returns>single arranged scenario</returns>
    public static Scenario<TSyntax>.Arranged<IEnumerable<TData>> Sequence<TSyntax, TData>(
        this IEnumerable<Scenario<TSyntax>.Arranged<TData>> scenarios
    )
        where TSyntax : struct, Syntax =>
        new(
            name: null,
            async () => await Task.WhenAll(scenarios.Select(x => x.ArrangeScenario())),
            new HashSet<IDisposable>()
        );

    /// <summary>
    /// Converts n number of scenarios to a single scenario
    /// </summary>
    /// <param name="scenarios">n number of scenarios</param>
    /// <typeparam name="TSyntax">syntax</typeparam>
    /// <typeparam name="TData">arranged data</typeparam>
    /// <typeparam name="TResult">result of acting</typeparam>
    /// <returns>single asserted scenario</returns>
    public static Scenario<TSyntax>.Asserted<IEnumerable<TData>, IEnumerable<TResult>> Sequence<
        TSyntax,
        TData,
        TResult
    >(this IEnumerable<Scenario<TSyntax>.Asserted<TData, TResult>> scenarios)
        where TSyntax : struct, Syntax =>
        new(
            name: null,
            async () => await Task.WhenAll(scenarios.Select(x => x.ArrangeScenario())),
            async data => await Task.WhenAll(scenarios.Zip(data, (s, d) => s.ActOnScenario(d))),
            async (context, result) =>
                await Task.WhenAll(
                    scenarios.Zip(
                        context.Zip(result, (d, r) => (d, r)),
                        (s, t) => s.AssertAgainstResult(t.d, t.r)
                    )
                ),
            new HashSet<IDisposable>()
        );
}
