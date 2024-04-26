using System.Linq.Expressions;

namespace BunsenBurner;

public static partial class AaaSyntax
{
    /// <summary>
    /// Asserts on the result of acting on the test
    /// </summary>
    /// <param name="test">acted on test</param>
    /// <param name="fn">async assert function</param>
    /// <typeparam name="TData">test data</typeparam>
    /// <typeparam name="TResult">test result</typeparam>
    /// <returns>asserted test</returns>
    [Pure]
    public static AaaBuilder.Asserted<TData, TResult> Assert<TData, TResult>(
        this AaaBuilder.Acted<TData, TResult> test,
        Func<TData, TResult, Task> fn
    ) => Shared.Assert(test, fn);

    /// <summary>
    /// Asserts on the result of acting on the test
    /// </summary>
    /// <param name="test">acted on test</param>
    /// <param name="fn">async assert function</param>
    /// <typeparam name="TData">test data</typeparam>
    /// <typeparam name="TResult">test result</typeparam>
    /// <returns>asserted test</returns>
    [Pure]
    public static AaaBuilder.Asserted<TData, TResult> Assert<TData, TResult>(
        this AaaBuilder.Acted<TData, TResult> test,
        Func<TResult, Task> fn
    ) => Shared.Assert(test, fn);

    /// <summary>
    /// Asserts on the result of acting on the test
    /// </summary>
    /// <param name="test">acted on test</param>
    /// <param name="fn">assert function</param>
    /// <typeparam name="TData">test data</typeparam>
    /// <typeparam name="TResult">test result</typeparam>
    /// <returns>asserted test</returns>
    [Pure]
    public static AaaBuilder.Asserted<TData, TResult> Assert<TData, TResult>(
        this AaaBuilder.Acted<TData, TResult> test,
        Action<TData, TResult> fn
    ) => Shared.Assert(test, fn);

    /// <summary>
    /// Asserts on the result of acting on the test
    /// </summary>
    /// <param name="test">acted on test</param>
    /// <param name="fn">assert function</param>
    /// <typeparam name="TData">test data</typeparam>
    /// <typeparam name="TResult">test result</typeparam>
    /// <returns>asserted test</returns>
    [Pure]
    public static AaaBuilder.Asserted<TData, TResult> Assert<TData, TResult>(
        this AaaBuilder.Acted<TData, TResult> test,
        Action<TResult> fn
    ) => Shared.Assert(test, fn);

    /// <summary>
    /// Asserts on the result of acting on the test
    /// </summary>
    /// <param name="test">acted on test</param>
    /// <param name="expression">assert expression</param>
    /// <typeparam name="TData">test data</typeparam>
    /// <typeparam name="TResult">test result</typeparam>
    /// <returns>asserted test</returns>
    [Pure]
    public static AaaBuilder.Asserted<TData, TResult> Assert<TData, TResult>(
        this AaaBuilder.Acted<TData, TResult> test,
        Expression<Func<TResult, bool>> expression
    ) => test.Assert<TData, TResult, Syntax.Aaa>(expression);

    /// <summary>
    /// Asserts on the result of acting on the test
    /// </summary>
    /// <param name="test">acted on test</param>
    /// <param name="expression">assert expression</param>
    /// <typeparam name="TData">test data</typeparam>
    /// <typeparam name="TResult">test result</typeparam>
    /// <returns>asserted test</returns>
    [Pure]
    public static AaaBuilder.Asserted<TData, TResult> Assert<TData, TResult>(
        this AaaBuilder.Acted<TData, TResult> test,
        Expression<Func<TData, TResult, bool>> expression
    ) => test.Assert<TData, TResult, Syntax.Aaa>(expression);
}
