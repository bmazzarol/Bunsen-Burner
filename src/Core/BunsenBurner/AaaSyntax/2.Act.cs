namespace BunsenBurner;

public static partial class AaaSyntax
{
    /// <summary>
    /// Acts on the test data and returns a result to assert against
    /// </summary>
    /// <param name="test">arranged test</param>
    /// <param name="fn">async act function</param>
    /// <typeparam name="TData">test data</typeparam>
    /// <typeparam name="TResult">result of acting on the test data</typeparam>
    /// <returns>acted test</returns>
    [Pure]
    public static AaaBuilder.Acted<TData, TResult> Act<TData, TResult>(
        this AaaBuilder.Arranged<TData> test,
        Func<TData, Task<TResult>> fn
    ) => Shared.Act(test, fn);

    /// <summary>
    /// Acts on the test data and returns a result to assert against
    /// </summary>
    /// <param name="test">arranged test</param>
    /// <param name="fn">async act function</param>
    /// <typeparam name="TData">test data</typeparam>
    /// <typeparam name="TResult">result of acting on the test data</typeparam>
    /// <returns>acted test</returns>
    [Pure]
    public static AaaBuilder.Acted<TData, TResult> Act<TData, TResult>(
        this AaaBuilder.Arranged<TData> test,
        Func<TData, TResult> fn
    ) => Shared.Act(test, fn);
}
