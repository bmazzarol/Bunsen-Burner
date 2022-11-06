# Bunsen Burner Bogus

[![Nuget](https://img.shields.io/nuget/v/BunsenBurner.Bogus)](https://www.nuget.org/packages/BunsenBurner.Bogus/)

This provides extension methods to
integrate [Bogus](https://github.com/bchavez/Bogus) into the arrange/given
step.

## Getting Started

To use this library, simply include `BunsenBurner.Bogus.dll` in your
project
or grab
it from [NuGet](https://www.nuget.org/packages/BunsenBurner.Bogus/), and add
this to the top of each test `.cs` file
that needs it:

```C#
using static BunsenBurner.Bogus.Aaa;
```

or

```C#
using static BunsenBurner.Bogus.Bdd;
```

## What?

[Bogus](https://github.com/bchavez/Bogus) is a fake data generator for DotNet.

This can help reduce the amount of code required in the arrange/given step.

## How to use

To start using it import the syntax that your test project is using.

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

For more information on the added arrange functions check out the code and the
tests.
