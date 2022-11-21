namespace BunsenBurner.BenchmarkDotNet;

using AaaScenario = Scenario<Syntax.Aaa>;

/// <summary>
/// DSL for building tests using an arrange, act, assert syntax
/// </summary>
public static class Aaa
{
    /// <summary>
    /// Arranges a set of performance benchmarks to be run
    /// </summary>
    /// <param name="name">name/description</param>
    /// <param name="configure">optional configuration</param>
    /// <param name="logSink">optional log message sink</param>
    /// <param name="runParameters">optional parameters</param>
    /// <typeparam name="TBenchmarks">type of benchmarks</typeparam>
    /// <returns>arranged scenario</returns>
    [Pure]
    public static AaaScenario.Arranged<RunContext> ArrangeBenchmarks<TBenchmarks>(
        this string name,
        Func<ManualConfig, ManualConfig>? configure = default,
        Action<string>? logSink = default,
        params string[] runParameters
    ) where TBenchmarks : class =>
        Shared.ArrangeBenchmarks<Syntax.Aaa, TBenchmarks>(name, configure, logSink, runParameters);

    /// <summary>
    /// Arranges a set of performance benchmarks to be run
    /// </summary>
    /// <param name="configure">optional configuration</param>
    /// <param name="logSink">optional log message sink</param>
    /// <param name="runParameters">optional parameters</param>
    /// <typeparam name="TBenchmarks">type of benchmarks</typeparam>
    /// <returns>arranged scenario</returns>
    [Pure]
    public static AaaScenario.Arranged<RunContext> ArrangeBenchmarks<TBenchmarks>(
        Func<ManualConfig, ManualConfig>? configure = default,
        Action<string>? logSink = default,
        params string[] runParameters
    ) where TBenchmarks : class =>
        Shared.ArrangeBenchmarks<Syntax.Aaa, TBenchmarks>(configure, logSink, runParameters);

    /// <summary>
    /// Acts and executes the performance benchmarks defined by the run context
    /// </summary>
    /// <param name="scenario">arranged scenario</param>
    /// <returns>acted scenario</returns>
    [Pure]
    public static AaaScenario.Acted<RunContext, Summary> ActAndExecuteBenchmarks(
        this AaaScenario.Arranged<RunContext> scenario
    ) => Shared.ActAndExecuteBenchmarks(scenario);
}
