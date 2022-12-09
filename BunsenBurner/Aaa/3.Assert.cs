using System.Linq.Expressions;

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
    /// Asserts on the result of acting on the test
    /// </summary>
    /// <param name="scenario">acted on scenario</param>
    /// <param name="expression">assert expression</param>
    /// <typeparam name="TData">test data</typeparam>
    /// <typeparam name="TResult">test result</typeparam>
    /// <returns>asserted scenario</returns>
    [Pure]
    public static AaaScenario.Asserted<TData, TResult> Assert<TData, TResult>(
        this AaaScenario.Acted<TData, TResult> scenario,
        Expression<Func<TResult, bool>> expression
    ) => scenario.Assert<TData, TResult, Syntax.Aaa>(expression);

    /// <summary>
    /// Asserts on the result of acting on the test
    /// </summary>
    /// <param name="scenario">acted on scenario</param>
    /// <param name="expression">assert expression</param>
    /// <typeparam name="TData">test data</typeparam>
    /// <typeparam name="TResult">test result</typeparam>
    /// <returns>asserted scenario</returns>
    [Pure]
    public static AaaScenario.Asserted<TData, TResult> Assert<TData, TResult>(
        this AaaScenario.Acted<TData, TResult> scenario,
        Expression<Func<TData, TResult, bool>> expression
    ) => scenario.Assert<TData, TResult, Syntax.Aaa>(expression);

    /// <summary>
    /// Asserts on the result of a failure when acting
    /// </summary>
    /// <param name="scenario">acted on scenario that is expected to fail</param>
    /// <param name="fn">async assert failure function</param>
    /// <typeparam name="TData">test data</typeparam>
    /// <typeparam name="TResult">test result</typeparam>
    /// <typeparam name="TException">exception type</typeparam>
    /// <returns>asserted and failed scenario</returns>
    [Pure]
    public static AaaScenario.Asserted<TData, TException> AssertFailsWith<
        TData,
        TResult,
        TException
    >(this AaaScenario.Acted<TData, TResult> scenario, Func<TData, TException, Task> fn)
        where TException : Exception => Shared.AssertFailsWith(scenario, fn);

    /// <summary>
    /// Asserts on the result of a failure when acting
    /// </summary>
    /// <param name="scenario">acted on scenario that is expected to fail</param>
    /// <param name="fn">async assert failure function</param>
    /// <typeparam name="TData">test data</typeparam>
    /// <typeparam name="TResult">test result</typeparam>
    /// <typeparam name="TException">exception type</typeparam>
    /// <returns>asserted and failed scenario</returns>
    [Pure]
    public static AaaScenario.Asserted<TData, TException> AssertFailsWith<
        TData,
        TResult,
        TException
    >(this AaaScenario.Acted<TData, TResult> scenario, Func<TException, Task> fn)
        where TException : Exception => Shared.AssertFailsWith(scenario, fn);

    /// <summary>
    /// Asserts on the result of a failure when acting
    /// </summary>
    /// <param name="scenario">acted on scenario that is expected to fail</param>
    /// <param name="fn">assert failure function</param>
    /// <typeparam name="TData">test data</typeparam>
    /// <typeparam name="TResult">test result</typeparam>
    /// <typeparam name="TException">exception type</typeparam>
    /// <returns>asserted and failed scenario</returns>
    [Pure]
    public static AaaScenario.Asserted<TData, TException> AssertFailsWith<
        TData,
        TResult,
        TException
    >(this AaaScenario.Acted<TData, TResult> scenario, Action<TData, TException> fn)
        where TException : Exception => Shared.AssertFailsWith(scenario, fn);

    /// <summary>
    /// Asserts on the result of a failure when acting
    /// </summary>
    /// <param name="scenario">acted on scenario that is expected to fail</param>
    /// <param name="fn">assert failure function</param>
    /// <typeparam name="TData">test data</typeparam>
    /// <typeparam name="TResult">test result</typeparam>
    /// <typeparam name="TException">exception type</typeparam>
    /// <returns>asserted and failed scenario</returns>
    [Pure]
    public static AaaScenario.Asserted<TData, TException> AssertFailsWith<
        TData,
        TResult,
        TException
    >(this AaaScenario.Acted<TData, TResult> scenario, Action<TException> fn)
        where TException : Exception => Shared.AssertFailsWith(scenario, fn);

    /// <summary>
    /// Asserts on the result of a failure when acting
    /// </summary>
    /// <param name="scenario">acted on scenario that is expected to fail</param>
    /// <param name="fn">assert failure function</param>
    /// <typeparam name="TData">test data</typeparam>
    /// <typeparam name="TResult">test result</typeparam>
    /// <typeparam name="TException">exception type</typeparam>
    /// <returns>asserted and failed scenario</returns>
    [Pure]
    public static AaaScenario.Asserted<TData, TException> AssertFailsWith<
        TData,
        TResult,
        TException
    >(this AaaScenario.Acted<TData, TResult> scenario, Expression<Func<TData, TException, bool>> fn)
        where TException : Exception => Shared.AssertFailsWith(scenario, fn);

    /// <summary>
    /// Asserts on the result of a failure when acting
    /// </summary>
    /// <param name="scenario">acted on scenario that is expected to fail</param>
    /// <param name="fn">assert failure function</param>
    /// <typeparam name="TData">test data</typeparam>
    /// <typeparam name="TResult">test result</typeparam>
    /// <typeparam name="TException">exception type</typeparam>
    /// <returns>asserted and failed scenario</returns>
    [Pure]
    public static AaaScenario.Asserted<TData, TException> AssertFailsWith<
        TData,
        TResult,
        TException
    >(this AaaScenario.Acted<TData, TResult> scenario, Expression<Func<TException, bool>> fn)
        where TException : Exception => Shared.AssertFailsWith(scenario, fn);

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
    ) => AssertFailsWith<TData, TResult, Exception>(scenario, fn);

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
    ) => AssertFailsWith<TData, TResult, Exception>(scenario, fn);

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
    ) => AssertFailsWith<TData, TResult, Exception>(scenario, fn);

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
    ) => AssertFailsWith<TData, TResult, Exception>(scenario, fn);

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
        Expression<Func<TData, Exception, bool>> fn
    ) => AssertFailsWith<TData, TResult, Exception>(scenario, fn);

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
        Expression<Func<Exception, bool>> fn
    ) => AssertFailsWith<TData, TResult, Exception>(scenario, fn);

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
    /// Asserts again on the result of acting on the test
    /// </summary>
    /// <param name="scenario">asserted on scenario</param>
    /// <param name="expression">assert expression</param>
    /// <typeparam name="TData">test data</typeparam>
    /// <typeparam name="TResult">test result</typeparam>
    /// <returns>asserted scenario</returns>
    [Pure]
    public static AaaScenario.Asserted<TData, TResult> And<TData, TResult>(
        this AaaScenario.Asserted<TData, TResult> scenario,
        Expression<Func<TResult, bool>> expression
    ) => scenario.And<TData, TResult, Syntax.Aaa>(expression);

    /// <summary>
    /// Asserts again on the result of acting on the test
    /// </summary>
    /// <param name="scenario">asserted on scenario</param>
    /// <param name="expression">assert expression</param>
    /// <typeparam name="TData">test data</typeparam>
    /// <typeparam name="TResult">test result</typeparam>
    /// <returns>asserted scenario</returns>
    [Pure]
    public static AaaScenario.Asserted<TData, TResult> And<TData, TResult>(
        this AaaScenario.Asserted<TData, TResult> scenario,
        Expression<Func<TData, TResult, bool>> expression
    ) => scenario.And<TData, TResult, Syntax.Aaa>(expression);

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

    /// <summary>
    /// Resets the asserted scenario back to arranged, throwing away the act and assert information
    /// </summary>
    /// <param name="scenario">scenario</param>
    /// <typeparam name="TData">test data</typeparam>
    /// <typeparam name="TResult">test result</typeparam>
    /// <returns>arranged scenario</returns>
    [Pure]
    public static AaaScenario.Arranged<TData> ResetToArranged<TData, TResult>(
        this AaaScenario.Asserted<TData, TResult> scenario
    ) => Shared.ResetToArranged(scenario);

    /// <summary>
    /// Resets the asserted scenario back to acted, throwing away the information
    /// </summary>
    /// <param name="scenario">scenario</param>
    /// <typeparam name="TData">test data</typeparam>
    /// <typeparam name="TResult">test result</typeparam>
    /// <returns>acted scenario</returns>
    [Pure]
    public static AaaScenario.Acted<TData, TResult> ResetToActed<TData, TResult>(
        this AaaScenario.Asserted<TData, TResult> scenario
    ) => Shared.ResetToActed(scenario);

    /// <summary>
    /// Replaces the act in the scenario keeping the assertions
    /// </summary>
    /// <param name="scenario">scenario</param>
    /// <param name="fn">new act to perform</param>
    /// <typeparam name="TData">test data</typeparam>
    /// <typeparam name="TResult">test result</typeparam>
    /// <returns>asserted scenario</returns>
    [Pure]
    public static AaaScenario.Asserted<TData, TResult> ReplaceAct<TData, TResult>(
        this AaaScenario.Asserted<TData, TResult> scenario,
        Func<TData, Task<TResult>> fn
    ) => Shared.ReplaceAct(scenario, fn);

    /// <summary>
    /// Replaces the act in the scenario keeping the assertions
    /// </summary>
    /// <param name="scenario">scenario</param>
    /// <param name="fn">new act to perform</param>
    /// <typeparam name="TData">test data</typeparam>
    /// <typeparam name="TResult">test result</typeparam>
    /// <returns>asserted scenario</returns>
    [Pure]
    public static AaaScenario.Asserted<TData, TResult> ReplaceAct<TData, TResult>(
        this AaaScenario.Asserted<TData, TResult> scenario,
        Func<TData, TResult> fn
    ) => Shared.ReplaceAct(scenario, fn);
}
