<!-- markdownlint-disable MD013 -->

# ![Bunsen Burner](https://raw.githubusercontent.com/bmazzarol/Bunsen-Burner/main/fire-icon-small.png) Fondo de Bunsen Burner

<!-- markdownlint-enable MD013 -->

[![Nuget](https://img.shields.io/nuget/v/BunsenBurner.Background)](https://www.nuget.org/packages/BunsenBurner.Background/)

## Empezar

Para usar esta biblioteca, simplemente incluya `BunsenBurner.Background.dll` en su
proyecto
o agarrar
de [NuGet](https://www.nuget.org/packages/BunsenBurner.Background/), y
Agregue esto a la parte superior de cada archivo de prueba `.cs` 
que lo necesita:

```C#
using BunsenBurner.Background;
```

## ¿Qué?

Esto proporciona un conjunto de extensiones y tipos para realizar pruebas de servicios en segundo plano
¡fácil!

Organice el servicio en segundo plano y luego proporcione una condición para completarlo,

``` c#
ArrangeBackgroundService<Startup, Background>() // some background service and Startup
     // run the background service for no longer than 5 minuets
     // stopping as soon as the log message "Work complete" is logged
    .ActAndRunFor(
        5 * minutes,
        context => context.Store.Any(x => x.Message == "Work complete")
    )
    // assertions against the log messages
    .Assert(store =>
    {
        Assert.NotEmpty(store);
        Assert.Contains(store, x => x.Message == "Work complete");
    });
```

¡Eso es todo! Simplemente redacte el servicio en segundo plano y la condición de salida, luego afirme
contra mensajes de registro.

Para obtener más ejemplos, consulte el proyecto de prueba, cree un problema o inicie un
discusión.
