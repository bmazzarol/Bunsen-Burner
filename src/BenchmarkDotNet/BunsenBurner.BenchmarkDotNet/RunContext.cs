namespace BunsenBurner.BenchmarkDotNet;

/// <summary>
/// Context for running a benchmark
/// </summary>
public sealed record RunContext
{
    /// <summary>
    /// Type of benchmark to run
    /// </summary>
    public Type Benchmark { get; init; }

    /// <summary>
    /// Configuration for the benchmark
    /// </summary>
    public IConfig Config { get; init; }

    /// <summary>
    /// Parameters
    /// </summary>
    public IEnumerable<string> Parameters { get; init; }

    private RunContext(Type benchmark, IConfig config, IEnumerable<string> parameters)
    {
        Benchmark = benchmark;
        Config = config;
        Parameters = parameters;
    }

    /// <summary>
    /// Constructs a new benchmark run context
    /// </summary>
    /// <param name="config">configuration</param>
    /// <param name="parameters">parameters</param>
    /// <typeparam name="TBenchmark">type of benchmark to run</typeparam>
    /// <returns>run context</returns>
    public static RunContext New<TBenchmark>(IConfig config, IEnumerable<string> parameters)
        where TBenchmark : class => new(typeof(TBenchmark), config, parameters);
}
