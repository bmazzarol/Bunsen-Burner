<!-- markdownlint-disable MD013 -->

# ![Bunsen Burner](https://raw.githubusercontent.com/bmazzarol/Bunsen-Burner/main/fire-icon-small.png) Bunsen Burner xUnit.net

<!-- markdownlint-enabled MD013 -->

[![Nuget](https://img.shields.io/nuget/v/BunsenBurner.Xunit)](https://www.nuget.org/packages/BunsenBurner.Xunit/)

This provides extension methods to
integrate with [xUnit.net](https://github.com/xunit/xunit) and easily consume
Bunsen Burner.

## Getting Started

To use this library, simply include `BunsenBurner.Xunit.dll` in your
project
or grab
it from [NuGet](https://www.nuget.org/packages/BunsenBurner.Xunit/), and
add this to the top of each test `.cs` file
that needs it:

```C#
using BunsenBurner.Xunit;
```

## What?

[xUnit.net](https://github.com/xunit/xunit) is a testing framework for Dotnet.

## How to use

To start using it import the extension methods and start building tests.

For example create theory data from scenarios

```c#
// this can help with readability
using TestScenario = Scenario<Syntax.Aaa>.Asserted<int, string>;

// define the test cases here
public static TheoryData<TestScenario> TestCases = TheoryData(
    "Some test".Arrange(1).Act(x => x.ToString()).Assert(r => r == "1"),
    "Some other test".Arrange(2).Act(x => x.ToString()).Assert(r => r == "2")
);

// run them
[Theory(DisplayName = "Example running scenarios from theory data")]
[MemberData(nameof(TestCases))]
public static async Task Case4(TestScenario scenario) => await scenario;
```

For more examples check out the test project, create an issue or start a
discussion.
