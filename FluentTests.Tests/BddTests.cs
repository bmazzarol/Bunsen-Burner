namespace FluentTests.Tests;
using static Dsl.Bdd;
using static Dsl.Shared;

public class BddTests
{
    [Fact(DisplayName = "Async methods operate correctly")]
    public async Task Case1() =>
        await Given(() => Task.FromResult(1))
            .And(x => Task.FromResult(x.ToString()))
            .When(x => Task.FromResult(x.Length))
            .And((_, r) => Task.FromResult(r + 1))
            .Then(
                (_, r) =>
                {
                    Assert.Equal(2, r);
                    return Task.CompletedTask;
                }
            );

    [Fact(DisplayName = "Sync methods operate correctly")]
    public async Task Case2() =>
        await Given(() => 1)
            .And(x => x.ToString())
            .When(x => x.Length)
            .And((_, r) => r + 1)
            .Then((_, r) => Assert.Equal(2, r));

    [Fact(DisplayName = "Mixing sync and async methods operate correctly")]
    public async Task Case3() =>
        await "Some description"
            .Given(() => 2)
            .When(x => Task.FromResult(x.ToString()))
            .Then(r => Assert.Equal("2", r));

    [Fact(DisplayName = "Mixing sync and async methods operate correctly")]
    public async Task Case4() =>
        await "Some other description"
            .Given(() => Task.FromResult(2))
            .When(x => x.ToString())
            .Then(r =>
            {
                Assert.Equal("2", r);
                return Task.CompletedTask;
            });
}
