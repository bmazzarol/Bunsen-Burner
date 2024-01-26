namespace BunsenBurner;

internal static partial class Shared
{
    [Pure]
    internal static TestBuilder<TSyntax>.Acted<TData, TResult> Act<TData, TResult, TSyntax>(
        this TestBuilder<TSyntax>.Arranged<TData> test,
        Func<TData, Task<TResult>> fn
    )
        where TSyntax : struct, Syntax => new(test.Name, test.ArrangeStep, fn, test.Disposables);

    [Pure]
    internal static TestBuilder<TSyntax>.Acted<TData, TResult> Act<TData, TResult, TSyntax>(
        this TestBuilder<TSyntax>.Arranged<TData> test,
        Func<TData, TResult> fn
    )
        where TSyntax : struct, Syntax => test.Act(x => Task.FromResult(fn(x)));

    [Pure]
    internal static TestBuilder<TSyntax>.Acted<TData, TResultNext> And<
        TData,
        TResult,
        TResultNext,
        TSyntax
    >(
        this TestBuilder<TSyntax>.Acted<TData, TResult> test,
        Func<TData, TResult, Task<TResultNext>> fn
    )
        where TSyntax : struct, Syntax =>
        new(
            test.Name,
            test.ArrangeStep,
            async data =>
            {
                var result = await test.ActStep(data);
                var nextResult = await fn(data, result);
                return nextResult;
            },
            test.Disposables
        );

    [Pure]
    internal static TestBuilder<TSyntax>.Acted<TData, TResultNext> And<
        TData,
        TResult,
        TResultNext,
        TSyntax
    >(this TestBuilder<TSyntax>.Acted<TData, TResult> test, Func<TData, TResult, TResultNext> fn)
        where TSyntax : struct, Syntax => test.And((d, r) => Task.FromResult(fn(d, r)));

    [Pure]
    internal static TestBuilder<TSyntax>.Arranged<TData> ResetToArranged<TData, TResult, TSyntax>(
        this TestBuilder<TSyntax>.Acted<TData, TResult> test
    )
        where TSyntax : struct, Syntax => Arrange<TData, TSyntax>(test.ArrangeStep);
}
