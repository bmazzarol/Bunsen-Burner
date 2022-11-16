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
    /// <typeparam name="TA">some TA</typeparam>
    /// <typeparam name="TB">some TB</typeparam>
    /// <typeparam name="TSyntax">syntax</typeparam>
    /// <returns>arranged scenario of TB</returns>
    [Pure]
    public static Scenario<TSyntax>.Arranged<TB> Select<TSyntax, TA, TB>(
        this Scenario<TSyntax>.Arranged<TA> scenario,
        Func<TA, TB> fn
    ) where TSyntax : struct, Syntax =>
        new(scenario.Name, async () => fn(await scenario.ArrangeScenario()));

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
    ) where TSyntax : struct, Syntax =>
        new(
            scenario.Name,
            scenario.ArrangeScenario,
            async x => fn(await scenario.ActOnScenario(x))
        );

    /// <summary>
    /// SelectMany (Bind) for Scenario.Arrange
    /// </summary>
    /// <param name="scenario">arranged scenario of TA</param>
    /// <param name="fn">fn from TA to arranged scenario of TB</param>
    /// <typeparam name="TA">some TA</typeparam>
    /// <typeparam name="TB">some TB</typeparam>
    /// <typeparam name="TSyntax">syntax</typeparam>
    /// <returns>arranged scenario of TB</returns>
    [Pure]
    public static Scenario<TSyntax>.Arranged<TB> SelectMany<TSyntax, TA, TB>(
        this Scenario<TSyntax>.Arranged<TA> scenario,
        Func<TA, Scenario<TSyntax>.Arranged<TB>> fn
    ) where TSyntax : struct, Syntax =>
        new(
            scenario.Name,
            async () => await fn(await scenario.ArrangeScenario()).ArrangeScenario()
        );

    /// <summary>
    /// SelectMany (Bind) for Scenario.Arrange
    /// </summary>
    /// <param name="scenario">arranged scenario of TA</param>
    /// <param name="mapFn">fn from TA to arranged scenario of TB</param>
    /// <param name="projectFn">projection function from TA, TB to TC</param>
    /// <typeparam name="TA">some TA</typeparam>
    /// <typeparam name="TB">some TB</typeparam>
    /// <typeparam name="TC">some TC</typeparam>
    /// <typeparam name="TSyntax">syntax</typeparam>
    /// <returns>arranged scenario of TC</returns>
    [Pure]
    public static Scenario<TSyntax>.Arranged<TC> SelectMany<TSyntax, TA, TB, TC>(
        this Scenario<TSyntax>.Arranged<TA> scenario,
        Func<TA, Scenario<TSyntax>.Arranged<TB>> mapFn,
        Func<TA, TB, TC> projectFn
    ) where TSyntax : struct, Syntax =>
        new(
            scenario.Name,
            async () =>
            {
                var ta = await scenario.ArrangeScenario();
                var tb = await mapFn(ta).ArrangeScenario();
                return projectFn(ta, tb);
            }
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
    ) where TSyntax : struct, Syntax =>
        new(
            scenario.Name,
            scenario.ArrangeScenario,
            async x => await fn(await scenario.ActOnScenario(x)).ActOnScenario(x)
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
    ) where TSyntax : struct, Syntax =>
        new(
            scenario.Name,
            scenario.ArrangeScenario,
            async x =>
            {
                var ta = await scenario.ActOnScenario(x);
                var tb = await mapFn(ta).ActOnScenario(x);
                return projectFn(ta, tb);
            }
        );
}
