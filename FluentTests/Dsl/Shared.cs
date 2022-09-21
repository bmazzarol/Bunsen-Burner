using System.Runtime.CompilerServices;

namespace FluentTests.Dsl;

public static class Shared
{
    /// <summary>
    /// Allows for additional arranging of test data
    /// </summary>
    /// <param name="scenario">arranged scenario</param>
    /// <param name="fn">function transforming test data into test data</param>
    /// <typeparam name="TData">initial data required to act on the test</typeparam>
    /// <typeparam name="TDataNext">next data required to act on the test</typeparam>
    /// <returns>arranged scenario</returns>
    public static Scenario.Arranged<TDataNext> And<TData, TDataNext>(
        this Scenario.Arranged<TData> scenario,
        Func<TData, TDataNext> fn
    ) => scenario.And(x => Task.FromResult(fn(x)));

    /// <summary>
    /// Allows for additional arranging of test data
    /// </summary>
    /// <param name="scenario">arranged scenario</param>
    /// <param name="fn">async function transforming test data into test data</param>
    /// <typeparam name="TData">initial data required to act on the test</typeparam>
    /// <typeparam name="TDataNext">next data required to act on the test</typeparam>
    /// <returns>arranged scenario</returns>
    public static Scenario.Arranged<TDataNext> And<TData, TDataNext>(
        this Scenario.Arranged<TData> scenario,
        Func<TData, Task<TDataNext>> fn
    ) => new(scenario.Name, async () => await fn(await scenario.ArrangeScenario()));

    /// <summary>
    /// Acts on the test data and returns a result to assert against
    /// </summary>
    /// <param name="scenario">arranged scenario</param>
    /// <param name="fn">async act function</param>
    /// <typeparam name="TData">test data</typeparam>
    /// <typeparam name="TResult">initial result of acting on the test data</typeparam>
    /// <typeparam name="TResultNext">next result of acting on the test data and last result</typeparam>
    /// <returns>acted scenario</returns>
    public static Scenario.Acted<TData, TResultNext> And<TData, TResult, TResultNext>(
        this Scenario.Acted<TData, TResult> scenario,
        Func<TData, TResult, Task<TResultNext>> fn
    ) =>
        new(
            scenario.Name,
            scenario.ArrangeScenario,
            async data => await fn(data, await scenario.ActOnScenario(data))
        );

    /// <summary>
    /// Acts on the test data and returns a result to assert against
    /// </summary>
    /// <param name="scenario">arranged scenario</param>
    /// <param name="fn">act function</param>
    /// <typeparam name="TData">test data</typeparam>
    /// <typeparam name="TResult">initial result of acting on the test data</typeparam>
    /// <typeparam name="TResultNext">next result of acting on the test data and last result</typeparam>
    /// <returns>acted scenario</returns>
    public static Scenario.Acted<TData, TResultNext> And<TData, TResult, TResultNext>(
        this Scenario.Acted<TData, TResult> scenario,
        Func<TData, TResult, TResultNext> fn
    ) => scenario.And((d, r) => Task.FromResult(fn(d, r)));

    /// <summary>
    /// Asserts again on the result of acting on the test
    /// </summary>
    /// <param name="scenario">asserted on scenario</param>
    /// <param name="fn">async assert function</param>
    /// <typeparam name="TData">test data</typeparam>
    /// <typeparam name="TResult">test result</typeparam>
    /// <returns>asserted scenario</returns>
    public static Scenario.Asserted<TData, TResult> And<TData, TResult>(
        this Scenario.Asserted<TData, TResult> scenario,
        Func<TData, TResult, Task> fn
    ) =>
        new(
            scenario.Name,
            scenario.ArrangeScenario,
            scenario.ActOnScenario,
            async (data, result) =>
            {
                await scenario.AssertAgainstResult(data, result);
                await fn(data, result);
            }
        );

    /// <summary>
    /// Asserts again on the result of acting on the test
    /// </summary>
    /// <param name="scenario">asserted on scenario</param>
    /// <param name="fn">async assert function</param>
    /// <typeparam name="TData">test data</typeparam>
    /// <typeparam name="TResult">test result</typeparam>
    /// <returns>asserted scenario</returns>
    public static Scenario.Asserted<TData, TResult> And<TData, TResult>(
        this Scenario.Asserted<TData, TResult> scenario,
        Func<TResult, Task> fn
    ) => scenario.And((_, r) => fn(r));

    /// <summary>
    /// Asserts again on the result of acting on the test
    /// </summary>
    /// <param name="scenario">asserted on scenario</param>
    /// <param name="fn">assert function</param>
    /// <typeparam name="TData">test data</typeparam>
    /// <typeparam name="TResult">test result</typeparam>
    /// <returns>asserted scenario</returns>
    public static Scenario.Asserted<TData, TResult> And<TData, TResult>(
        this Scenario.Asserted<TData, TResult> scenario,
        Action<TData, TResult> fn
    ) =>
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
    /// <returns>asserted scenario</returns>
    public static Scenario.Asserted<TData, TResult> And<TData, TResult>(
        this Scenario.Asserted<TData, TResult> scenario,
        Action<TResult> fn
    ) => scenario.And((_, r) => fn(r));

    /// <summary>
    /// Awaiter for a scenario so it can be run
    /// </summary>
    /// <param name="scenario">scenario to run</param>
    /// <typeparam name="TData">context</typeparam>
    /// <typeparam name="TResult">result</typeparam>
    /// <returns>awaiter</returns>
    public static TaskAwaiter GetAwaiter<TData, TResult>(
        this Scenario.Asserted<TData, TResult> scenario
    ) => scenario.Run().GetAwaiter();
}
