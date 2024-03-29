﻿using BenchmarkDotNet.Exporters;

namespace BunsenBurner.BenchmarkDotNet;

/// <summary>
/// Shared builder function implementations
/// </summary>
internal static class Shared
{
    [Pure]
    internal static Scenario<TSyntax>.Arranged<RunContext> ArrangeBenchmarks<TSyntax, TBenchmarks>(
        this string name,
        Func<ManualConfig, ManualConfig>? configure = default,
        Action<string>? logSink = default,
        params string[] runParameters
    )
        where TSyntax : struct, Syntax
        where TBenchmarks : class =>
        name.Arrange<RunContext, TSyntax>(
            RunContext.New<TBenchmarks>(
                (
                    configure?.Invoke(ManualConfig.CreateEmpty())
                    ?? ManualConfig
                        .CreateMinimumViable()
                        .AddLogger(new TestLogger(logSink ?? Console.WriteLine))
                )
                    .WithOptions(ConfigOptions.DisableOptimizationsValidator)
                    .AddExporter(BenchmarkReportExporter.Default),
                runParameters
            )
        );

    [Pure]
    internal static Scenario<TSyntax>.Arranged<RunContext> ArrangeBenchmarks<TSyntax, TBenchmarks>(
        Func<ManualConfig, ManualConfig>? configure = default,
        Action<string>? logSink = default,
        params string[] runParameters
    )
        where TSyntax : struct, Syntax
        where TBenchmarks : class =>
        string.Empty.ArrangeBenchmarks<TSyntax, TBenchmarks>(configure, logSink, runParameters);

    [Pure]
    internal static Scenario<TSyntax>.Acted<RunContext, Summary> ActAndExecuteBenchmarks<TSyntax>(
        this Scenario<TSyntax>.Arranged<RunContext> scenario
    )
        where TSyntax : struct, Syntax =>
        scenario.Act(
            ctx =>
                Task.Run(
                    () =>
                        ctx.Parameters.Any()
                            ? BenchmarkRunner.Run(
                                ctx.Benchmark,
                                ctx.Config,
                                ctx.Parameters.ToArray()
                            )
                            : BenchmarkRunner.Run(ctx.Benchmark, ctx.Config)
                )
        );
}
