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

        // won't be disposed within the DSL
        await disposableType.Arrange().Act(data => data.IsDisposed).Assert(Assert.False);

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

        await disposableType
            .Arrange()
            .Act(data =>
            {
                // can access it via `Value`
                var result = data.IsDisposed;
                // or via implicit conversion
                SomeDisposableType resultAsWell = data;
                return result && resultAsWell.IsDisposed;
            })
            .Assert(Assert.False)
            // disable auto-disposal
            .NoDisposal();

        // it will not be disposed
        Assert.False(disposableType.IsDisposed);
    }

    #endregion
}
