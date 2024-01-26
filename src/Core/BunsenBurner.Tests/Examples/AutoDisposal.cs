using BunsenBurner.Utility;
using static BunsenBurner.AaaSyntax;

namespace BunsenBurner.Tests.Examples;

public class AutoDisposal
{
    #region Example1

    private sealed class SomeDisposableType : IDisposable
    {
        public bool IsDisposed { get; private set; }

        public void Dispose()
        {
            IsDisposed = true;
        }
    }

    [Fact(DisplayName = "Auto disposal is still standard")]
    public async Task ExampleTest1()
    {
        // disposable type without a using
        var disposableType = new SomeDisposableType();

        // wont be disposed within the DSL
        await disposableType.ArrangeData().Act(data => data.IsDisposed).Assert(Assert.False);

        // it will be disposed after
        Assert.True(disposableType.IsDisposed);
    }

    #endregion

    #region Example2

    [Fact(DisplayName = "Manual disposal is still possible")]
    public async Task ExampleTest2()
    {
        // disposable type
        var disposableType = new SomeDisposableType();

        await Arrange(
                () =>
                    // wrap it to disabled auto disposal
                    ManualDisposal.New(disposableType)
            )
            .Act(data =>
            {
                // can access it via `Value`
                var result = data.Value.IsDisposed;
                // or via implicit conversion
                SomeDisposableType resultAsWell = data;
                return result && resultAsWell.IsDisposed;
            })
            .Assert(Assert.False);

        // it will not be disposed
        Assert.False(disposableType.IsDisposed);
    }

    #endregion
}
