using System.Linq.Expressions;

namespace BunsenBurner;

/// <summary>
/// Shared builder function implementations for assert steps
/// </summary>
internal static partial class Shared
{
    private static void RunExpressionAssertion<TResult>(
        this TResult result,
        Expression<Func<TResult, bool>> expression
    )
    {
        var fn = expression.Compile();
        if (!fn(result))
        {
            throw new InvalidOperationException(
                $"{expression} is not true for the result {result}"
            );
        }
    }

    private static void RunExpressionAssertion<TData, TResult>(
        TData data,
        TResult result,
        Expression<Func<TData, TResult, bool>> expression
    )
    {
        var fn = expression.Compile();
        if (!fn(data, result))
        {
            throw new InvalidOperationException(
                $"{expression} is not true for the result {result} and data {data}"
            );
        }
    }

    /// <summary>
    /// Asserts on the result of acting on the test
    /// </summary>
    /// <param name="scenario">acted on scenario</param>
    /// <param name="fn">async assert function</param>
    /// <typeparam name="TData">test data</typeparam>
    /// <typeparam name="TResult">test result</typeparam>
    /// <typeparam name="TSyntax">supported syntax</typeparam>
    /// <returns>asserted scenario</returns>
    [Pure]
    public static Scenario<TSyntax>.Asserted<TData, TResult> Assert<TData, TResult, TSyntax>(
        this Scenario<TSyntax>.Acted<TData, TResult> scenario,
        Func<TData, TResult, Task> fn
    )
        where TSyntax : struct, Syntax =>
        new(
            scenario.Name,
            scenario.ArrangeScenario,
            scenario.ActOnScenario,
            fn,
            scenario.Disposables
        );

    /// <summary>
    /// Asserts on the result of acting on the test
    /// </summary>
    /// <param name="scenario">acted on scenario</param>
    /// <param name="fn">async assert function</param>
    /// <typeparam name="TData">test data</typeparam>
    /// <typeparam name="TResult">test result</typeparam>
    /// <typeparam name="TSyntax">supported syntax</typeparam>
    /// <returns>asserted scenario</returns>
    [Pure]
    public static Scenario<TSyntax>.Asserted<TData, TResult> Assert<TData, TResult, TSyntax>(
        this Scenario<TSyntax>.Acted<TData, TResult> scenario,
        Func<TResult, Task> fn
    )
        where TSyntax : struct, Syntax => scenario.Assert((_, r) => fn(r));

    /// <summary>
    /// Asserts on the result of acting on the test
    /// </summary>
    /// <param name="scenario">acted on scenario</param>
    /// <param name="fn">assert function</param>
    /// <typeparam name="TData">test data</typeparam>
    /// <typeparam name="TResult">test result</typeparam>
    /// <typeparam name="TSyntax">supported syntax</typeparam>
    /// <returns>asserted scenario</returns>
    [Pure]
    public static Scenario<TSyntax>.Asserted<TData, TResult> Assert<TData, TResult, TSyntax>(
        this Scenario<TSyntax>.Acted<TData, TResult> scenario,
        Action<TData, TResult> fn
    )
        where TSyntax : struct, Syntax =>
        scenario.Assert(
            (d, r) =>
            {
                fn(d, r);
                return Task.CompletedTask;
            }
        );

    /// <summary>
    /// Asserts on the result of acting on the test
    /// </summary>
    /// <param name="scenario">acted on scenario</param>
    /// <param name="fn">assert function</param>
    /// <typeparam name="TData">test data</typeparam>
    /// <typeparam name="TResult">test result</typeparam>
    /// <typeparam name="TSyntax">supported syntax</typeparam>
    /// <returns>asserted scenario</returns>
    [Pure]
    public static Scenario<TSyntax>.Asserted<TData, TResult> Assert<TData, TResult, TSyntax>(
        this Scenario<TSyntax>.Acted<TData, TResult> scenario,
        Action<TResult> fn
    )
        where TSyntax : struct, Syntax => scenario.Assert((_, r) => fn(r));

