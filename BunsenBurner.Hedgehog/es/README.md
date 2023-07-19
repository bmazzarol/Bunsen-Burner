<!-- markdownlint-disable MD013 -->

# ![Bunsen Burner ](https://raw.githubusercontent.com/bmazzarol/Bunsen-Burner/main/fire-icon-small.png) Bunsen Burner Erizo

<!-- markdownlint-enabled MD013 -->

[![Nuget](https://img.shields.io/nuget/v/BunsenBurner.Hedgehog)](https://www.nuget.org/packages/BunsenBurner.Hedgehog/)

Esto proporciona integración
con [Hedgehog](https://github.com/hedgehogqa/fsharp-hedgehog) para escribir limpio
y pruebas fáciles basadas en propiedades.

## Empezar

Para usar esta biblioteca, simplemente incluya `BunsenBurner.Hedgehog.dll` en su
proyecto
o agarrar
desde [NuGet ](https://www.nuget.org/packages/BunsenBurner.Hedgehog/)y agregar
Esto en la parte superior de cada archivo de prueba `.cs`
que lo necesita:

```C#
using static BunsenBurner.Hedgehog.Aaa;
```

o

```C#
using static BunsenBurner.Hedgehog.Bdd;
```

## ¿Qué?

[Hedgehog](https://github.com/hedgehogqa/fsharp-hedgehog) es un erizo fácil de usar
[Pruebas basadas en propiedades](https://en.wikipedia.org/wiki/Property_testing)
biblioteca.

Proporciona una forma de crear generadores de datos que
puede [encogerse automáticamente](https://hypothesis.works/articles/integrated-shrinking/).

Las pruebas basadas en propiedades son excelentes
para
Prueba de [funciones ](https://en.wikipedia.org/wiki/Pure_function)puras/deterministas
. _(Funciones que siempre devuelven el mismo resultado para un conjunto dado de entradas)_

## Modo de empleo

Para empezar a usarlo, importe la sintaxis que utiliza el proyecto de prueba.

```c#
using Hedgehog.Linq;
using Range = Hedgehog.Linq.Range;
using static BunsenBurner.Hedgehog.Aaa;

public class AaaTests
{
    [Fact(DisplayName = "The string is not empty and is less then 25 charecters")]
    public async Task Case1() =>
        // compose a generator using Linq
        await Gen.Alpha
            .String(Range.FromValue(25))
            // convert the generator to a scenario
            .ArrangeGenerator()
            // assert that the provided property holds for generated data
            .AssertPropertyHolds(s => s != string.Empty && s.Length is > 0 and < 26);
            
    [Fact(DisplayName = "Generators can be combined!")]
    public static async Task Case6() =>
        await (
            // use Linq to combine generators together to make more complex ones
            from name in Gen.Alpha.String(Range.FromValue(25))
            from age in Gen.Int32(Range.LinearFromInt32(1, 20, 99))
            select (name, age)
        )
            .ArrangeGenerator()
            .AssertPropertyHolds(t => t.name != null && t.age > 0);
}
```

Para obtener más información sobre las pruebas basadas en propiedades y cómo usar Hedgehog con
Bunsen Burner revisa las pruebas o plantea una pregunta / problema.
