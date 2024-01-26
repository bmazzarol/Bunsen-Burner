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

    /// <summary>
    /// Acts on the test data and returns a result to assert against
    /// </summary>
    /// <param name="test">arranged test</param>
    /// <param name="fn">async act function</param>
    /// <typeparam name="TData">test data</typeparam>
    /// <typeparam name="TResult">initial result of acting on the test data</typeparam>
    /// <typeparam name="TResultNext">next result of acting on the test data and last result</typeparam>
    /// <returns>acted test</returns>
    [Pure]
    public static AaaBuilder.Acted<TData, TResultNext> And<TData, TResult, TResultNext>(
        this AaaBuilder.Acted<TData, TResult> test,
        Func<TData, TResult, Task<TResultNext>> fn
    ) => Shared.And(test, fn);

    /// <summary>
    /// Acts on the test data and returns a result to assert against
    /// </summary>
    /// <param name="test">arranged test</param>
    /// <param name="fn">act function</param>
    /// <typeparam name="TData">test data</typeparam>
    /// <typeparam name="TResult">initial result of acting on the test data</typeparam>
    /// <typeparam name="TResultNext">next result of acting on the test data and last result</typeparam>
    /// <returns>acted test</returns>
    [Pure]
    public static AaaBuilder.Acted<TData, TResultNext> And<TData, TResult, TResultNext>(
        this AaaBuilder.Acted<TData, TResult> test,
        Func<TData, TResult, TResultNext> fn
    ) => Shared.And(test, fn);

    /// <summary>
    /// Resets the acted test back to arranged, throwing away the act information
    /// </summary>
    /// <param name="test">test</param>
    /// <typeparam name="TData">test data</typeparam>
    /// <typeparam name="TResult">test result</typeparam>
    /// <returns>arranged test</returns>
    [Pure]
    public static AaaBuilder.Arranged<TData> ResetToArranged<TData, TResult>(
        this AaaBuilder.Acted<TData, TResult> test
    ) => Shared.ResetToArranged(test);
}
