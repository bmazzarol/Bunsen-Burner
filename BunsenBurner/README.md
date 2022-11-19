# <img height="50" src="https://raw.githubusercontent.com/bmazzarol/Bunsen-Burner/main/fire-icon.png" width="50"/> Bunsen Burner Core

[![Nuget](https://img.shields.io/nuget/v/BunsenBurner)](https://www.nuget.org/packages/BunsenBurner/)

This provides the core dependency free test abstraction that Bunsen Burner is
built on.

## Getting Started

To use this library, simply include `BunsenBurner.dll` in your project or grab
it from [NuGet](https://www.nuget.org/packages/BunsenBurner/), and add this to
the top of each test `.cs` file
that needs it:

```C#
using static BunsenBurner.Aaa; // For Arrange act assert
```

or

```C#
using static BunsenBurner.Bdd; // For Given when then
```

## What?

The type that makes this library possible is a Scenario.

This is an [ADT](https://en.wikipedia.org/wiki/Algebraic_data_type) that models
a single test. It has the following structure,

```c#
Scenario<
 // Supported Syntax, either arrange act assert (Aaa) or given when then (Bdd)
    TSyntax
>.(
 // Scenario that has been arranged and is ready to act on, returning a TData   
    Arranged<TData> | 
 // Scenario that has been acted on, taking a TData returning a TResult and is ready to assert against 
    Acted<TData, TResult> | 
 // Scenario that is asserted against a TResult. This can be run.
    Asserted<TData, TResult>)
```

This structure represents a lazy asynchronous immutable test that can be
composed and
later run.

The aim is for the type parameters to be fully inferable. There are a few edge
cases to this, but the core abstractions require no provided type parameters.

## How to use

To start using it import either the Aaa or Bdd syntax and start composing the
test.

### Arrange, Act, Assert

```c#
using static BunsenBurner.Aaa;

[Fact(DisplayName = "Some simple test")]
public async Task Case1() =>
  await Arrange(() => 1)
      .And(x => x.ToString())
      .Act(x => x.Length)
      .And((_, r) => r + 1)
      .Assert((_, r) => Assert.Equal(2, r));
```

### Given, When, Then

```c#
using static BunsenBurner.Bdd;

[Fact(DisplayName = "Some simple test")]
public async Task Case1() =>
  await Given(() => 1)
      .And(x => x.ToString())
      .When(x => x.Length)
      .And((_, r) => r + 1)
      .Then((_, r) => Assert.Equal(2, r));
```

The test is run using a custom awaiter, so async await is required.

Also as the pipeline is fully async, you need to always await a Scenario, even
if all your components are synchronous.

## Benefits

* Standardised - All tests are built on the same abstraction, it can model any
  test,
    * Must be composed in the correct sequence
        * Arrange/Given
        * Act/When
        * Assert/Then
    * And steps can be used between to allow for transformations
* Data driven - Scenarios are just data, can be passed to and from functions,
  can be put
  in a list, assigned to variables
* Composable - Both within a single test and over multiple tests
    * Write custom extension methods to extend the syntax in any direction, with
      the comfort of knowing it will make sure the developer uses it correctly.
      Just look at the other libraries provided for inspiration
    * Functional first, its just data with functions that operate on that data
* Readable - Enforced structures with no easy way to shoot yourself in the foot,
  should result in code that is easier to maintain and read
* Simple - 1 data type, and functions that operate on it
* Flexible
    * No dependencies
    * No assumptions around test frameworks
    * No assumptions around SUT
    * No requirements on the user code