<!-- markdownlint-disable MD013 -->

# ![Bunsen Burner](https://raw.githubusercontent.com/bmazzarol/Bunsen-Burner/main/fire-icon-small.png) Bunsen Burner Core

<!-- markdownlint-enable MD013 -->

[![Nuget](https://img.shields.io/nuget/v/BunsenBurner)](https://www.nuget.org/packages/BunsenBurner/)

Esto proporciona la abstracción de prueba libre de dependencia central que Bunsen Burner es
construido sobre.

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

## ¿Qué?

El tipo que hace posible esta biblioteca es un escenario.

Este es un [ADT](https://en.wikipedia.org/wiki/Algebraic_data_type) que modela
una sola prueba. Tiene la siguiente estructura,

```c#
Scenario<
 // Supported Syntax, either arrange act assert (Aaa) or given when then (Bdd)
    TSyntax
>.(
 // Scenario that has been arranged and is ready to act on, returning a TData   
    Arranged<TData> | 
 // Scenario that has been acted on, taking a TData returning a TResult and is 
 // ready to assert against 
    Acted<TData, TResult> | 
 // Scenario that is asserted against a TResult. This can be run.
    Asserted<TData, TResult>)
```

Esta estructura representa una prueba asíncrona inmutable perezosa que puede ser
compuesto y
Ejecución posterior.

El objetivo es que los parámetros de tipo sean completamente inferibles. Hay algunas ventajas
casos para esto, pero las abstracciones centrales no requieren parámetros de tipo proporcionados.

## Modo de empleo

Para empezar a usarlo, importe la sintaxis Aaa o Bdd y comience a componer el
prueba.

### Organizar, actuar, afirmar

```c#
using static BunsenBurner.Aaa;

[Fact(DisplayName = "Some simple test")]
public async Task Case1() =>
  await Arrange(() => 1)
      .And(x => x.ToString())
      .Act(x => x.Length)
      .And((_, r) => r + 1)
      .Assert((_, r) => Assert.Equal(2, r));
```

### dado, cuándo, entonces

```c#
using static BunsenBurner.Bdd;

[Fact(DisplayName = "Some simple test")]
public async Task Case1() =>
  await Given(() => 1)
      .And(x => x.ToString())
      .When(x => x.Length)
      .And((_, r) => r + 1)
      .Then((_, r) => Assert.Equal(2, r));
```

La prueba se ejecuta con un awaiter personalizado, por lo que se requiere una espera asincrónica.

Además, como la canalización es completamente asincrónica, siempre debe esperar un escenario, incluso
si todos los componentes son sincrónicos.

## Beneficios

* Estandarizado - Todas las pruebas se basan en la misma abstracción, puede modelar cualquier
  prueba
  * Debe estar compuesto en la secuencia correcta
    * Arreglar/Dar
    * Actúa/Cuándo
    * Afirmar/Entonces
  * Y los pasos se pueden usar entre para permitir transformaciones
* Basado en datos: los escenarios son solo datos, se pueden pasar hacia y desde las funciones,
  se puede poner
  en una lista, asignada a variables
* Composable: tanto dentro de una sola prueba como en varias pruebas
  * Escriba métodos de extensión personalizados para extender la sintaxis en cualquier dirección, con
    La comodidad de conocerlo asegurará que el desarrollador lo use correctamente.
    Basta con mirar las otras bibliotecas proporcionadas para inspirarse
  * Funcional primero, son solo datos con funciones que operan en esos datos
* Legible - Estructuras forzadas sin una manera fácil de dispararse en el pie,
  debe dar como resultado un código que sea más fácil de mantener y leer
* Simple: 1 tipo de datos y funciones que operan en él
* Flexible
  * Sin dependencias
  * Sin suposiciones en torno a los marcos de prueba
  * No hay suposiciones en torno a SUT
  * No hay requisitos en el código de usuario
