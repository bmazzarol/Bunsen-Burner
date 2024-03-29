﻿<!-- markdownlint-disable MD013 -->

# ![Bunsen Burner](https://raw.githubusercontent.com/bmazzarol/Bunsen-Burner/main/fire-icon-small.png) Bunsen Burner Auto Fixture

<!-- markdownlint-enable MD013 -->

[![Nuget](https://img.shields.io/nuget/v/BunsenBurner.AutoFixture)](https://www.nuget.org/packages/BunsenBurner.AutoFixture/)

This provides extension methods to
integrate [AutoFixture](https://github.com/AutoFixture) into the arrange/given
step.

## Getting Started

To use this library, simply include `BunsenBurner.AutoFixture.dll` in your
project
or grab
it from [NuGet](https://www.nuget.org/packages/BunsenBurner.AutoFixture/), and
add this to the top of each test `.cs` file
that needs it:

```C#
using static BunsenBurner.AutoFixture.Aaa;
```

or

```C#
using static BunsenBurner.AutoFixture.Bdd;
```

## What?

[AutoFixture](https://github.com/AutoFixture) is a randomized automatic specimen
builder with a large number of extensions.

For some tests it can reduce code and increase test coverage, while
focusing the developer on the core logic under test. (I highly recommend
reading [Mark Seemann](https://blog.ploeh.dk/2009/03/22/AnnouncingAutoFixture/)
everything he writes is great and informative)

## How to use

To start using it import the syntax that your test project is using.

```c#
using static BunsenBurner.AutoFixture.Aaa;

public class AaaTests
{
    [Fact(DisplayName = "Some test with auto arranged data")]
    public async Task Case1() =>
        // generate some T, in this case strings
        await AutoArrange(f => f.CreateMany<string>())
            // use this generated data to exercise the SUT
            .Act(d => d.Select(x => x.Length))
            // assert against the result
            .Assert(r => Assert.All(r, i => Assert.True(i > 0)));
}
```

For more information on the added arrange functions check out the code and the
tests.