    /// <summary>
    /// Asserts on the result of acting on the test
    /// </summary>
    /// <param name="scenario">acted on scenario</param>
    /// <param name="expression">assert expression</param>
    /// <typeparam name="TData">test data</typeparam>
    /// <typeparam name="TResult">test result</typeparam>
    /// <typeparam name="TSyntax">supported syntax</typeparam>
    /// <returns>asserted scenario</returns>
    [Pure]
    public static Scenario<TSyntax>.Asserted<TData, TResult> Assert<TData, TResult, TSyntax>(
        this Scenario<TSyntax>.Acted<TData, TResult> scenario,
        Expression<Func<TResult, bool>> expression
    )
        where TSyntax : struct, Syntax =>
        scenario.Assert(r => r.RunExpressionAssertion(expression));

    /// <summary>
    /// Asserts on the result of acting on the test
    /// </summary>
    /// <param name="scenario">acted on scenario</param>
    /// <param name="expression">assert expression</param>
    /// <typeparam name="TData">test data</typeparam>
    /// <typeparam name="TResult">test result</typeparam>
    /// <typeparam name="TSyntax">supported syntax</typeparam>
    /// <returns>asserted scenario</returns>
    [Pure]
    public static Scenario<TSyntax>.Asserted<TData, TResult> Assert<TData, TResult, TSyntax>(
        this Scenario<TSyntax>.Acted<TData, TResult> scenario,
        Expression<Func<TData, TResult, bool>> expression
    )
        where TSyntax : struct, Syntax =>
        scenario.Assert((d, r) => RunExpressionAssertion(d, r, expression));

    /// <summary>
    /// Asserts on the result of a failure when acting
    /// </summary>
    /// <param name="scenario">acted on scenario that is expected to fail</param>
    /// <param name="fn">async assert failure function</param>
    /// <typeparam name="TData">test data</typeparam>
    /// <typeparam name="TResult">test result</typeparam>
    /// <typeparam name="TException">exception type</typeparam>
    /// <typeparam name="TSyntax">supported syntax</typeparam>
    /// <returns>asserted and failed scenario</returns>
    [Pure]
    public static Scenario<TSyntax>.Asserted<TData, TException> AssertFailsWith<
        TData,
        TResult,
        TException,
        TSyntax
    >(this Scenario<TSyntax>.Acted<TData, TResult> scenario, Func<TData, TException, Task> fn)
        where TException : Exception
        where TSyntax : struct, Syntax =>
        new(
            scenario.Name,
            scenario.ArrangeScenario,
            async data =>
            {
                try
                {
                    await scenario.ActOnScenario(data);
                    throw new NoFailureException();
                }
                // ReSharper disable once RedundantCatchClause
                catch (NoFailureException)
                {
                    throw;
                }
                catch (TException e)
                {
                    return e;
                }
            },
            fn,
            scenario.Disposables
        );

    /// <summary>
    /// Asserts on the result of a failure when acting
    /// </summary>
    /// <param name="scenario">acted on scenario that is expected to fail</param>
    /// <param name="fn">async assert failure function</param>
    /// <typeparam name="TData">test data</typeparam>
    /// <typeparam name="TResult">test result</typeparam>
    /// <typeparam name="TException">exception type</typeparam>
    /// <typeparam name="TSyntax">supported syntax</typeparam>
    /// <returns>asserted and failed scenario</returns>
    [Pure]
    public static Scenario<TSyntax>.Asserted<TData, TException> AssertFailsWith<
        TData,
        TResult,
        TException,
        TSyntax
    >(this Scenario<TSyntax>.Acted<TData, TResult> scenario, Func<TException, Task> fn)
        where TException : Exception
        where TSyntax : struct, Syntax =>
        scenario.AssertFailsWith<TData, TResult, TException, TSyntax>((_, e) => fn(e));

