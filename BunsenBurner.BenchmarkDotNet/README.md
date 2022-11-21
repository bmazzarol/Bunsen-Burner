# ![](https://raw.githubusercontent.com/bmazzarol/Bunsen-Burner/main/fire-icon-small.png) Bunsen Burner BenchmarkDotNet

[![Nuget](https://img.shields.io/nuget/v/BunsenBurner.BenchmarkDotNet)](https://www.nuget.org/packages/BunsenBurner.BenchmarkDotNet/)

This provides integration
with [BenchmarkDotNet](https://github.com/dotnet/BenchmarkDotNet) to write
performance tests.

## Getting Started

To use this library, simply include `BunsenBurner.BenchmarkDotNet.dll` in your
project
or grab
it from [NuGet](https://www.nuget.org/packages/BunsenBurner.BenchmarkDotNet/),
and add
this to the top of each test `.cs` file
that needs it:

```C#
using static BunsenBurner.BenchmarkDotNet.Aaa;
```

or

```C#
using static BunsenBurner.BenchmarkDotNet.Bdd;
```

## What?

[BenchmarkDotNet](https://github.com/dotnet/BenchmarkDotNet) is an easy to use
performance/benchmarking library.

These are not tests that are fast to run, they should be pulled into a separate
project.

## How to use

To start using it import the syntax that your test project is using.

```c#
using static Aaa;

public class AaaTests
{
    private readonly ITestOutputHelper _outputHelper;

    public AaaTests(ITestOutputHelper outputHelper) => _outputHelper = outputHelper;

    [Fact(DisplayName = "Benchmarks can be run and asserted against")]
    public async Task Case1() =>
        // for a supported benchmarks class
        await ArrangeBenchmarks<TestBenchmarks>(
                // custom configuration
                config => config.AddDiagnoser(...),
                // location of log messages
                logSink: _outputHelper.WriteLine,
                // extra parameters
                "test",
                "test2"
            ) 
            .ActAndExecuteBenchmarks() // run benchmarks, can take a while 
            .Assert(
                r => // summary to assert against
                {
                    Assert.Empty(r.ValidationErrors);
                    Assert.NotEqual(r.TotalTime, TimeSpan.Zero);
                });
}
```

For more information on performance testing and how to use BenchmarkDotNet with
Bunsen Burner checkout the tests or raise a question/issue.
