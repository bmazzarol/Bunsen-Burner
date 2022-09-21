namespace FluentTests.Tests;

using static Dsl.Bdd;

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

    private static Task<int> SomeAsyncFunction(int i) => throw new Exception("Some failure");

    [Fact(DisplayName = "Failure assertions work on async functions")]
    public async Task Case6() =>
        await Given(() => 1)
            .When(SomeAsyncFunction)
            .ThenFailsWith(
                (_, e) =>
                {
                    Assert.Equal("Some failure", e.Message);
                    return Task.CompletedTask;
                }
            );

    [Fact(DisplayName = "Failure assertions work on async functions with initial data")]
    public async Task Case7() =>
        await Given(() => 1)
            .When(SomeAsyncFunction)
            .ThenFailsWith(e =>
            {
                Assert.Equal("Some failure", e.Message);
                return Task.CompletedTask;
            });

    private static int SomeFunction(int i) => throw new Exception("Some failure");

    [Fact(DisplayName = "Failure assertions work on sync functions")]
    public async Task Case9() =>
        await Given(() => 1)
            .When(SomeFunction)
            .ThenFailsWith(e => Assert.Equal("Some failure", e.Message));

    [Fact(DisplayName = "Failure assertions work on sync functions and initial data")]
    public async Task Case10() =>
        await Given(() => 1)
            .When(SomeFunction)
            .ThenFailsWith(
                (data, e) =>
                {
                    Assert.Equal(1, data);
                    Assert.Equal("Some failure", e.Message);
                }
            )
            .And(e => Assert.NotNull(e.StackTrace));
}
