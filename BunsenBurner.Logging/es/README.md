<!-- markdownlint-disable MD013 -->

# ![Bunsen Burner](https://raw.githubusercontent.com/bmazzarol/Bunsen-Burner/main/fire-icon-small.png) Bunsen Burner Logging

<!-- markdownlint-enabled MD013 -->

[![Nuget](https://img.shields.io/nuget/v/BunsenBurner.Logging)](https://www.nuget.org/packages/BunsenBurner.Logging/)

Esto proporciona un conjunto de abstracciones de registro que permiten realizar aserciones.
contra la tala realizada por el SUT.

## Empezar

Para usar esta biblioteca, simplemente inclúyala `BunsenBurner.Logging.dll` en su proyecto
o agarrar
desde [NuGet ](https://www.nuget.org/packages/BunsenBurner.Logging/)y agregar
Esto en la parte superior de cada archivo de prueba `.cs`
que lo necesita:

```C#
using BunsenBurner.Logging;
```

## ¿Qué?

Hay dos tipos,

1. DummyLogger - Implementación de un ILogger que inicia sesión en un subproceso en memoria
   Recogida segura
2. LogMessageStore: receptor de mensajes seguro para subprocesos diseñado para compartirse
   a través de instancias de DummyLogger

Ambos tipos extienden IEnumerable de LogMessage, por lo que se pueden utilizar fácilmente en
Afirmaciones.

Para obtener más detalles, consulte las pruebas unitarias o el código.

## Modo de empleo

Para comenzar a usarlo, importe el registrador y cree una instancia del mismo,

```c#
using BunsenBurner.Logging;

var logger = DummyLogger.New<object>();

logger.LogInformation("Some log message");

// assert against the logger instance
Assert.Contains(logger, x => x.Message == "Some log message");
```

o reemplace el registro proporcionado en el contenedor DI

```c#
using BunsenBurner.Logging;

var store = LogMessageStore.New();

service.AddDummyLogger(store);
var sp = service.BuildServiceProvider();
var logger = sp.GetRequiredService<ILogger<object>>();
logger.LogInformation("Some log message");
// assert against the shared log message store
Assert.Contains(store, x => x.Message == "Some log message");
```
