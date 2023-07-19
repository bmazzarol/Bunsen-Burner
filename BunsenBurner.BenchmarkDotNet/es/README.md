<!-- markdownlint-disable MD013 -->

# ![Bunsen Burner](https://raw.githubusercontent.com/bmazzarol/Bunsen-Burner/main/fire-icon-small.png) Bunsen Burner BenchmarkDotNet

<!-- markdownlint-enable MD013 -->

[![Nuget](https://img.shields.io/nuget/v/BunsenBurner.BenchmarkDotNet)](https://www.nuget.org/packages/BunsenBurner.BenchmarkDotNet/)

Esto proporciona integración
con [BenchmarkDotNet](https://github.com/dotnet/BenchmarkDotNet) para escribir
Pruebas de rendimiento.

## Empezar

Para usar esta biblioteca, simplemente incluya `BunsenBurner.BenchmarkDotNet.dll` en su
proyecto
o agarrar
de [NuGet](https://www.nuget.org/packages/BunsenBurner.BenchmarkDotNet/),
y añadir
Esto en la parte superior de cada archivo de prueba `.cs` 
que lo necesita:

```C#
using static BunsenBurner.BenchmarkDotNet.Aaa;
```

o

```C#
using static BunsenBurner.BenchmarkDotNet.Bdd;
```

## ¿Qué?

[BenchmarkDotNet](https://github.com/dotnet/BenchmarkDotNet) es un
Biblioteca de rendimiento/benchmarking.

Estas no son pruebas que son rápidas de ejecutar, deben colocarse en un
proyecto.

## Modo de empleo

Para empezar a usarlo, importe la sintaxis que utiliza el proyecto de prueba.

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

Para obtener más información sobre las pruebas de rendimiento y cómo utilizar BenchmarkDotNet con
Bunsen Burner revisa las pruebas o plantea una pregunta / problema.
