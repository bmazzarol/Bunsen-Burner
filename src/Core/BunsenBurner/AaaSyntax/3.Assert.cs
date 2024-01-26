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

    /// <summary>
    /// Asserts on the result of a failure when acting
    /// </summary>
    /// <param name="test">acted on test that is expected to fail</param>
    /// <param name="fn">async assert failure function</param>
    /// <typeparam name="TData">test data</typeparam>
    /// <typeparam name="TResult">test result</typeparam>
    /// <typeparam name="TException">exception type</typeparam>
    /// <returns>asserted and failed test</returns>
    [Pure]
    public static AaaBuilder.Asserted<TData, TException> AssertFailsWith<
        TData,
        TResult,
        TException
    >(this AaaBuilder.Acted<TData, TResult> test, Func<TData, TException, Task> fn)
        where TException : Exception => Shared.AssertFailsWith(test, fn);

    /// <summary>
    /// Asserts on the result of a failure when acting
    /// </summary>
    /// <param name="test">acted on test that is expected to fail</param>
    /// <param name="fn">async assert failure function</param>
    /// <typeparam name="TData">test data</typeparam>
    /// <typeparam name="TResult">test result</typeparam>
    /// <typeparam name="TException">exception type</typeparam>
    /// <returns>asserted and failed test</returns>
    [Pure]
    public static AaaBuilder.Asserted<TData, TException> AssertFailsWith<
        TData,
        TResult,
        TException
    >(this AaaBuilder.Acted<TData, TResult> test, Func<TException, Task> fn)
        where TException : Exception => Shared.AssertFailsWith(test, fn);

    /// <summary>
    /// Asserts on the result of a failure when acting
    /// </summary>
    /// <param name="test">acted on test that is expected to fail</param>
    /// <param name="fn">assert failure function</param>
    /// <typeparam name="TData">test data</typeparam>
    /// <typeparam name="TResult">test result</typeparam>
    /// <typeparam name="TException">exception type</typeparam>
    /// <returns>asserted and failed test</returns>
    [Pure]
    public static AaaBuilder.Asserted<TData, TException> AssertFailsWith<
        TData,
        TResult,
        TException
    >(this AaaBuilder.Acted<TData, TResult> test, Action<TData, TException> fn)
        where TException : Exception => Shared.AssertFailsWith(test, fn);

    /// <summary>
    /// Asserts on the result of a failure when acting
    /// </summary>
    /// <param name="test">acted on test that is expected to fail</param>
    /// <param name="fn">assert failure function</param>
    /// <typeparam name="TData">test data</typeparam>
    /// <typeparam name="TResult">test result</typeparam>
    /// <typeparam name="TException">exception type</typeparam>
    /// <returns>asserted and failed test</returns>
    [Pure]
    public static AaaBuilder.Asserted<TData, TException> AssertFailsWith<
        TData,
        TResult,
        TException
    >(this AaaBuilder.Acted<TData, TResult> test, Action<TException> fn)
        where TException : Exception => Shared.AssertFailsWith(test, fn);

    /// <summary>
    /// Asserts on the result of a failure when acting
    /// </summary>
    /// <param name="test">acted on test that is expected to fail</param>
    /// <param name="fn">assert failure function</param>
    /// <typeparam name="TData">test data</typeparam>
    /// <typeparam name="TResult">test result</typeparam>
    /// <typeparam name="TException">exception type</typeparam>
    /// <returns>asserted and failed test</returns>
    [Pure]
    public static AaaBuilder.Asserted<TData, TException> AssertFailsWith<
        TData,
        TResult,
        TException
    >(this AaaBuilder.Acted<TData, TResult> test, Expression<Func<TData, TException, bool>> fn)
        where TException : Exception => Shared.AssertFailsWith(test, fn);

    /// <summary>
    /// Asserts on the result of a failure when acting
    /// </summary>
    /// <param name="test">acted on test that is expected to fail</param>
    /// <param name="fn">assert failure function</param>
    /// <typeparam name="TData">test data</typeparam>
    /// <typeparam name="TResult">test result</typeparam>
    /// <typeparam name="TException">exception type</typeparam>
    /// <returns>asserted and failed test</returns>
    [Pure]
    public static AaaBuilder.Asserted<TData, TException> AssertFailsWith<
        TData,
        TResult,
        TException
    >(this AaaBuilder.Acted<TData, TResult> test, Expression<Func<TException, bool>> fn)
        where TException : Exception => Shared.AssertFailsWith(test, fn);

    /// <summary>
    /// Asserts on the result of a failure when acting
    /// </summary>
    /// <param name="test">acted on test that is expected to fail</param>
    /// <param name="fn">async assert failure function</param>
    /// <typeparam name="TData">test data</typeparam>
    /// <typeparam name="TResult">test result</typeparam>
    /// <returns>asserted and failed test</returns>
    [Pure]
    public static AaaBuilder.Asserted<TData, Exception> AssertFailsWith<TData, TResult>(
        this AaaBuilder.Acted<TData, TResult> test,
        Func<TData, Exception, Task> fn
    ) => AssertFailsWith<TData, TResult, Exception>(test, fn);

    /// <summary>
    /// Asserts on the result of a failure when acting
    /// </summary>
    /// <param name="test">acted on test that is expected to fail</param>
    /// <param name="fn">async assert failure function</param>
    /// <typeparam name="TData">test data</typeparam>
    /// <typeparam name="TResult">test result</typeparam>
    /// <returns>asserted and failed test</returns>
    [Pure]
    public static AaaBuilder.Asserted<TData, Exception> AssertFailsWith<TData, TResult>(
        this AaaBuilder.Acted<TData, TResult> test,
        Func<Exception, Task> fn
    ) => AssertFailsWith<TData, TResult, Exception>(test, fn);

    /// <summary>
    /// Asserts on the result of a failure when acting
    /// </summary>
    /// <param name="test">acted on test that is expected to fail</param>
    /// <param name="fn">assert failure function</param>
    /// <typeparam name="TData">test data</typeparam>
    /// <typeparam name="TResult">test result</typeparam>
    /// <returns>asserted and failed test</returns>
    [Pure]
    public static AaaBuilder.Asserted<TData, Exception> AssertFailsWith<TData, TResult>(
        this AaaBuilder.Acted<TData, TResult> test,
        Action<TData, Exception> fn
    ) => AssertFailsWith<TData, TResult, Exception>(test, fn);

    /// <summary>
    /// Asserts on the result of a failure when acting
    /// </summary>
    /// <param name="test">acted on test that is expected to fail</param>
    /// <param name="fn">assert failure function</param>
    /// <typeparam name="TData">test data</typeparam>
    /// <typeparam name="TResult">test result</typeparam>
    /// <returns>asserted and failed test</returns>
    [Pure]
    public static AaaBuilder.Asserted<TData, Exception> AssertFailsWith<TData, TResult>(
        this AaaBuilder.Acted<TData, TResult> test,
        Action<Exception> fn
    ) => AssertFailsWith<TData, TResult, Exception>(test, fn);

    /// <summary>
    /// Asserts on the result of a failure when acting
    /// </summary>
    /// <param name="test">acted on test that is expected to fail</param>
    /// <param name="fn">assert failure function</param>
    /// <typeparam name="TData">test data</typeparam>
    /// <typeparam name="TResult">test result</typeparam>
    /// <returns>asserted and failed test</returns>
    [Pure]
    public static AaaBuilder.Asserted<TData, Exception> AssertFailsWith<TData, TResult>(
        this AaaBuilder.Acted<TData, TResult> test,
        Expression<Func<TData, Exception, bool>> fn
    ) => AssertFailsWith<TData, TResult, Exception>(test, fn);

    /// <summary>
    /// Asserts on the result of a failure when acting
    /// </summary>
    /// <param name="test">acted on test that is expected to fail</param>
    /// <param name="fn">assert failure function</param>
    /// <typeparam name="TData">test data</typeparam>
    /// <typeparam name="TResult">test result</typeparam>
    /// <returns>asserted and failed test</returns>
    [Pure]
    public static AaaBuilder.Asserted<TData, Exception> AssertFailsWith<TData, TResult>(
        this AaaBuilder.Acted<TData, TResult> test,
        Expression<Func<Exception, bool>> fn
    ) => AssertFailsWith<TData, TResult, Exception>(test, fn);

    /// <summary>
    /// Asserts again on the result of acting on the test
    /// </summary>
    /// <param name="test">asserted on test</param>
    /// <param name="fn">async assert function</param>
    /// <typeparam name="TData">test data</typeparam>
    /// <typeparam name="TResult">test result</typeparam>
    /// <returns>asserted test</returns>
    [Pure]
    public static AaaBuilder.Asserted<TData, TResult> And<TData, TResult>(
        this AaaBuilder.Asserted<TData, TResult> test,
        Func<TData, TResult, Task> fn
    ) => Shared.And(test, fn);

    /// <summary>
    /// Asserts again on the result of acting on the test
    /// </summary>
    /// <param name="test">asserted on test</param>
    /// <param name="fn">async assert function</param>
    /// <typeparam name="TData">test data</typeparam>
    /// <typeparam name="TResult">test result</typeparam>
    /// <returns>asserted test</returns>
    [Pure]
    public static AaaBuilder.Asserted<TData, TResult> And<TData, TResult>(
        this AaaBuilder.Asserted<TData, TResult> test,
        Func<TResult, Task> fn
    ) => Shared.And(test, fn);

    /// <summary>
    /// Asserts again on the result of acting on the test
    /// </summary>
    /// <param name="test">asserted on test</param>
    /// <param name="fn">assert function</param>
    /// <typeparam name="TData">test data</typeparam>
    /// <typeparam name="TResult">test result</typeparam>
    /// <returns>asserted test</returns>
    [Pure]
    public static AaaBuilder.Asserted<TData, TResult> And<TData, TResult>(
        this AaaBuilder.Asserted<TData, TResult> test,
        Action<TData, TResult> fn
    ) => Shared.And(test, fn);

    /// <summary>
    /// Asserts again on the result of acting on the test
    /// </summary>
    /// <param name="test">asserted on test</param>
    /// <param name="fn">assert function</param>
    /// <typeparam name="TData">test data</typeparam>
    /// <typeparam name="TResult">test result</typeparam>
    /// <returns>asserted test</returns>
    [Pure]
    public static AaaBuilder.Asserted<TData, TResult> And<TData, TResult>(
        this AaaBuilder.Asserted<TData, TResult> test,
        Action<TResult> fn
    ) => Shared.And(test, fn);

    /// <summary>
    /// Asserts again on the result of acting on the test
    /// </summary>
    /// <param name="test">asserted on test</param>
    /// <param name="expression">assert expression</param>
    /// <typeparam name="TData">test data</typeparam>
    /// <typeparam name="TResult">test result</typeparam>
    /// <returns>asserted test</returns>
    [Pure]
    public static AaaBuilder.Asserted<TData, TResult> And<TData, TResult>(
        this AaaBuilder.Asserted<TData, TResult> test,
        Expression<Func<TResult, bool>> expression
    ) => test.And<TData, TResult, Syntax.Aaa>(expression);

    /// <summary>
    /// Asserts again on the result of acting on the test
    /// </summary>
    /// <param name="test">asserted on test</param>
    /// <param name="expression">assert expression</param>
    /// <typeparam name="TData">test data</typeparam>
    /// <typeparam name="TResult">test result</typeparam>
    /// <returns>asserted test</returns>
    [Pure]
    public static AaaBuilder.Asserted<TData, TResult> And<TData, TResult>(
        this AaaBuilder.Asserted<TData, TResult> test,
        Expression<Func<TData, TResult, bool>> expression
    ) => test.And<TData, TResult, Syntax.Aaa>(expression);

    /// <summary>
    /// Awaiter for a test so it can be run
    /// </summary>
    /// <param name="test">test to run</param>
    /// <typeparam name="TData">context</typeparam>
    /// <typeparam name="TResult">result</typeparam>
    /// <returns>awaiter</returns>
    public static TaskAwaiter GetAwaiter<TData, TResult>(
        this AaaBuilder.Asserted<TData, TResult> test
    ) => test.Run().GetAwaiter();

    /// <summary>
    /// Resets the asserted test back to arranged, throwing away the act and assert information
    /// </summary>
    /// <param name="test">test</param>
    /// <typeparam name="TData">test data</typeparam>
    /// <typeparam name="TResult">test result</typeparam>
    /// <returns>arranged test</returns>
    [Pure]
    public static AaaBuilder.Arranged<TData> ResetToArranged<TData, TResult>(
        this AaaBuilder.Asserted<TData, TResult> test
    ) => Shared.ResetToArranged(test);

    /// <summary>
    /// Resets the asserted test back to acted, throwing away the information
    /// </summary>
    /// <param name="test">test</param>
    /// <typeparam name="TData">test data</typeparam>
    /// <typeparam name="TResult">test result</typeparam>
    /// <returns>acted test</returns>
    [Pure]
    public static AaaBuilder.Acted<TData, TResult> ResetToActed<TData, TResult>(
        this AaaBuilder.Asserted<TData, TResult> test
    ) => Shared.ResetToActed(test);

    /// <summary>
    /// Replaces the act in the test keeping the assertions
    /// </summary>
    /// <param name="test">test</param>
    /// <param name="fn">new act to perform</param>
    /// <typeparam name="TData">test data</typeparam>
    /// <typeparam name="TResult">test result</typeparam>
    /// <returns>asserted test</returns>
    [Pure]
    public static AaaBuilder.Asserted<TData, TResult> ReplaceAct<TData, TResult>(
        this AaaBuilder.Asserted<TData, TResult> test,
        Func<TData, Task<TResult>> fn
    ) => Shared.ReplaceAct(test, fn);

    /// <summary>
    /// Replaces the act in the test keeping the assertions
    /// </summary>
    /// <param name="test">test</param>
    /// <param name="fn">new act to perform</param>
    /// <typeparam name="TData">test data</typeparam>
    /// <typeparam name="TResult">test result</typeparam>
    /// <returns>asserted test</returns>
    [Pure]
    public static AaaBuilder.Asserted<TData, TResult> ReplaceAct<TData, TResult>(
        this AaaBuilder.Asserted<TData, TResult> test,
        Func<TData, TResult> fn
    ) => Shared.ReplaceAct(test, fn);
}
