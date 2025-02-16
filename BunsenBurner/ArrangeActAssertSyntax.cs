using System.Linq.Expressions;
using BunsenBurner.Extensions;

namespace BunsenBurner;

using static TestBuilder<ArrangeActAssertSyntax>;

/// <summary>
/// Arrange, act, assert style syntax
/// </summary>
public readonly struct ArrangeActAssertSyntax : ISyntax<ArrangeActAssertSyntax>;

/// <summary>
/// Static class to support <see cref="ArrangeActAssertSyntax"/> keywords
/// </summary>
public static class ArrangeActAssert
{
    /// <summary>
    /// Arranges the test data
    /// </summary>
    /// <param name="fn">async function returning test data</param>
    /// <typeparam name="TData">data required to act on the test</typeparam>
    /// <returns>arranged test</returns>
    public static Arranged<TData> Arrange<TData>(Func<Task<TData>> fn) => New(fn);

    /// <summary>
    /// Arranges the test data
    /// </summary>
    /// <param name="fn">function returning test data</param>
    /// <typeparam name="TData">data required to act on the test</typeparam>
    /// <returns>arranged test</returns>
    public static Arranged<TData> Arrange<TData>(Func<TData> fn) =>
        New(() => Task.FromResult(fn()));

    /// <summary>
    /// Arranges the test data
    /// </summary>
    /// <param name="data">test data</param>
    /// <typeparam name="TData">data required to act on the test</typeparam>
    /// <returns>arranged test</returns>
    public static Arranged<TData> Arrange<TData>(this TData data) => Arrange(() => data);

    /// <summary>
    /// Acts on the test data and returns a result to assert against
    /// </summary>
    /// <param name="test">arranged test</param>
    /// <param name="fn">async act function</param>
    /// <typeparam name="TData">test data</typeparam>
    /// <typeparam name="TResult">result of acting on the test data</typeparam>
    /// <returns>acted test</returns>
    public static Acted<TData, TResult> Act<TData, TResult>(
        this Arranged<TData> test,
        Func<TData, Task<TResult>> fn
    ) => New(test.ArrangeStep, fn, test.Name);

    /// <summary>
    /// Acts on the test data and returns a result to assert against
    /// </summary>
    /// <param name="test">arranged test</param>
    /// <param name="fn">async act function</param>
    /// <typeparam name="TData">test data</typeparam>
    /// <typeparam name="TResult">result of acting on the test data</typeparam>
    /// <returns>acted test</returns>
    public static Acted<TData, TResult> Act<TData, TResult>(
        this Arranged<TData> test,
        Func<TData, TResult> fn
    ) => test.Act(data => Task.FromResult(fn(data)));

    /// <summary>
    /// Asserts on the result of acting on the test
    /// </summary>
    /// <param name="test">acted on test</param>
    /// <param name="fn">async assert function</param>
    /// <typeparam name="TData">test data</typeparam>
    /// <typeparam name="TResult">test result</typeparam>
    /// <returns>asserted test</returns>
    public static Asserted<TData, TResult> Assert<TData, TResult>(
        this Acted<TData, TResult> test,
        Func<TData, TResult, Task> fn
    ) => New(test.ArrangeStep, test.ActStep, fn, test.Name);

    /// <summary>
    /// Asserts on the result of acting on the test
    /// </summary>
    /// <param name="test">acted on test</param>
    /// <param name="fn">async assert function</param>
    /// <typeparam name="TData">test data</typeparam>
    /// <typeparam name="TResult">test result</typeparam>
    /// <returns>asserted test</returns>
    public static Asserted<TData, TResult> Assert<TData, TResult>(
        this Acted<TData, TResult> test,
        Func<TResult, Task> fn
    ) => test.Assert((_, result) => fn(result));

    /// <summary>
    /// Asserts on the result of acting on the test
    /// </summary>
    /// <param name="test">acted on test</param>
    /// <param name="fn">assert function</param>
    /// <typeparam name="TData">test data</typeparam>
    /// <typeparam name="TResult">test result</typeparam>
    /// <returns>asserted test</returns>
    public static Asserted<TData, TResult> Assert<TData, TResult>(
        this Acted<TData, TResult> test,
        Action<TData, TResult> fn
    ) =>
        test.Assert(
            (data, result) =>
            {
                fn(data, result);
                return Task.CompletedTask;
            }
        );

    /// <summary>
    /// Asserts on the result of acting on the test
    /// </summary>
    /// <param name="test">acted on test</param>
    /// <param name="fn">assert function</param>
    /// <typeparam name="TData">test data</typeparam>
    /// <typeparam name="TResult">test result</typeparam>
    /// <returns>asserted test</returns>
    public static Asserted<TData, TResult> Assert<TData, TResult>(
        this Acted<TData, TResult> test,
        Action<TResult> fn
    ) =>
        test.Assert(
            (_, result) =>
            {
                fn(result);
                return Task.CompletedTask;
            }
        );

    /// <summary>
    /// Asserts on the result of acting on the test
    /// </summary>
    /// <param name="test">acted on test</param>
    /// <param name="expression">assert expression</param>
    /// <typeparam name="TData">test data</typeparam>
    /// <typeparam name="TResult">test result</typeparam>
    /// <returns>asserted test</returns>
    public static Asserted<TData, TResult> Assert<TData, TResult>(
        this Acted<TData, TResult> test,
        Expression<Func<TResult, bool>> expression
    ) => test.Assert(expression.RunExpressionAssertion);

    /// <summary>
    /// Asserts on the result of acting on the test
    /// </summary>
    /// <param name="test">acted on test</param>
    /// <param name="expression">assert expression</param>
    /// <typeparam name="TData">test data</typeparam>
    /// <typeparam name="TResult">test result</typeparam>
    /// <returns>asserted test</returns>
    public static Asserted<TData, TResult> Assert<TData, TResult>(
        this Acted<TData, TResult> test,
        Expression<Func<TData, TResult, bool>> expression
    ) => test.Assert(expression.RunExpressionAssertion);
}
