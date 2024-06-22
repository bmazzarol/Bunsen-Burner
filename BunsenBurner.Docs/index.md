<!-- markdownlint-disable MD033 MD041 -->
<div align="center">

<img src="images/fire-icon.png" alt="Bunsen Burner" width="150px"/>

# Bunsen Burner

---

[![Nuget](https://img.shields.io/nuget/v/BunsenBurner)](https://www.nuget.org/packages/BunsenBurner/)
[![Coverage](https://sonarcloud.io/api/project_badges/measure?project=bmazzarol_Bunsen-Burner&metric=coverage)](https://sonarcloud.io/summary/new_code?id=bmazzarol_Bunsen-Burner)
[![Quality Gate Status](https://sonarcloud.io/api/project_badges/measure?project=bmazzarol_Bunsen-Burner&metric=alert_status)](https://sonarcloud.io/summary/new_code?id=bmazzarol_Bunsen-Burner)
[![CD Build](https://github.com/bmazzarol/Bunsen-Burner/actions/workflows/cd-build.yml/badge.svg)](https://github.com/bmazzarol/Bunsen-Burner/actions/workflows/cd-build.yml)
[![Check Markdown](https://github.com/bmazzarol/Bunsen-Burner/actions/workflows/check-markdown.yml/badge.svg)](https://github.com/bmazzarol/Bunsen-Burner/actions/workflows/check-markdown.yml)

Set :fire: to your old tests!
A better way to write tests in C#.

---

</div>

## Why?

Most tests in the C# are written in an arrange, act, assert style, like so,

[!code-csharp[Example1a](../BunsenBurner.Tests/Examples/GettingStarted.cs#Example1a)]

This library aims to formalize this structure in the following ways,

* Enforces that all tests must be arranged before acting and acted upon before
  assertions can occur, which is typically only convention. Now it's a compile
  time error if you don't follow this pattern
* Scaffolding tests using a fluent API can make them easier to read, write and
  refactor
* Encourages automatic refactoring of tests sections into helper methods, which
  is only possible if the test is structured using delegates
* Works with the developers IDE to provide a better experience when writing
  tests

[!code-csharp[Example1c](../BunsenBurner.Tests/Examples/GettingStarted.cs#Example1c)]
