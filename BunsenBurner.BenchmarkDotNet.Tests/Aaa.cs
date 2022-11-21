using Xunit.Abstractions;

namespace BunsenBurner.BenchmarkDotNet.Tests;

using static Aaa;

[Collection(nameof(NoOpBenchmarks))]
public class AaaTests
{
    private readonly ITestOutputHelper _outputHelper;

    public AaaTests(ITestOutputHelper outputHelper) => _outputHelper = outputHelper;

    [Fact(
        DisplayName = "Benchmarks without config can be run and asserted against",
        Skip = "Long running"
    )]
    public async Task Case1() =>
        await ArrangeBenchmarks<TestBenchmarks>(
                config => config.AddDiagnoser(),
                logSink: _outputHelper.WriteLine,
                "test",
                "test2"
            )
            .ActAndExecuteBenchmarks()
            .Assert(r =>
            {
                Assert.Empty(r.ValidationErrors);
                Assert.NotEqual(r.TotalTime, TimeSpan.Zero);
            });

    [Fact(DisplayName = "Benchmarks without config can be run and asserted against")]
    public async Task Case2() =>
        await ArrangeBenchmarks<NoOpBenchmarks>().ActAndExecuteBenchmarks().Assert(Assert.NotNull);

    [Fact(
        DisplayName = "Benchmarks without config can be run and asserted against with description"
    )]
    public async Task Case3() =>
        await "Some Description"
            .ArrangeBenchmarks<NoOpBenchmarks>()
            .ActAndExecuteBenchmarks()
            .Assert(Assert.NotNull);
}
