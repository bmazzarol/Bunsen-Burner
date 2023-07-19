<!-- markdownlint-disable MD013 -->

# ![Bunsen Burner ](https://raw.githubusercontent.com/bmazzarol/Bunsen-Burner/main/fire-icon-small.png) Bunsen Burner Auto Fixture

<!-- markdownlint-enable MD013 -->

[![Nuget](https://img.shields.io/nuget/v/BunsenBurner.AutoFixture)](https://www.nuget.org/packages/BunsenBurner.AutoFixture/)

Esto proporciona métodos de extensión para
integrar [AutoFixture](https://github.com/AutoFixture) en el arreglo/dado
paso.

## Empezar

Para usar esta biblioteca, simplemente incluya `BunsenBurner.AutoFixture.dll` en su
proyecto
o agarrar
de [NuGet](https://www.nuget.org/packages/BunsenBurner.AutoFixture/), y
Agregue esto a la parte superior de cada archivo de prueba `.cs` 
que lo necesita:

```C#
using static BunsenBurner.AutoFixture.Aaa;
```

o

```C#
using static BunsenBurner.AutoFixture.Bdd;
```

## ¿Qué?

[AutoFixture](https://github.com/AutoFixture) es una muestra automática aleatoria
Constructor con un gran número de extensiones.

Para algunas pruebas, puede reducir el código y aumentar la cobertura de la prueba, mientras que
Enfocar al desarrollador en la lógica central bajo prueba. (Recomiendo encarecidamente
leyendo a [Mark Seemann](https://blog.ploeh.dk/2009/03/22/AnnouncingAutoFixture/)
todo lo que escribe es genial e informativo)

## Modo de empleo

Para empezar a usarlo, importe la sintaxis que utiliza el proyecto de prueba.

```c#
using static BunsenBurner.AutoFixture.Aaa;

public class AaaTests
{
    [Fact(DisplayName = "Some test with auto arranged data")]
    public async Task Case1() =>
        // generate some T, in this case strings
        await AutoArrange(f => f.CreateMany<string>())
            // use this generated data to exercise the SUT
            .Act(d => d.Select(x => x.Length))
            // assert against the result
            .Assert(r => Assert.All(r, i => Assert.True(i > 0)));
}
```

Para obtener más información sobre las funciones de organización añadidas, consulte el código y el
Pruebas.
