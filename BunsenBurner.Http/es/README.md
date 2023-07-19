<!-- markdownlint-disable MD013 -->

# ![Bunsen Burner](https://raw.githubusercontent.com/bmazzarol/Bunsen-Burner/main/fire-icon-small.png) Bunsen Burner http

<!-- markdownlint-enable MD013 -->

[![Nuget](https://img.shields.io/nuget/v/BunsenBurner.Http)](https://www.nuget.org/packages/BunsenBurner.Http/)

## Empezar

Para usar esta biblioteca, simplemente inclúyala `BunsenBurner.Http.dll` en su proyecto
o agarrar
desde [NuGet](https://www.nuget.org/packages/BunsenBurner.Http/), y agregue esto
en la parte superior de cada archivo de prueba `.cs` 
que lo necesita:

```C#
using BunsenBurner.Http;
// allows for fluent building of requests and responses
using HttpBuildR;
using Req = System.Net.Http.HttpMethod;
using Resp = System.Net.HttpStatusCode;
```

## ¿Qué?

Esto proporciona un conjunto de extensiones y tipos para realizar pruebas de servicios basados en HTTP
¡fácil!

Redacte una solicitud así,

```c#
using HttpBuildR;
using Req = System.Net.Http.HttpMethod;
...
var req = 
    // start with a HTTP method
    Req.Get
    // flurl can be used for the URL composition
    .To("/hello-world".SetQueryParam("a", 1)) // all http verbs as covered
    // the non-url based parts are covered by methods
    // including JWT based auth token construction
    .WithHeader("b", 123, x => x.ToString())
```

A continuación, convierta la solicitud en un escenario que se pueda ejecutar en una prueba o
servidor real.

El generador de servidores de prueba proporciona una forma opinada de crear servidores de prueba.

```c#
req.ArrangeRequest() // convert to a scenario
    // run the request against the test server defined by the Startup class
   .ActAndCall(TestServerBuilderOptions.New<Startup>().Build())
    // a response context contains the http response and all log messages produced
    // while handling the request
   .Assert(ctx => ctx.Response.StatusCode == HttpStatusCode.OK);
```

Y HttpClient se puede burlar a través de un HttpMessageStore

```c#
using HttpBuildR;
using static BunsenBurner.Http.HttpMessageMatchers;
using Req = System.Net.Http.HttpMethod;
using Resp = System.Net.HttpStatusCode;

var store = HttpMessageStore.New();
store.Setup(
    // for a given named client
    "PersonService",
    // matchers can be used and composed to match incomming requests
    HasMethod(HttpMethod.Put).And(HasJsonContent((Person p) => p.Age > 19))),
    // response builder can be provided
    req => Resp.OK.Result(request: req)
                  .WithJsonContent(new { LastUpdatedDate = DateTime.Now })
...
// now a store can be converted to a client, or passed to a DummyFactory
var client = store.CreateClient("PersonService");
// now call the client
var result = await client.SendAsync(Req.Put.To("some-endpoint")
                                           .WithBearerToken(...)
                                           .WithJsonContent(new Person(25)));
// the store records all requests and responses made against it
Assert.True(store.Any(m => m.ClientName == "PersonService"
                        && m.Request.Method == HttpMethod.Put
                        && m.Response.StatusCode == Resp.OK))
```

¡Eso es todo! Simplemente redacte solicitudes y haga valer las respuestas.

Para obtener más ejemplos, consulte el proyecto de prueba, cree un problema o inicie un
discusión.
