namespace BunsenBurner;

internal static partial class Shared
{
    [Pure]
    internal static TestBuilder<TSyntax>.Acted<TData, TResult> Act<TData, TResult, TSyntax>(
        this TestBuilder<TSyntax>.Arranged<TData> test,
        Func<TData, Task<TResult>> fn
    )
        where TSyntax : struct, Syntax => new(test.ArrangeStep, fn, test.Disposables);

    [Pure]
    internal static TestBuilder<TSyntax>.Acted<TData, TResult> Act<TData, TResult, TSyntax>(
        this TestBuilder<TSyntax>.Arranged<TData> test,
        Func<TData, TResult> fn
    )
        where TSyntax : struct, Syntax => test.Act(x => Task.FromResult(fn(x)));
}
