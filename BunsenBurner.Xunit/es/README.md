<!-- markdownlint-disable MD013 -->

# ![Bunsen Burner ](https://raw.githubusercontent.com/bmazzarol/Bunsen-Burner/main/fire-icon-small.png) Bunsen Burner xUnit.net

<!-- markdownlint-enabled MD013 -->

[![Nuget](https://img.shields.io/nuget/v/BunsenBurner.Xunit)](https://www.nuget.org/packages/BunsenBurner.Xunit/)

Esto proporciona métodos de extensión para
Integrarse con [xUnit.net](https://github.com/xunit/xunit) y consumir fácilmente
Quemador Bunsen.

## Empezar

Para usar esta biblioteca, simplemente incluya `BunsenBurner.Xunit.dll` en su
proyecto
o agarrar
de [NuGet](https://www.nuget.org/packages/BunsenBurner.Xunit/), y
Agregue esto a la parte superior de cada archivo de prueba `.cs` 
que lo necesita:

```C#
using BunsenBurner.Xunit;
```

## ¿Qué?

[xUnit.net](https://github.com/xunit/xunit) es un marco de pruebas para Dotnet.

Esto también atrae a
 [Meziantou.Xunit.ParallelTestFramework](https://github.com/meziantou/Meziantou.Xunit.ParallelTestFramework)
que ejecutará todas las pruebas en paralelo de forma predeterminada, lo que funciona mejor con el
Diseño BunsenBurner.

## Modo de empleo

Para empezar a usarlo, importe los métodos de extensión y comience a crear pruebas.

Por ejemplo, crear datos teóricos a partir de escenarios

```c#
// this can help with readability
using TestScenario = Scenario<Syntax.Aaa>.Asserted<int, string>;

// define the test cases here
public static TheoryData<TestScenario> TestCases = TheoryData(
    "Some test".Arrange(1).Act(x => x.ToString()).Assert(r => r == "1"),
    "Some other test".Arrange(2).Act(x => x.ToString()).Assert(r => r == "2")
);

// run them
[Theory(DisplayName = "Example running scenarios from theory data")]
[MemberData(nameof(TestCases))]
public static async Task Case4(TestScenario scenario) => await scenario;
```

Para obtener más ejemplos, consulte el proyecto de prueba, cree un problema o inicie un
discusión.
