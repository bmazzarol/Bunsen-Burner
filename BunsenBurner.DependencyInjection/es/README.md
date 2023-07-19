<!-- markdownlint-disable MD013 -->

# ![Bunsen Burner](https://raw.githubusercontent.com/bmazzarol/Bunsen-Burner/main/fire-icon-small.png) Bunsen Burner Inyección de dependencia

<!-- markdownlint-enable MD013 -->

[![Nuget](https://img.shields.io/nuget/v/BunsenBurner.DependencyInjection)](https://www.nuget.org/packages/BunsenBurner.DependencyInjection/)

Esto proporciona métodos de extensión para probar que un contenedor DI estándar es
configurado correctamente.

## Empezar

Para utilizar esta biblioteca, simplemente incluya `BunsenBurner.DependencyInjection.dll` en
Tu proyector agárralo
de [NuGet](https://www.nuget.org/packages/BunsenBurner.DependencyInjection/),
y agregue esto a la parte superior de cada archivo de prueba `.cs` que lo necesite:

```C#
using static BunsenBurner.DependencyInjection.Aaa;
```

o

```C#
using static BunsenBurner.DependencyInjection.Bdd;
```

## ¿Qué?

El contenedor estándar de inyección de dependencias en dotnet no es seguro para tipos y
Requiere verificación en tiempo de ejecución para garantizar que está configurado correctamente.

Esta es una prueba muy simple que crea todos los servicios que coinciden con un predicado.

Hay predicados predeterminados que funcionan en un ensamblado o ensamblados. Esto en la mayoría de los
Los casos hacen el trabajo.

## Modo de empleo

Para empezar a usarlo, importe la sintaxis que utiliza el proyecto de prueba.

```c#
using static BunsenBurner.DependencyInjection.Aaa;

public static class DITests
{
    [Fact(DisplayName = "All services are resolvable in the project assembly")]
    public static async Task Case1() =>
        await new Startup()
            .ArrangeData()
            .ActAndAssertServicesAreConfigured(
                // convert the data to a service collection
                startup => startup.ConfigureServices(new ServiceCollection()),
                // now build anything found in the provided assemblies
                typeof(Startup).Assembly);
}
```

Para obtener más información sobre las funciones de organización añadidas, consulte el código y el
Pruebas.
