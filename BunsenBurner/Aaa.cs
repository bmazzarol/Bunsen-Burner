using System.Runtime.CompilerServices;

namespace BunsenBurner;

using AaaScenario = Scenario<Syntax.Aaa>;

/// <summary>
/// DSL for building tests using an arrange, act, assert syntax
/// </summary>
public static class Aaa
{
    /// <summary>
    /// Arranges the test data
    /// </summary>
    /// <param name="fn">async function returning test data</param>
    /// <typeparam name="TData">data required to act on the test</typeparam>
    /// <returns>arranged scenario</returns>
    [Pure]
    public static AaaScenario.Arranged<TData> Arrange<TData>(Func<Task<TData>> fn) =>
        Shared.Arrange<TData, Syntax.Aaa>(fn);

    /// <summary>
    /// Arranges the test data
    /// </summary>
    /// <param name="fn">function returning test data</param>
    /// <typeparam name="TData">data required to act on the test</typeparam>
    /// <returns>arranged scenario</returns>
    [Pure]
    public static AaaScenario.Arranged<TData> Arrange<TData>(Func<TData> fn) =>
        Shared.Arrange<TData, Syntax.Aaa>(fn);

    /// <summary>
    /// Arranges the test data
    /// </summary>
    /// <param name="name">name/description for the test</param>
    /// <param name="fn">async function returning test data</param>
    /// <typeparam name="TData">data required to act on the test</typeparam>
    /// <returns>arranged scenario</returns>
    [Pure]
    public static AaaScenario.Arranged<TData> Arrange<TData>(
        this string name,
        Func<Task<TData>> fn
    ) => name.Arrange<TData, Syntax.Aaa>(fn);

    /// <summary>
    /// Arranges the test data
    /// </summary>
    /// <param name="name">name/description for the test</param>
    /// <param name="fn">function returning test data</param>
    /// <typeparam name="TData">data required to act on the test</typeparam>
    /// <returns>arranged scenario</returns>
    [Pure]
    public static AaaScenario.Arranged<TData> Arrange<TData>(this string name, Func<TData> fn) =>
        name.Arrange<TData, Syntax.Aaa>(fn);

    /// <summary>
    /// Allows for additional arranging of test data
    /// </summary>
    /// <param name="scenario">arranged scenario</param>
    /// <param name="fn">async function transforming test data into test data</param>
    /// <typeparam name="TData">initial data required to act on the test</typeparam>
    /// <typeparam name="TDataNext">next data required to act on the test</typeparam>
    /// <returns>arranged scenario</returns>
    [Pure]
    public static AaaScenario.Arranged<TDataNext> And<TData, TDataNext>(
        this AaaScenario.Arranged<TData> scenario,
        Func<TData, Task<TDataNext>> fn
    ) => Shared.And(scenario, fn);

    /// <summary>
    /// Allows for additional arranging of test data
    /// </summary>
    /// <param name="scenario">arranged scenario</param>
    /// <param name="fn">function transforming test data into test data</param>
    /// <typeparam name="TData">initial data required to act on the test</typeparam>
    /// <typeparam name="TDataNext">next data required to act on the test</typeparam>
    /// <returns>arranged scenario</returns>
    [Pure]
    public static AaaScenario.Arranged<TDataNext> And<TData, TDataNext>(
        this AaaScenario.Arranged<TData> scenario,
        Func<TData, TDataNext> fn
    ) => Shared.And(scenario, fn);

    /// <summary>
    /// Acts on the test data and returns a result to assert against
    /// </summary>
    /// <param name="scenario">arranged scenario</param>
    /// <param name="fn">async act function</param>
    /// <typeparam name="TData">test data</typeparam>
    /// <typeparam name="TResult">result of acting on the test data</typeparam>
    /// <returns>acted scenario</returns>
    [Pure]
    public static AaaScenario.Acted<TData, TResult> Act<TData, TResult>(
        this AaaScenario.Arranged<TData> scenario,
        Func<TData, Task<TResult>> fn
    ) => Shared.Act(scenario, fn);

    /// <summary>
    /// Acts on the test data and returns a result to assert against
    /// </summary>
    /// <param name="scenario">arranged scenario</param>
    /// <param name="fn">async act function</param>
    /// <typeparam name="TData">test data</typeparam>
    /// <typeparam name="TResult">result of acting on the test data</typeparam>
    /// <returns>acted scenario</returns>
    [Pure]
    public static AaaScenario.Acted<TData, TResult> Act<TData, TResult>(
        this AaaScenario.Arranged<TData> scenario,
        Func<TData, TResult> fn
    ) => Shared.Act(scenario, fn);

