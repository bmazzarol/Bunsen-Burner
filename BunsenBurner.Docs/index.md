# Bunsen Burner

![Bunsen Burner](images/fire-icon.png)

**Turn the heat up on your old tests.**

A test framework for C# that turns arrange-act-assert into a composable pipeline, and disposes everything the arrange step created for you.

```shell
dotnet add package BunsenBurner
```

## Why?

Most tests in C# are written in an arrange, act, assert style, like so,

[!code-csharp[Example1a](../BunsenBurner.Tests/Examples/GettingStarted.cs#Example1a)]

This structure is only a convention and is not enforced by the test framework.

This library aims to formalize this structure in the following ways,

* Enforces that all tests must be arranged before acting and acted upon before
  assertions can occur. Making it a compile-time error if this is not followed
* Scaffolding tests using a fluent API making them easier to read, write and
  refactor
* Encourages automatic refactoring of tests sections into helper methods, which
  is only possible if the test is structured using delegates
* Works with the developers IDE to provide a better experience when writing
  tests

[!code-csharp[Example1c](../BunsenBurner.Tests/Examples/GettingStarted.cs#Example1c)]
