namespace BunsenBurner.BenchmarkDotNet;

using BddScenario = Scenario<Syntax.Bdd>;

/// <summary>
/// DSL for building tests using a given, when, then syntax
/// </summary>
public static class Bdd
{
    /// <summary>
    /// Given a set of performance benchmarks to be run
    /// </summary>
    /// <param name="name">name/description</param>
    /// <param name="configure">optional configuration</param>
    /// <param name="logSink">optional log message sink</param>
    /// <param name="runParameters">optional parameters</param>
    /// <typeparam name="TBenchmarks">type of benchmarks</typeparam>
    /// <returns>given scenario</returns>
    [Pure]
    public static BddScenario.Arranged<RunContext> GivenBenchmarks<TBenchmarks>(
        this string name,
        Func<ManualConfig, ManualConfig>? configure = default,
        Action<string>? logSink = default,
        params string[] runParameters
    )
        where TBenchmarks : class =>
        name.ArrangeBenchmarks<Syntax.Bdd, TBenchmarks>(configure, logSink, runParameters);

    /// <summary>
    /// Given a set of performance benchmarks to be run
    /// </summary>
    /// <param name="configure">optional configuration</param>
    /// <param name="logSink">optional log message sink</param>
    /// <param name="runParameters">optional parameters</param>
    /// <typeparam name="TBenchmarks">type of benchmarks</typeparam>
    /// <returns>given scenario</returns>
    [Pure]
    public static BddScenario.Arranged<RunContext> GivenBenchmarks<TBenchmarks>(
        Func<ManualConfig, ManualConfig>? configure = default,
        Action<string>? logSink = default,
        params string[] runParameters
    )
        where TBenchmarks : class =>
        Shared.ArrangeBenchmarks<Syntax.Bdd, TBenchmarks>(configure, logSink, runParameters);

    /// <summary>
    /// When the performance benchmarks defined by the run context are executed
    /// </summary>
    /// <param name="scenario">given scenario</param>
    /// <returns>acted scenario</returns>
    [Pure]
    public static BddScenario.Acted<RunContext, Summary> WhenBenchmarksExecuted(
        this BddScenario.Arranged<RunContext> scenario
    ) => scenario.ActAndExecuteBenchmarks();
}