    /// <summary>
    /// Acts on the test data and returns a result to assert against
    /// </summary>
    /// <param name="scenario">arranged scenario</param>
    /// <param name="fn">async act function</param>
    /// <typeparam name="TData">test data</typeparam>
    /// <typeparam name="TResult">initial result of acting on the test data</typeparam>
    /// <typeparam name="TResultNext">next result of acting on the test data and last result</typeparam>
    /// <returns>acted scenario</returns>
    [Pure]
    public static AaaScenario.Acted<TData, TResultNext> And<TData, TResult, TResultNext>(
        this AaaScenario.Acted<TData, TResult> scenario,
        Func<TData, TResult, Task<TResultNext>> fn
    ) => Shared.And(scenario, fn);

    /// <summary>
    /// Acts on the test data and returns a result to assert against
    /// </summary>
    /// <param name="scenario">arranged scenario</param>
    /// <param name="fn">act function</param>
    /// <typeparam name="TData">test data</typeparam>
    /// <typeparam name="TResult">initial result of acting on the test data</typeparam>
    /// <typeparam name="TResultNext">next result of acting on the test data and last result</typeparam>
    /// <returns>acted scenario</returns>
    [Pure]
    public static AaaScenario.Acted<TData, TResultNext> And<TData, TResult, TResultNext>(
        this AaaScenario.Acted<TData, TResult> scenario,
        Func<TData, TResult, TResultNext> fn
    ) => Shared.And(scenario, fn);

    /// <summary>
    /// Asserts on the result of acting on the test
    /// </summary>
    /// <param name="scenario">acted on scenario</param>
    /// <param name="fn">async assert function</param>
    /// <typeparam name="TData">test data</typeparam>
    /// <typeparam name="TResult">test result</typeparam>
    /// <returns>asserted scenario</returns>
    [Pure]
    public static AaaScenario.Asserted<TData, TResult> Assert<TData, TResult>(
        this AaaScenario.Acted<TData, TResult> scenario,
        Func<TData, TResult, Task> fn
    ) => Shared.Assert(scenario, fn);

    /// <summary>
    /// Asserts on the result of acting on the test
    /// </summary>
    /// <param name="scenario">acted on scenario</param>
    /// <param name="fn">async assert function</param>
    /// <typeparam name="TData">test data</typeparam>
    /// <typeparam name="TResult">test result</typeparam>
    /// <returns>asserted scenario</returns>
    [Pure]
    public static AaaScenario.Asserted<TData, TResult> Assert<TData, TResult>(
        this AaaScenario.Acted<TData, TResult> scenario,
        Func<TResult, Task> fn
    ) => Shared.Assert(scenario, fn);

    /// <summary>
    /// Asserts on the result of acting on the test
    /// </summary>
    /// <param name="scenario">acted on scenario</param>
    /// <param name="fn">assert function</param>
    /// <typeparam name="TData">test data</typeparam>
    /// <typeparam name="TResult">test result</typeparam>
    /// <returns>asserted scenario</returns>
    [Pure]
    public static AaaScenario.Asserted<TData, TResult> Assert<TData, TResult>(
        this AaaScenario.Acted<TData, TResult> scenario,
        Action<TData, TResult> fn
    ) => Shared.Assert(scenario, fn);

    /// <summary>
    /// Asserts on the result of acting on the test
    /// </summary>
    /// <param name="scenario">acted on scenario</param>
    /// <param name="fn">assert function</param>
    /// <typeparam name="TData">test data</typeparam>
    /// <typeparam name="TResult">test result</typeparam>
    /// <returns>asserted scenario</returns>
    [Pure]
    public static AaaScenario.Asserted<TData, TResult> Assert<TData, TResult>(
        this AaaScenario.Acted<TData, TResult> scenario,
        Action<TResult> fn
    ) => Shared.Assert(scenario, fn);

    /// <summary>
    /// Asserts on the result of a failure when acting
    /// </summary>
    /// <param name="scenario">acted on scenario that is expected to fail</param>
    /// <param name="fn">async assert failure function</param>
    /// <typeparam name="TData">test data</typeparam>
    /// <typeparam name="TResult">test result</typeparam>
    /// <returns>asserted and failed scenario</returns>
    [Pure]
    public static AaaScenario.Asserted<TData, Exception> AssertFailsWith<TData, TResult>(
        this AaaScenario.Acted<TData, TResult> scenario,
        Func<TData, Exception, Task> fn
    ) => Shared.AssertFailsWith(scenario, fn);

