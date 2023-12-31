namespace BunsenBurner.AutoFixture.Tests;

using static Aaa;

internal sealed record Person(string Name, int Age, DateTimeOffset Dob);

public class AaaTests
{
    [Fact(DisplayName = "async auto arrange works")]
    public async Task Case1() =>
        await AutoArrange(f => Task.FromResult(f.CreateMany<string>()))
            .Act(d => d.Select(x => x.Length))
            .Assert(r => Assert.All(r, i => Assert.True(i > 0)));

    [Fact(DisplayName = "auto arrange with builder works")]
    public async Task Case2() =>
        await AutoArrange(f => new Person(f.Create<string>(), 30, f.Create<DateTimeOffset>()))
            .Act(x => x)
            .Assert(r => Assert.Equal(30, r.Age));

    [Fact(DisplayName = "async auto arrange works")]
    public async Task Case3() =>
        await "some description"
            .AutoArrange(f => Task.FromResult(f.CreateMany<string>()))
            .Act(d => d.Select(x => x.Length))
            .Assert(r => Assert.All(r, i => Assert.True(i > 0)));

    [Fact(DisplayName = "auto arrange with builder works")]
    public async Task Case4() =>
        await "some description"
            .AutoArrange(f => new Person(f.Create<string>(), 30, f.Create<DateTimeOffset>()))
            .Act(p => p)
            .Assert(r => Assert.Equal(30, r.Age));
}
