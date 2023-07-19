<!-- markdownlint-disable MD013 -->

# ![Aplicación Bunsen Burner Bunsen](https://raw.githubusercontent.com/bmazzarol/Bunsen-Burner/main/fire-icon-small.png) Burner Function

<!-- markdownlint-enable MD013 -->

[![Nuget](https://img.shields.io/nuget/v/BunsenBurner.FunctionApp)](https://www.nuget.org/packages/BunsenBurner.FunctionApp/)

## Empezar

Para usar esta biblioteca, simplemente incluya `BunsenBurner.FunctionApp.dll` en su
proyecto
o agarrar
de [NuGet](https://www.nuget.org/packages/BunsenBurner.FunctionApp/), y
Agregue esto a la parte superior de cada archivo de prueba `.cs` 
que lo necesita:

```C#
using BunsenBurner.FunctionApp;
```

## ¿Qué?

Esto proporciona un conjunto de extensiones y tipos para crear aplicaciones de función de prueba
¡fácil!

Organice los datos de solicitud requeridos que acepta la aplicación de función,

```c#
Arrange(() => 2) // some data required to call the function app
     // this builds the function app from the Startup class
    .AndFunctionApp<int, Startup, Function>()
     // now the function instance is in scope along with the params
     // just call it and return the result
    .ActAndExecute(
        x => x.FunctionApp,
        async (i, function) =>
        {
            // example HTTP trigger, the Bunsen Burner HTTP library can be used here
            var result = await function.SomeFunctionTrigger(
                await Req.Get.To($"/some-path/{i.Data}".SetQueryParam("noBody")).AsHttpRequest()
            );
            // extension methed to get the ObjectResult back into a HTTP Response
            return result.AsResponse();
        }
    )
    // assertions as normal
    .Assert(async resp =>
    {
        Assert.Equal(HttpStatusCode.OK, resp.StatusCode);
        Assert.Empty(await resp.Content.ReadAsStringAsync());
    });
```

¡Eso es todo! Simplemente redacte parámetros y haga valer contra los resultados.

Para obtener más ejemplos, consulte el proyecto de prueba, cree un problema o inicie un
discusión.
