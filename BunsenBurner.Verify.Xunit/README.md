# Bunsen Burner Verify Xunit

This provides extension methods to
integrate [Verify.Xunit](https://github.com/VerifyTests/Verify) into the
assert/then
step.

## Getting Started

To use this library, simply include `BunsenBurner.Verify.Xunit.dll` in your
project
or grab
it from NuGet (Coming Soon), and add this to the top of each test `.cs` file
that needs it:

```C#
using BunsenBurner.Verify.Xunit;
```

## What?

[Verify.Xunit](https://github.com/VerifyTests/Verify) is a snapshot tool used to
replace the assert/then stage of a test.

## How to use

To start using it import the syntax that your test project is using.

```c#
using BunsenBurner.Verify.Xunit;
using static BunsenBurner.Aaa;

[UsesVerify]
public static class AaaTests
{
    [Fact(DisplayName = "Assertion with scrubbing and projection")]
    public static async Task Case1() =>
        await Arrange(() => new { A = 1, B = "2", C = DateTimeOffset.Now })
            .Act(_ => _)
            // this will create a snapshot file under __snapshots__ in the same folder as the tests
            // it will then project and scrub the result before saving and comparing with the last
            // saved result
            .AssertResultIsUnchanged(x => new { x.A, x.C }, scrubResults: true);
}
```

For more information on the added assert/then functions check out the code and the
tests.
