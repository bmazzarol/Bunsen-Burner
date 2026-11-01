# Bunsen Burner

![Bunsen Burner](images/fire-icon.png)

**Arrange, act and assert, enforced by the compiler.**

A test DSL for C# that turns arrange-act-assert into a composable pipeline, and disposes everything the arrange and act stages create.

```shell
dotnet add package BunsenBurner
```

## Why?

Most tests in C# are written in an arrange, act, assert style, like so,

[!code-csharp[Example1a](../BunsenBurner.Tests/Examples/GettingStarted.cs#Example1a)]

That order is a convention. Nothing in the test framework stops a test from
asserting before it acts, or from leaking the disposables it created.

Bunsen Burner makes the convention part of the type system:

* arranging, acting and asserting in the wrong order does not compile
* each stage is a delegate, so the compiler and the IDE can lift shared code out
  of the test into reusable helpers
* everything the arrange and act stages create is disposed at the end of the test

[!code-csharp[Example1c](../BunsenBurner.Tests/Examples/GettingStarted.cs#Example1c)]

## What it does not do

* It is not a test runner. Tests are still `xUnit` `Fact`s; the DSL only
  structures them, so the runner, reporting and CI stay the same
* It does not execute anything. Each stage is a delegate the test itself calls
* It does not run tests that do not return a `Task`; the DSL is async all the way
  down
* It does not keep disposables alive. Anything disposable created in the arrange
  or act stage is disposed unless the asserted stage calls `NoDisposal`

## See also

* [Getting Started](articles/getting-started.md) - convert a test you already have
* [Arrange Act Assert](articles/arrange-act-assert.md) - how the stages chain
* [Auto Disposal](articles/auto-disposal.md) - what happens to disposable values
