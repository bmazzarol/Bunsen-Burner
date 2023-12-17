using System.Diagnostics.CodeAnalysis;

namespace BunsenBurner.Bogus.Tests;

using static Aaa;

[ExcludeFromCodeCoverage]
internal class Person
{
    public string? Name { get; set; }
    public int? Age { get; set; }
    public DateTimeOffset? Dob { get; set; }
}

public class AaaTests
{
    [Fact(DisplayName = "async auto arrange works")]
    public async Task Case1() =>
        await AutoArrange(f => Task.FromResult(f.Random.String()))
            .Act(_ => _)
            .Assert(Assert.NotEmpty);

    [Fact(DisplayName = "sync auto arrange works")]
    public async Task Case2() =>
        await AutoArrange(f => f.Random.String()).Act(_ => _).Assert(Assert.NotEmpty);

    [Fact(DisplayName = "async auto arrange with builder works")]
    public async Task Case3() =>
        await AutoArrange<Person>(
                f =>
                    Task.FromResult(
                        f.RuleFor(x => x.Name, x => x.Name.FirstName())
                            .RuleFor(x => x.Age, x => x.Random.Int(20, 50))
                            .RuleFor(x => x.Dob, x => x.Person.DateOfBirth)
                            .Generate()
                    )
            )
            .Act(_ => _)
            .Assert(Assert.NotNull);

    [Fact(DisplayName = "sync auto arrange with builder works")]
    public async Task Case4() =>
        await AutoArrange<Person>(
                f =>
                    f.RuleFor(x => x.Name, x => x.Name.FirstName())
                        .RuleFor(x => x.Age, x => x.Random.Int(20, 50))
                        .RuleFor(x => x.Dob, x => x.Person.DateOfBirth)
                        .Generate()
            )
            .Act(_ => _)
            .Assert(Assert.NotNull);

    [Fact(DisplayName = "async auto arrange works with description")]
    public async Task Case5() =>
        await "some description"
            .AutoArrange(f => Task.FromResult(f.Random.String()))
            .Act(_ => _)
            .Assert(Assert.NotEmpty);

    [Fact(DisplayName = "sync auto arrange works with description")]
    public async Task Case6() =>
        await "some description"
            .AutoArrange(f => f.Random.String())
            .Act(_ => _)
            .Assert(Assert.NotEmpty);

    [Fact(DisplayName = "async auto arrange with builder works with description")]
    public async Task Case7() =>
        await "some description"
            .AutoArrange<Person>(
                f =>
                    Task.FromResult(
                        f.RuleFor(x => x.Name, x => x.Name.FirstName())
                            .RuleFor(x => x.Age, x => x.Random.Int(20, 50))
                            .RuleFor(x => x.Dob, x => x.Person.DateOfBirth)
                            .Generate()
                    )
            )
            .Act(_ => _)
            .Assert(Assert.NotNull);

    [Fact(DisplayName = "sync auto arrange with builder works with description")]
    public async Task Case8() =>
        await "some description"
            .AutoArrange<Person>(
                f =>
                    f.RuleFor(x => x.Name, x => x.Name.FirstName())
                        .RuleFor(x => x.Age, x => x.Random.Int(20, 50))
                        .RuleFor(x => x.Dob, x => x.Person.DateOfBirth)
            )
            .Act(_ => _)
            .Assert(Assert.NotNull);
}
