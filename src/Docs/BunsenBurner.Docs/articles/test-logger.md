# Test Logging

Bunsen Burner provides a implementation of a test logger that can be used to
assert against logs that have occured while acting.

It was added because there is no general purpose _(not tied to a particular
test framework)_ test logger that is dependency free.

To use this library, simply include `BunsenBurner.Logging.dll` in your project
or grab
it from [NuGet](https://www.nuget.org/packages/BunsenBurner.Logging/), and add
this to the top of each test `.cs` file
that needs it:

```C#
using BunsenBurner.Logging;
```

A @BunsenBurner.Logging.LogMessageStore provides the core storage for instances
of @BunsenBurner.Logging.DummyLogger`1

It is expected that you create one @BunsenBurner.Logging.LogMessageStore and as
many @BunsenBurner.Logging.DummyLogger`1 as required.

[!code-csharp[Example1](../../../Logging/BunsenBurner.Logging.Tests/Examples/LoggingExample.cs#Example1)]
