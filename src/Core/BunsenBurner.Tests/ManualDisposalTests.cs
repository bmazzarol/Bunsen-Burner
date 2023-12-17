using BunsenBurner.Utility;

namespace BunsenBurner.Tests;

class TestDisposable : IDisposable
{
    public bool IsDisposed;

    public TestDisposable() => IsDisposed = false;

    public void Dispose()
    {
        IsDisposed = true;
    }
}

public static class ManualDisposalTests
{
    [Fact(DisplayName = "Disposal is automatic")]
    public static async Task Case1()
    {
        var disposable = new TestDisposable();
        await disposable.ArrangeData().Act(_ => _).Assert(r => !r.IsDisposed);
        Assert.True(disposable.IsDisposed);
    }

    [Fact(DisplayName = "Manual disposal protects from automatic disposal")]
    public static async Task Case2()
    {
        var disposable = new TestDisposable();
        await ManualDisposal
            .New(disposable)
            .ArrangeData()
            .Act(_ => _)
            .Assert(r => !((TestDisposable)r).IsDisposed);
        Assert.False(disposable.IsDisposed);
    }
}
