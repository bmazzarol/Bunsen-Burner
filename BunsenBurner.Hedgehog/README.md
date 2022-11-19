# ![](https://raw.githubusercontent.com/bmazzarol/Bunsen-Burner/main/fire-icon-small.png) Bunsen Burner Hedgehog

[![Nuget](https://img.shields.io/nuget/v/BunsenBurner.Hedgehog)](https://www.nuget.org/packages/BunsenBurner.Hedgehog/)

This provides integration
with [Hedgehog](https://github.com/hedgehogqa/fsharp-hedgehog) to write clean
and easy property based tests.

## Getting Started

To use this library, simply include `BunsenBurner.Hedgehog.dll` in your
project
or grab
it from [NuGet](https://www.nuget.org/packages/BunsenBurner.Hedgehog/), and add
this to the top of each test `.cs` file
that needs it:

```C#
using static BunsenBurner.Hedgehog.Aaa;
```

or

```C#
using static BunsenBurner.Hedgehog.Bdd;
```

## What?

[Hedgehog](https://github.com/hedgehogqa/fsharp-hedgehog) is an easy to use
[property based testing](https://en.wikipedia.org/wiki/Property_testing)
library.

It provides a way to build data generators that
can [auto shrink](https://hypothesis.works/articles/integrated-shrinking/).

Property based tests are great
for
testing [pure/deterministic functions](https://en.wikipedia.org/wiki/Pure_function)
. _(Functions that always return the same result for a given set of inputs)_

## How to use

To start using it import the syntax that your test project is using.

```c#
using Hedgehog.Linq;
using Range = Hedgehog.Linq.Range;
using static BunsenBurner.Hedgehog.Aaa;

public class AaaTests
{
    [Fact(DisplayName = "The string is not empty and is less then 25 charecters")]
    public async Task Case1() =>
        // compose a generator using Linq
        await Gen.Alpha
            .String(Range.FromValue(25))
            // convert the generator to a scenario
            .ArrangeGenerator()
            // assert that the provided property holds for generated data
            .AssertPropertyHolds(s => s != string.Empty && s.Length is > 0 and < 26);
            
    [Fact(DisplayName = "Generators can be combined!")]
    public static async Task Case6() =>
        await (
            // use Linq to combine generators together to make more complex ones
            from name in Gen.Alpha.String(Range.FromValue(25))
            from age in Gen.Int32(Range.LinearFromInt32(1, 20, 99))
            select (name, age)
        )
            .ArrangeGenerator()
            .AssertPropertyHolds(t => t.name != null && t.age > 0);
}
```

For more information on property based testing and how to use Hedgehog with
Bunsen Burner checkout the tests or raise a question/issue.
