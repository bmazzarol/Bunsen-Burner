namespace BunsenBurner.Tests;

sealed class TestDisposable : IDisposable
{
    public bool IsDisposed;

    public void Dispose()
    {
        IsDisposed = true;
    }
}

sealed class TestAsyncDisposable : IAsyncDisposable
{
    public bool IsDisposed;

    public ValueTask DisposeAsync()
    {
        IsDisposed = true;
        return ValueTask.CompletedTask;
    }
}

public static class ManualDisposalTests
{
    [Fact(DisplayName = "Disposal is automatic")]
    public static async Task Case1()
    {
        var disposable = new TestDisposable();
        await disposable.Arrange().Act(t => t).Assert(r => !r.IsDisposed);
        Assert.True(disposable.IsDisposed);
    }

    [Fact(DisplayName = "Manual disposal protects from automatic disposal")]
    public static async Task Case2()
    {
        var disposable = new TestDisposable();
        await disposable.Arrange().Act(d => d).Assert(r => !r.IsDisposed).NoDisposal();
        Assert.False(disposable.IsDisposed);
    }

    [Fact(DisplayName = "Async disposal is automatic")]
    public static async Task Case3()
    {
        var disposable = new TestAsyncDisposable();
        await disposable.Arrange().Act(d => d).Assert(r => !r.IsDisposed);
        Assert.True(disposable.IsDisposed);
    }
}
