<!-- markdownlint-disable MD013 -->

# ![Bunsen Burner](https://raw.githubusercontent.com/bmazzarol/Bunsen-Burner/main/fire-icon-small.png) Bunsen Burner NUnit

<!-- markdownlint-enabled MD013 -->

[![Nuget](https://img.shields.io/nuget/v/BunsenBurner.NUnit)](https://www.nuget.org/packages/BunsenBurner.NUnit/)

Esto proporciona métodos de extensión para
integrarse con [NUnit](https://github.com/nunit/nunit) y consumir fácilmente
Quemador Bunsen.

## Empezar

Para usar esta biblioteca, simplemente incluya `BunsenBurner.NUnit.dll` en su
proyecto
o agarrar
de [NuGet](https://www.nuget.org/packages/BunsenBurner.NUnit/), y
Agregue esto a la parte superior de cada archivo de prueba `.cs` 
que lo necesita:

```C#
using BunsenBurner.NUnit;
```

## ¿Qué?

[NUnit](https://github.com/nunit/nunit) es un marco de pruebas para Dotnet.

Se recomienda utilizar la paralelización completa, esto se puede hacer en el
Nivel de montaje.

```c#
using NUnit.Framework;

[assembly: Parallelizable(ParallelScope.All)]
```

## Modo de empleo

Para empezar a usarlo, importe los métodos de extensión y comience a crear pruebas.

Por ejemplo, cree datos de casos de prueba a partir de escenarios.

```c#
// this can help with readability
using TestScenario = Scenario<Syntax.Aaa>.Asserted<int, string>;

// define the test cases here
public static TheoryData<TestScenario> TestCases = TheoryData(
    "Some test".Arrange(1).Act(x => x.ToString()).Assert(r => r == "1"),
    "Some other test".Arrange(2).Act(x => x.ToString()).Assert(r => r == "2")
);

// run them
[Test(Description = "Example running scenarios from theory data")]
[TestCaseSource(nameof(TestCases))]
public static async Task Case4(TestScenario scenario) => await scenario;
```

Para obtener más ejemplos, consulte el proyecto de prueba, cree un problema o inicie un
discusión.
