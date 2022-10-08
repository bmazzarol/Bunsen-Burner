namespace BunsenBurner.Tests;

using static Bdd;

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
            .Given(2)
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

    [Fact(DisplayName = "Additional And assertions work")]
    public async Task Case5() =>
        await Given(() => (a: 1, b: "c"))
            .When(x => x.a + x.b)
            .Then(
                (d, r) =>
                {
                    Assert.NotEqual(d.b, r);
                    return Task.CompletedTask;
                }
            )
            .And(
                (d, r) =>
                {
                    Assert.Equal(2, r.Length);
                    Assert.Equal(1, d.a);
                    return Task.CompletedTask;
                }
            )
            .And(r =>
            {
                Assert.NotEqual(1, r.Length);
                return Task.CompletedTask;
            })
            .And((d, r) => Assert.NotEqual(d.a, r.Length))
            .And(r => Assert.NotEqual(1, r.Length));

    private static Task<int> SomeAsyncFunction(int i) => throw new Exception("Some failure");

    [Fact(DisplayName = "Failure assertions work on async functions")]
    public async Task Case6() =>
        await "Some description"
            .Given(() => 1)
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

    [Fact(DisplayName = "Expression based assertions work")]
    public async Task Case11() =>
        await Given(() => 1).When(x => x + 2).Then(x => x > 0 && x < 4).And(x => x % 1 == 0);

    [Fact(DisplayName = "Expression based assertions that are wrong fail")]
    public async Task Case12()
    {
        var exception = await Assert.ThrowsAsync<InvalidOperationException>(
            async () => await Given(() => 1).When(x => x + 2).Then(x => x < 4).And(x => x % 1 != 0)
        );
        Assert.Equal("x => ((x % 1) != 0) is not true for the result 3", exception.Message);
    }

    [Fact(DisplayName = "Expression based assertions with data work")]
    public async Task Case13() =>
        await 1
            .GivenData()
            .When(x => x + 2)
            .Then((r, x) => r == 1 && x > 0 && x < 4)
            .And((r, x) => x % r == 0);

    [Fact(DisplayName = "Expression based assertions with data that are wrong fail")]
    public async Task Case14()
    {
        var exception = await Assert.ThrowsAsync<InvalidOperationException>(
            async () => await Given(1).When(x => x + 2).Then((r, x) => r == 2 && x > 4 && x < 6)
        );
        Assert.Equal(
            "(r, x) => (((r == 2) AndAlso (x > 4)) AndAlso (x < 6)) is not true for the result 3 and data 1",
            exception.Message
        );
    }

    [Fact(DisplayName = "Expression based assertions on failures work")]
    public async Task Case15() =>
        // ReSharper disable once IntDivisionByZero
        await 1
            .GivenData()
            .When(x => x / 0)
            .ThenFailsWith(e => e.Message == "Attempted to divide by zero.");

    [Fact(DisplayName = "Expression based assertions on failures with data work")]
    public async Task Case16() =>
        // ReSharper disable once IntDivisionByZero
        await 1
            .GivenData()
            .When(x => x / 0)
            .ThenFailsWith((r, e) => r == 1 && e.Message == "Attempted to divide by zero.");

    [Fact(DisplayName = "Expression based assertions on typed failures work")]
    public async Task Case17() =>
        // ReSharper disable once IntDivisionByZero
        await 1
            .GivenData()
            .When(x => x / 0)
            .ThenFailsWith(
                (int r, DivideByZeroException e) =>
                    r == 1 && e.Message == "Attempted to divide by zero."
            );
}