    /// <summary>
    /// Asserts on the result of a failure when acting
    /// </summary>
    /// <param name="scenario">acted on scenario that is expected to fail</param>
    /// <param name="fn">assert failure function</param>
    /// <typeparam name="TData">test data</typeparam>
    /// <typeparam name="TResult">test result</typeparam>
    /// <typeparam name="TException">exception type</typeparam>
    /// <typeparam name="TSyntax">supported syntax</typeparam>
    /// <returns>asserted and failed scenario</returns>
    [Pure]
    public static Scenario<TSyntax>.Asserted<TData, TException> AssertFailsWith<
        TData,
        TResult,
        TException,
        TSyntax
    >(this Scenario<TSyntax>.Acted<TData, TResult> scenario, Action<TData, TException> fn)
        where TException : Exception
        where TSyntax : struct, Syntax =>
        scenario.AssertFailsWith<TData, TResult, TException, TSyntax>(
            (d, e) =>
            {
                fn(d, e);
                return Task.CompletedTask;
            }
        );

    /// <summary>
    /// Asserts on the result of a failure when acting
    /// </summary>
    /// <param name="scenario">acted on scenario that is expected to fail</param>
    /// <param name="fn">assert failure function</param>
    /// <typeparam name="TData">test data</typeparam>
    /// <typeparam name="TResult">test result</typeparam>
    /// <typeparam name="TException">exception type</typeparam>
    /// <typeparam name="TSyntax">supported syntax</typeparam>
    /// <returns>asserted and failed scenario</returns>
    [Pure]
    public static Scenario<TSyntax>.Asserted<TData, TException> AssertFailsWith<
        TData,
        TResult,
        TException,
        TSyntax
    >(this Scenario<TSyntax>.Acted<TData, TResult> scenario, Action<TException> fn)
        where TException : Exception
        where TSyntax : struct, Syntax =>
        scenario.AssertFailsWith<TData, TResult, TException, TSyntax>((_, e) => fn(e));

    /// <summary>
    /// Asserts on the result of a failure when acting
    /// </summary>
    /// <param name="scenario">acted on scenario that is expected to fail</param>
    /// <param name="fn">assert failure function</param>
    /// <typeparam name="TData">test data</typeparam>
    /// <typeparam name="TResult">test result</typeparam>
    /// <typeparam name="TException">exception type</typeparam>
    /// <typeparam name="TSyntax">supported syntax</typeparam>
    /// <returns>asserted and failed scenario</returns>
    [Pure]
    public static Scenario<TSyntax>.Asserted<TData, TException> AssertFailsWith<
        TData,
        TResult,
        TException,
        TSyntax
    >(
        this Scenario<TSyntax>.Acted<TData, TResult> scenario,
        Expression<Func<TData, TException, bool>> fn
    )
        where TException : Exception
        where TSyntax : struct, Syntax =>
        scenario.AssertFailsWith<TData, TResult, TException, TSyntax>(
            (d, e) => RunExpressionAssertion(d, e, fn)
        );

    /// <summary>
    /// Asserts on the result of a failure when acting
    /// </summary>
    /// <param name="scenario">acted on scenario that is expected to fail</param>
    /// <param name="fn">assert failure function</param>
    /// <typeparam name="TData">test data</typeparam>
    /// <typeparam name="TResult">test result</typeparam>
    /// <typeparam name="TException">exception type</typeparam>
    /// <typeparam name="TSyntax">supported syntax</typeparam>
    /// <returns>asserted and failed scenario</returns>
    [Pure]
    public static Scenario<TSyntax>.Asserted<TData, TException> AssertFailsWith<
        TData,
        TResult,
        TException,
        TSyntax
    >(this Scenario<TSyntax>.Acted<TData, TResult> scenario, Expression<Func<TException, bool>> fn)
        where TException : Exception
        where TSyntax : struct, Syntax =>
        scenario.AssertFailsWith<TData, TResult, TException, TSyntax>(
            (_, e) => RunExpressionAssertion(e, fn)
        );

