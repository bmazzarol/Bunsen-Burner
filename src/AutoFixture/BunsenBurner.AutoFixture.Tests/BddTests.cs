namespace BunsenBurner.AutoFixture.Tests;

using static Bdd;

public class BddTests
{
    [Fact(DisplayName = "async auto arrange works")]
    public async Task Case1() =>
        await AutoGiven(f => Task.FromResult(f.CreateMany<string>()))
            .When(d => d.Select(x => x.Length))
            .Then(r => Assert.All(r, i => Assert.True(i > 0)));

    [Fact(DisplayName = "auto arrange with builder works")]
    public async Task Case2() =>
        await AutoGiven(f => new Person(f.Create<string>(), 30, f.Create<DateTimeOffset>()))
            .When(p => p)
            .Then(r => Assert.Equal(30, r.Age));

    [Fact(DisplayName = "async auto arrange works")]
    public async Task Case3() =>
        await "some description"
            .AutoGiven(f => Task.FromResult(f.CreateMany<string>()))
            .When(d => d.Select(x => x.Length))
            .Then(r => Assert.All(r, i => Assert.True(i > 0)));

    [Fact(DisplayName = "auto arrange with builder works")]
    public async Task Case4() =>
        await "some description"
            .AutoGiven(f => new Person(f.Create<string>(), 30, f.Create<DateTimeOffset>()))
            .When(p => p)
            .Then(r => Assert.Equal(30, r.Age));
}
