# Bunsen Burner Logging

This provides a set of logging abstractions that allow for assertions to be made
against logging done by the SUT.

## Getting Started

To use this library, simply include `BunsenBurner.Logging.dll` in your project
or grab
it from NuGet (Coming Soon), and add this to the top of each test `.cs` file
that needs it:

```C#
using BunsenBurner.Logging;
```

## What?

There are two types,

1. DummyLogger - Implementation of an ILogger that logs to an in-memory thread
   safe collection
2. LogMessageStore - Thread safe message sink that is designed to be shared
   across instances of DummyLogger

Both types extend IEnumerable of LogMessage, so can be easily used in
assertions.

For further details check out the unit tests or code.

## How to use

To start using it import the logger and create an instance of it,

```c#
using BunsenBurner.Logging;

var logger = DummyLogger.New();

logger.LogInformation("Some log message");

// assert against the logger instance
Assert.Contains(logger, x => x.Message == "Some log message");
```

or replace the log provided in the DI container

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