    /// <summary>
    /// Asserts again on the result of acting on the test
    /// </summary>
    /// <param name="scenario">asserted on scenario</param>
    /// <param name="fn">async assert function</param>
    /// <typeparam name="TData">test data</typeparam>
    /// <typeparam name="TResult">test result</typeparam>
    /// <typeparam name="TSyntax">supported syntax</typeparam>
    /// <returns>asserted scenario</returns>
    [Pure]
    public static Scenario<TSyntax>.Asserted<TData, TResult> And<TData, TResult, TSyntax>(
        this Scenario<TSyntax>.Asserted<TData, TResult> scenario,
        Func<TData, TResult, Task> fn
    )
        where TSyntax : struct, Syntax =>
        new(
            scenario.Name,
            scenario.ArrangeScenario,
            scenario.ActOnScenario,
            async (data, result) =>
            {
                await scenario.AssertAgainstResult(data, result);
                await fn(data, result);
            },
            scenario.Disposables
        );

    /// <summary>
    /// Asserts again on the result of acting on the test
    /// </summary>
    /// <param name="scenario">asserted on scenario</param>
    /// <param name="fn">async assert function</param>
    /// <typeparam name="TData">test data</typeparam>
    /// <typeparam name="TResult">test result</typeparam>
    /// <typeparam name="TSyntax">supported syntax</typeparam>
    /// <returns>asserted scenario</returns>
    [Pure]
    public static Scenario<TSyntax>.Asserted<TData, TResult> And<TData, TResult, TSyntax>(
        this Scenario<TSyntax>.Asserted<TData, TResult> scenario,
        Func<TResult, Task> fn
    )
        where TSyntax : struct, Syntax => scenario.And((_, r) => fn(r));

    /// <summary>
    /// Asserts again on the result of acting on the test
    /// </summary>
    /// <param name="scenario">asserted on scenario</param>
    /// <param name="fn">assert function</param>
    /// <typeparam name="TData">test data</typeparam>
    /// <typeparam name="TResult">test result</typeparam>
    /// <typeparam name="TSyntax">supported syntax</typeparam>
    /// <returns>asserted scenario</returns>
    [Pure]
    public static Scenario<TSyntax>.Asserted<TData, TResult> And<TData, TResult, TSyntax>(
        this Scenario<TSyntax>.Asserted<TData, TResult> scenario,
        Action<TData, TResult> fn
    )
        where TSyntax : struct, Syntax =>
        scenario.And(
            (d, r) =>
            {
                fn(d, r);
                return Task.CompletedTask;
            }
        );

    /// <summary>
    /// Asserts again on the result of acting on the test
    /// </summary>
    /// <param name="scenario">asserted on scenario</param>
    /// <param name="fn">assert function</param>
    /// <typeparam name="TData">test data</typeparam>
    /// <typeparam name="TResult">test result</typeparam>
    /// <typeparam name="TSyntax">supported syntax</typeparam>
    /// <returns>asserted scenario</returns>
    [Pure]
    public static Scenario<TSyntax>.Asserted<TData, TResult> And<TData, TResult, TSyntax>(
        this Scenario<TSyntax>.Asserted<TData, TResult> scenario,
        Action<TResult> fn
    )
        where TSyntax : struct, Syntax => scenario.And((_, r) => fn(r));

    /// <summary>
    /// Asserts again on the result of acting on the test
    /// </summary>
    /// <param name="scenario">asserted on scenario</param>
    /// <param name="expression">assert expression</param>
    /// <typeparam name="TData">test data</typeparam>
    /// <typeparam name="TResult">test result</typeparam>
    /// <typeparam name="TSyntax">supported syntax</typeparam>
    /// <returns>asserted scenario</returns>
    [Pure]
    public static Scenario<TSyntax>.Asserted<TData, TResult> And<TData, TResult, TSyntax>(
        this Scenario<TSyntax>.Asserted<TData, TResult> scenario,
        Expression<Func<TResult, bool>> expression
    )
        where TSyntax : struct, Syntax => scenario.And(r => r.RunExpressionAssertion(expression));

