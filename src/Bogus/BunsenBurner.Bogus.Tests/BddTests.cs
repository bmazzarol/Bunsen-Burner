namespace BunsenBurner.Bogus.Tests;

using static Bdd;

public class BddTests
{
    [Fact(DisplayName = "async auto arrange works")]
    public async Task Case1() =>
        await AutoGiven(f => Task.FromResult(f.Random.String())).When(_ => _).Then(Assert.NotEmpty);

    [Fact(DisplayName = "sync auto arrange works")]
    public async Task Case2() =>
        await AutoGiven(f => f.Random.String()).When(_ => _).Then(Assert.NotEmpty);

    [Fact(DisplayName = "async auto arrange with builder works")]
    public async Task Case3() =>
        await AutoGiven<Person>(
                f =>
                    Task.FromResult(
                        f.RuleFor(x => x.Name, x => x.Name.FirstName())
                            .RuleFor(x => x.Age, x => x.Random.Int(20, 50))
                            .RuleFor(x => x.Dob, x => x.Person.DateOfBirth)
                            .Generate()
                    )
            )
            .When(_ => _)
            .Then(Assert.NotNull);

    [Fact(DisplayName = "sync auto arrange with builder works")]
    public async Task Case4() =>
        await AutoGiven<Person>(
                f =>
                    f.RuleFor(x => x.Name, x => x.Name.FirstName())
                        .RuleFor(x => x.Age, x => x.Random.Int(20, 50))
                        .RuleFor(x => x.Dob, x => x.Person.DateOfBirth)
                        .Generate()
            )
            .When(_ => _)
            .Then(Assert.NotNull);

    [Fact(DisplayName = "async auto arrange works with description")]
    public async Task Case5() =>
        await "some description"
            .AutoGiven(f => Task.FromResult(f.Random.String()))
            .When(_ => _)
            .Then(Assert.NotEmpty);

    [Fact(DisplayName = "sync auto arrange works with description")]
    public async Task Case6() =>
        await "some description"
            .AutoGiven(f => f.Random.String())
            .When(_ => _)
            .Then(Assert.NotEmpty);

    [Fact(DisplayName = "async auto arrange with builder works with description")]
    public async Task Case7() =>
        await "some description"
            .AutoGiven<Person>(
                f =>
                    Task.FromResult(
                        f.RuleFor(x => x.Name, x => x.Name.FirstName())
                            .RuleFor(x => x.Age, x => x.Random.Int(20, 50))
                            .RuleFor(x => x.Dob, x => x.Person.DateOfBirth)
                            .Generate()
                    )
            )
            .When(_ => _)
            .Then(Assert.NotNull);

    [Fact(DisplayName = "sync auto arrange with builder works with description")]
    public async Task Case8() =>
        await "some description"
            .AutoGiven<Person>(
                f =>
                    f.RuleFor(x => x.Name, x => x.Name.FirstName())
                        .RuleFor(x => x.Age, x => x.Random.Int(20, 50))
                        .RuleFor(x => x.Dob, x => x.Person.DateOfBirth)
            )
            .When(_ => _)
            .Then(Assert.NotNull);
}
