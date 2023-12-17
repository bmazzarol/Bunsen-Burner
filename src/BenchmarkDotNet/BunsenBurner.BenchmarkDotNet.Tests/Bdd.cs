namespace BunsenBurner.BenchmarkDotNet.Tests;

using static Bdd;

[Collection(nameof(NoOpBenchmarks))]
public static class BddTests
{
    [Fact(DisplayName = "Benchmarks without config can be run and asserted against")]
    public static async Task Case1() =>
        await GivenBenchmarks<NoOpBenchmarks>().WhenBenchmarksExecuted().Then(Assert.NotNull);

    [Fact(
        DisplayName = "Benchmarks without config can be run and asserted against with description"
    )]
    public static async Task Case2() =>
        await "Some Description"
            .GivenBenchmarks<NoOpBenchmarks>()
            .WhenBenchmarksExecuted()
            .Then(Assert.NotNull);
}
