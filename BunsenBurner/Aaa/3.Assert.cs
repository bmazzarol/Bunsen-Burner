namespace BunsenBurner;

using AaaScenario = Scenario<Syntax.Aaa>;

/// <summary>
/// DSL for building tests using an arrange, act, assert syntax
/// </summary>
public static partial class Aaa
{
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