    /// <summary>
    /// Asserts again on the result of acting on the test
    /// </summary>
    /// <param name="scenario">asserted on scenario</param>
    /// <param name="expression">assert expression</param>
    /// <typeparam name="TData">test data</typeparam>
    /// <typeparam name="TResult">test result</typeparam>
    /// <typeparam name="TSyntax">supported syntax</typeparam>
    /// <returns>asserted scenario</returns>
    [Pure]
    public static Scenario<TSyntax>.Asserted<TData, TResult> And<TData, TResult, TSyntax>(
        this Scenario<TSyntax>.Asserted<TData, TResult> scenario,
        Expression<Func<TData, TResult, bool>> expression
    )
        where TSyntax : struct, Syntax =>
        scenario.And((d, r) => RunExpressionAssertion(d, r, expression));

    /// <summary>
    /// Resets the asserted <see cref="Scenario{TSyntax}"/> back to arranged, throwing away the act and assert information
    /// </summary>
    /// <param name="scenario">scenario</param>
    /// <typeparam name="TData">test data</typeparam>
    /// <typeparam name="TResult">test result</typeparam>
    /// <typeparam name="TSyntax">supported syntax</typeparam>
    /// <returns>arranged scenario</returns>
    [Pure]
    public static Scenario<TSyntax>.Arranged<TData> ResetToArranged<TData, TResult, TSyntax>(
        this Scenario<TSyntax>.Asserted<TData, TResult> scenario
    )
        where TSyntax : struct, Syntax => Arrange<TData, TSyntax>(scenario.ArrangeScenario);

    /// <summary>
    /// Resets the asserted <see cref="Scenario{TSyntax}"/> back to acted, throwing away the information
    /// </summary>
    /// <param name="scenario">scenario</param>
    /// <typeparam name="TData">test data</typeparam>
    /// <typeparam name="TResult">test result</typeparam>
    /// <typeparam name="TSyntax">supported syntax</typeparam>
    /// <returns>acted scenario</returns>
    [Pure]
    public static Scenario<TSyntax>.Acted<TData, TResult> ResetToActed<TData, TResult, TSyntax>(
        this Scenario<TSyntax>.Asserted<TData, TResult> scenario
    )
        where TSyntax : struct, Syntax =>
        Arrange<TData, TSyntax>(scenario.ArrangeScenario).Act(scenario.ActOnScenario);

    /// <summary>
    /// Replaces the act in the <see cref="Scenario{TSyntax}"/> keeping the assertions
    /// </summary>
    /// <param name="scenario">scenario</param>
    /// <param name="fn">new act to perform</param>
    /// <typeparam name="TData">test data</typeparam>
    /// <typeparam name="TResult">test result</typeparam>
    /// <typeparam name="TSyntax">supported syntax</typeparam>
    /// <returns>asserted scenario</returns>
    [Pure]
    public static Scenario<TSyntax>.Asserted<TData, TResult> ReplaceAct<TData, TResult, TSyntax>(
        this Scenario<TSyntax>.Asserted<TData, TResult> scenario,
        Func<TData, Task<TResult>> fn
    )
        where TSyntax : struct, Syntax =>
        Arrange<TData, TSyntax>(scenario.ArrangeScenario)
            .Act(fn)
            .Assert(scenario.AssertAgainstResult);

    /// <summary>
    /// Replaces the act in the <see cref="Scenario{TSyntax}"/> keeping the assertions
    /// </summary>
    /// <param name="scenario">scenario</param>
    /// <param name="fn">new act to perform</param>
    /// <typeparam name="TData">test data</typeparam>
    /// <typeparam name="TResult">test result</typeparam>
    /// <typeparam name="TSyntax">supported syntax</typeparam>
    /// <returns>asserted scenario</returns>
    [Pure]
    public static Scenario<TSyntax>.Asserted<TData, TResult> ReplaceAct<TData, TResult, TSyntax>(
        this Scenario<TSyntax>.Asserted<TData, TResult> scenario,
        Func<TData, TResult> fn
    )
        where TSyntax : struct, Syntax =>
        Arrange<TData, TSyntax>(scenario.ArrangeScenario)
            .Act(fn)
            .Assert(scenario.AssertAgainstResult);
}
