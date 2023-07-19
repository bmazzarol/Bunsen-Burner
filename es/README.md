<!-- markdownlint-disable MD033 MD041 -->
<div align="center">

<img src="fire-icon.png" alt="Bunsen Burner" width="150px"/>

# Quemador Bunsen

[:running**_: Introducción_**](https://bmazzarol.github.io/Bunsen-Burner/articles/getting-started.html)
:[libros: **_Documentación_**](https://bmazzarol.github.io/Bunsen-Burner)

[![Nuget](https://img.shields.io/nuget/v/BunsenBurner)](https://www.nuget.org/packages/BunsenBurner/)
[![Cobertura](https://sonarcloud.io/api/project_badges/measure?project=bmazzarol_Bunsen-Burner&metric=coverage)](https://sonarcloud.io/summary/new_code?id=bmazzarol_Bunsen-Burner)
Estado [![
 ](https://sonarcloud.io/api/project_badges/measure?project=bmazzarol_Bunsen-Burner&metric=alert_status)](https://sonarcloud.io/summary/new_code?id=bmazzarol_Bunsen-Burner)de la puerta de calidad[![Compilación de](https://github.com/bmazzarol/Bunsen-Burner/actions/workflows/cd-build.yml/badge.svg)](https://github.com/bmazzarol/Bunsen-Burner/actions/workflows/cd-build.yml)
 CD[![Comprobar Markdown](https://github.com/bmazzarol/Bunsen-Burner/actions/workflows/check-markdown.yml/badge.svg)](https://github.com/bmazzarol/Bunsen-Burner/actions/workflows/check-markdown.yml)
[![CodeQL](https://github.com/bmazzarol/Bunsen-Burner/actions/workflows/codeql.yml/badge.svg)](https://github.com/bmazzarol/Bunsen-Burner/actions/workflows/codeql.yml)

Set :fire: ¡a sus antiguas pruebas unitarias!
Una mejor manera de escribir pruebas :test_tube: en C#.

</div>

## Funciones

* Marco de prueba agnóstico
* Cero dependencias
* Fácil de usar y ampliar
* Más fácil de mantener
* Integraciones a tus bibliotecas de pruebas favoritas
* Unidad, Integración, Propiedad cualquier tipo de prueba!

```c#
// Arrange act assert style

using static BunsenBurner.Aaa;

namespace SomeNamespace;

public static class Tests
{
    [Fact(DisplayName = "Example AAA test!!!")]
    public static async Task SomeTest() =>
        await Arrange(() => new SUT(...))
             .Act(async sut => await sut.SomeMethod(...))
             .Assert(result => Assert.Equal("should be this", result));
}

// Given when then style

using static BunsenBurner.Bdd;

namespace SomeNamespace;

public static class Tests
{
    [Fact(DisplayName = "Example BDD test!!!")]
    public static async Task SomeTest() =>
        await Given(() => new SUT(...))
             .When(async sut => await sut.SomeMethod(...))
             .Then(result => Assert.Equal("should be this", result));
}
```

## Empezar

Para usar esta biblioteca, simplemente inclúyala `BunsenBurner.dll` en su proyecto o tome
desde [NuGet](https://www.nuget.org/packages/BunsenBurner/), y agregue esto a
La parte superior de cada archivo de prueba `.cs` 
que lo necesita:

```C#
using static BunsenBurner.Aaa; // For Arrange act assert
```

o

```C#
using static BunsenBurner.Bdd; // For Given when then
```

Haga clic en los enlaces a continuación para obtener más detalles.<!-- markdownlint-disable MD013 -->

| Biblioteca                                                             | Descripción                                                                                                          | Nu-Get                                                                                                                                        |
|---------------------------------------------------------------------|----------------------------------------------------------------------------------------------------------------------|-----------------------------------------------------------------------------------------------------------------------------------------------|
| [Núcleo](./BunsenBurner/README.md)                                    | Abstracción de prueba central que lo hace posible. ¡Esto es todo lo que se requiere para comenzar!                       | [![Nuget](https://img.shields.io/nuget/v/BunsenBurner)](https://www.nuget.org/packages/BunsenBurner/)                                         |
| [Registro](./BunsenBurner.Logging/README.md)                         | Abstracciones de registro de núcleos. Se usa para hacer valer contra los mensajes registrados, útil para casos como probar servicios en segundo plano | [![Nuget](https://img.shields.io/nuget/v/BunsenBurner.Logging)](https://www.nuget.org/packages/BunsenBurner.Logging/)                         |
| [Xunit](./BunsenBurner.Xunit/README.md)                             | Integración con [xUnit.net](https://github.com/xunit/xunit) para consumir fácilmente Bunsen Burner                         | [![Nuget](https://img.shields.io/nuget/v/BunsenBurner.Xunit)](https://www.nuget.org/packages/BunsenBurner.Xunit/)                             |
| [NUnit](./BunsenBurner.NUnit/README.md)                             | Integración con [NUnit](https://github.com/nunit/nunit) para consumir fácilmente Bunsen Burner                             | [![Nuget](https://img.shields.io/nuget/v/BunsenBurner.NUnit)](https://www.nuget.org/packages/BunsenBurner.NUnit/)                             |
| [AutoFixture](./BunsenBurner.AutoFixture/README.md)                 | Integración con [AutoFixture](https://github.com/AutoFixture) para simplificar los pasos de organización                             | [![Nuget](https://img.shields.io/nuget/v/BunsenBurner.AutoFixture)](https://www.nuget.org/packages/BunsenBurner.AutoFixture/)                 |
| [Falso](./BunsenBurner.Bogus/README.md)                             | Integración con [Bogus](https://github.com/bchavez/Bogus) para simplificar los pasos de organización                                 | [![Nuget](https://img.shields.io/nuget/v/BunsenBurner.Bogus)](https://www.nuget.org/packages/BunsenBurner.Bogus/)                             |
| [DependencyInjection](./BunsenBurner.DependencyInjection/README.md) | Proporciona pruebas para validar contenedores de inyección de dependencias                                                        | [![Nuget](https://img.shields.io/nuget/v/BunsenBurner.DependencyInjection)](https://www.nuget.org/packages/BunsenBurner.DependencyInjection/) |
| [Erizo](./BunsenBurner.Hedgehog/README.md)                       | Integración con [Hedgehog](https://github.com/hedgehogqa/fsharp-hedgehog) para escribir pruebas basadas en propiedades             | [![Nuget](https://img.shields.io/nuget/v/BunsenBurner.Hedgehog)](https://www.nuget.org/packages/BunsenBurner.Hedgehog/)                       |
| [BenchmarkDotNet](./BunsenBurner.BenchmarkDotNet/README.md)         | Integración con [BenchmarkDotNet](https://github.com/dotnet/BenchmarkDotNet) para escribir pruebas de rendimiento             | [![Nuget](https://img.shields.io/nuget/v/BunsenBurner.BenchmarkDotNet)](https://www.nuget.org/packages/BunsenBurner.BenchmarkDotNet/)         |
| [Verify.Xunit](./BunsenBurner.Verify.Xunit/README.md)               | Integración con [Verify.Xunit](https://github.com/VerifyTests/Verify) para simplificar los pasos de aserción                      | [![Nuget](https://img.shields.io/nuget/v/BunsenBurner.Verify.Xunit)](https://www.nuget.org/packages/BunsenBurner.Verify.Xunit/)               |
| [Verify.NUnit](./BunsenBurner.Verify.NUnit/README.md)               | Integración con [Verify.NUnit](https://github.com/VerifyTests/Verify) para simplificar los pasos de aserción                      | [![Nuget](https://img.shields.io/nuget/v/BunsenBurner.Verify.NUnit)](https://www.nuget.org/packages/BunsenBurner.Verify.NUnit/)               |
| [HTTP](./BunsenBurner.Http/README.md)                               | Métodos de extensión para probar servidores Http                                                                           | [![Nuget](https://img.shields.io/nuget/v/BunsenBurner.Http)](https://www.nuget.org/packages/BunsenBurner.Http/)                               |
| [FunctionApp](./BunsenBurner.FunctionApp/README.md)                 | Métodos de extensión para probar aplicaciones de función                                                                          | [![Nuget](https://img.shields.io/nuget/v/BunsenBurner.FunctionApp)](https://www.nuget.org/packages/BunsenBurner.FunctionApp/)                 |
| [Fondo](./BunsenBurner.Background/README.md)                   | Métodos de extensión para probar los servicios en segundo plano                                                                    | [![Nuget](https://img.shields.io/nuget/v/BunsenBurner.Background)](https://www.nuget.org/packages/BunsenBurner.Background/)                   |

<!-- markdownlint-enable MD013 -->

## ¿Por qué?

La mayoría de las pruebas en C# están escritas en un estilo de arreglar, actuar, afirmar, así,

```c#
using Xunit;

namespace SomeNamespace;

public static class Tests
{
    [Fact]
    public static async Task SomeTest()
    {
        // Arrange
        var sut = new SUT(...);
        
        // Act
        var result = await sut.SomeMethod(...);
        
        // Assert
        Assert.Equal("should be this", result);
    }
}
```

Esta biblioteca tiene como objetivo formalizar esta estructura de las siguientes maneras:

* Impone que todas las pruebas deben organizarse antes de actuar y actuar antes
  Las aserciones pueden ocurrir
* Convierte las pruebas en datos, que se pueden componer y construir y luego ejecutar
  * Funciona bien con teorías
* Debido a que las pruebas son solo datos, se pueden usar funciones para extenderlos y componerlos.
  ellos juntos
  * Funciona con métodos de extensión y otras bibliotecas de prueba, casos de uso

```c#
// can use implicit usings
using Xunit;
using static BunsenBurner.Aaa;

namespace SomeNamespace;

public static class Tests
{
    [Fact(DisplayName = "Example AAA test!!!")]
    public static async Task SomeTest() =>
              // arrange starts a new test, 
              // whatever type it returns can be used when acting 
        await Arrange(() => new SUT(...))
              // act on the arranged data, async is supported in all test steps
             .Act(async sut => await sut.SomeMethod(...))
              // assert against the result of acting
             .Assert(result => Assert.Equal("should be this", result));
}
```

Para obtener más detalles / información, eche un vistazo a los proyectos de prueba o cree un problema.

## Atribuciones

[Iconos de fuego creados por juicy_fish](https://www.flaticon.com/free-icons/fire)
