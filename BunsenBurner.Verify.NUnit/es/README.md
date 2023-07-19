<!-- markdownlint-disable MD013 -->

# ![Bunsen Burner](https://raw.githubusercontent.com/bmazzarol/Bunsen-Burner/main/fire-icon-small.png) Bunsen Burner Verify NUnit

<!-- markdownlint-enabled MD013 -->

[![Nuget](https://img.shields.io/nuget/v/BunsenBurner.Verify.NUnit)](https://www.nuget.org/packages/BunsenBurner.Verify.NUnit/)

Esto proporciona métodos de extensión para
integrar [Verify.NUnit](https://github.com/VerifyTests/Verify) en el
assert/then
paso.

## Empezar

Para usar esta biblioteca, simplemente incluya `BunsenBurner.Verify.NUnit.dll` en su
proyecto
o agarrar
de [NuGet](https://www.nuget.org/packages/BunsenBurner.Verify.NUnit/), y
Agregue esto a la parte superior de cada archivo de prueba `.cs` 
que lo necesita:

```C#
using BunsenBurner.Verify.NUnit;
```

## ¿Qué?

[Verify.NUnit](https://github.com/VerifyTests/Verify) es una herramienta de instantáneas utilizada para
Reemplace la etapa assert/then de una prueba.

## Modo de empleo

Para empezar a usarlo, importe la sintaxis que utiliza el proyecto de prueba.

```c#
using BunsenBurner.Verify.NUnit;
using static BunsenBurner.Aaa;

public static class AaaTests
{
    [Test(Description = "Assertion with scrubbing and projection")]
    public static async Task Case1() =>
        await Arrange(() => new { A = 1, B = "2", C = DateTimeOffset.Now })
            .Act(_ => _)
            // this will create a snapshot file under __snapshots__ in the same folder as the tests
            // it will then project and scrub the result before saving and comparing with the last
            // saved result
            .AssertResultIsUnchanged(x => new { x.A, x.C }, scrubResults: true);
}
```

Para obtener más información sobre las funciones asertiva/then añadidas, consulte el código y
el
Pruebas.
