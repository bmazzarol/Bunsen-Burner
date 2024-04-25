using System.Diagnostics.CodeAnalysis;
using BunsenBurner.Utility;

namespace BunsenBurner.Tests;

[SuppressMessage("Major Code Smell", "S3881:\"IDisposable\" should be implemented correctly")]
sealed class TestDisposable : IDisposable
{
    public bool IsDisposed;

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
        await disposable.Arrange().Act(t => t).Assert(r => !r.IsDisposed);
        Assert.True(disposable.IsDisposed);
    }

    [Fact(DisplayName = "Manual disposal protects from automatic disposal")]
    public static async Task Case2()
    {
        var disposable = new TestDisposable();
        await ManualDisposal
            .New(disposable)
            .Arrange()
            .Act(d => d)
            .Assert(r => !((TestDisposable)r).IsDisposed);
        Assert.False(disposable.IsDisposed);
    }
}
