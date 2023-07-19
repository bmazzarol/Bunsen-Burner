<!-- markdownlint-disable MD013 -->

# ![Bunsen Burner](https://raw.githubusercontent.com/bmazzarol/Bunsen-Burner/main/fire-icon-small.png) Bunsen Burner Falso

<!-- markdownlint-enable MD013 -->

[![Nuget](https://img.shields.io/nuget/v/BunsenBurner.Bogus)](https://www.nuget.org/packages/BunsenBurner.Bogus/)

Esto proporciona métodos de extensión para
integrar [Bogus](https://github.com/bchavez/Bogus) en el arreglo/dado
paso.

## Empezar

Para usar esta biblioteca, simplemente incluya `BunsenBurner.Bogus.dll` en su
proyecto
o agarrar
desde [NuGet ](https://www.nuget.org/packages/BunsenBurner.Bogus/)y agregar
Esto en la parte superior de cada archivo de prueba `.cs`
que lo necesita:

```C#
using static BunsenBurner.Bogus.Aaa;
```

o

```C#
using static BunsenBurner.Bogus.Bdd;
```

## ¿Qué?

[Bogus](https://github.com/bchavez/Bogus) es un generador de datos falso para DotNet.

Esto puede ayudar a reducir la cantidad de código requerido en el paso de organización/dado.

## Modo de empleo

Para empezar a usarlo, importe la sintaxis que utiliza el proyecto de prueba.

```c#
using static BunsenBurner.Bogus.Aaa;

public class AaaTests
{
    [Fact(DisplayName = "Some test with fake data")]
    public async Task Case1() =>
        // generate some T, in this case a Person
        await AutoArrange<Person>(
                f =>
                    f.RuleFor(x => x.Name, x => x.Name.FirstName())
                        .RuleFor(x => x.Age, x => x.Random.Int(20, 50))
                        .RuleFor(x => x.Dob, x => x.Person.DateOfBirth)
                        .Generate()
            )
            // some act
            .Act(_ => ...)
            // some assert
            .Assert(Assert.NotNull);
}
```

Para obtener más información sobre las funciones de organización añadidas, consulte el código y el
Pruebas.