    /// <summary>
    /// Asserts on the result of a failure when acting
    /// </summary>
    /// <param name="scenario">acted on scenario that is expected to fail</param>
    /// <param name="fn">async assert failure function</param>
    /// <typeparam name="TData">test data</typeparam>
    /// <typeparam name="TResult">test result</typeparam>
    /// <returns>asserted and failed scenario</returns>
    [Pure]
    public static AaaScenario.Asserted<TData, Exception> AssertFailsWith<TData, TResult>(
        this AaaScenario.Acted<TData, TResult> scenario,
        Func<Exception, Task> fn
    ) => Shared.AssertFailsWith(scenario, fn);

    /// <summary>
    /// Asserts on the result of a failure when acting
    /// </summary>
    /// <param name="scenario">acted on scenario that is expected to fail</param>
    /// <param name="fn">assert failure function</param>
    /// <typeparam name="TData">test data</typeparam>
    /// <typeparam name="TResult">test result</typeparam>
    /// <returns>asserted and failed scenario</returns>
    [Pure]
    public static AaaScenario.Asserted<TData, Exception> AssertFailsWith<TData, TResult>(
        this AaaScenario.Acted<TData, TResult> scenario,
        Action<TData, Exception> fn
    ) => Shared.AssertFailsWith(scenario, fn);

    /// <summary>
    /// Asserts on the result of a failure when acting
    /// </summary>
    /// <param name="scenario">acted on scenario that is expected to fail</param>
    /// <param name="fn">assert failure function</param>
    /// <typeparam name="TData">test data</typeparam>
    /// <typeparam name="TResult">test result</typeparam>
    /// <returns>asserted and failed scenario</returns>
    [Pure]
    public static AaaScenario.Asserted<TData, Exception> AssertFailsWith<TData, TResult>(
        this AaaScenario.Acted<TData, TResult> scenario,
        Action<Exception> fn
    ) => Shared.AssertFailsWith(scenario, fn);

    /// <summary>
    /// Asserts again on the result of acting on the test
    /// </summary>
    /// <param name="scenario">asserted on scenario</param>
    /// <param name="fn">async assert function</param>
    /// <typeparam name="TData">test data</typeparam>
    /// <typeparam name="TResult">test result</typeparam>
    /// <returns>asserted scenario</returns>
    [Pure]
    public static AaaScenario.Asserted<TData, TResult> And<TData, TResult>(
        this AaaScenario.Asserted<TData, TResult> scenario,
        Func<TData, TResult, Task> fn
    ) => Shared.And(scenario, fn);

    /// <summary>
    /// Asserts again on the result of acting on the test
    /// </summary>
    /// <param name="scenario">asserted on scenario</param>
    /// <param name="fn">async assert function</param>
    /// <typeparam name="TData">test data</typeparam>
    /// <typeparam name="TResult">test result</typeparam>
    /// <returns>asserted scenario</returns>
    [Pure]
    public static AaaScenario.Asserted<TData, TResult> And<TData, TResult>(
        this AaaScenario.Asserted<TData, TResult> scenario,
        Func<TResult, Task> fn
    ) => Shared.And(scenario, fn);

    /// <summary>
    /// Asserts again on the result of acting on the test
    /// </summary>
    /// <param name="scenario">asserted on scenario</param>
    /// <param name="fn">assert function</param>
    /// <typeparam name="TData">test data</typeparam>
    /// <typeparam name="TResult">test result</typeparam>
    /// <returns>asserted scenario</returns>
    [Pure]
    public static AaaScenario.Asserted<TData, TResult> And<TData, TResult>(
        this AaaScenario.Asserted<TData, TResult> scenario,
        Action<TData, TResult> fn
    ) => Shared.And(scenario, fn);

    /// <summary>
    /// Asserts again on the result of acting on the test
    /// </summary>
    /// <param name="scenario">asserted on scenario</param>
    /// <param name="fn">assert function</param>
    /// <typeparam name="TData">test data</typeparam>
    /// <typeparam name="TResult">test result</typeparam>
    /// <returns>asserted scenario</returns>
    [Pure]
    public static AaaScenario.Asserted<TData, TResult> And<TData, TResult>(
        this AaaScenario.Asserted<TData, TResult> scenario,
        Action<TResult> fn
    ) => Shared.And(scenario, fn);

    /// <summary>
    /// Awaiter for a scenario so it can be run
    /// </summary>
    /// <param name="scenario">scenario to run</param>
    /// <typeparam name="TData">context</typeparam>
    /// <typeparam name="TResult">result</typeparam>
    /// <returns>awaiter</returns>
    public static TaskAwaiter GetAwaiter<TData, TResult>(
        this AaaScenario.Asserted<TData, TResult> scenario
    ) => scenario.Run().GetAwaiter();
}
