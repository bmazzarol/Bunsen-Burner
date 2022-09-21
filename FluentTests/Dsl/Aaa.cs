namespace FluentTests.Dsl;

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
    public static Scenario.Arranged<TData> Arrange<TData>(Func<Task<TData>> fn) => new(default, fn);

    /// <summary>
    /// Arranges the test data
    /// </summary>
    /// <param name="fn">function returning test data</param>
    /// <typeparam name="TData">data required to act on the test</typeparam>
    /// <returns>arranged scenario</returns>
    public static Scenario.Arranged<TData> Arrange<TData>(Func<TData> fn) =>
        Arrange(() => Task.FromResult(fn()));

    /// <summary>
    /// Arranges the test data
    /// </summary>
    /// <param name="name">name/description for the test</param>
    /// <param name="fn">async function returning test data</param>
    /// <typeparam name="TData">data required to act on the test</typeparam>
    /// <returns>arranged scenario</returns>
    public static Scenario.Arranged<TData> Arrange<TData>(this string name, Func<Task<TData>> fn) =>
        new(name, fn);

    /// <summary>
    /// Arranges the test data
    /// </summary>
    /// <param name="name">name/description for the test</param>
    /// <param name="fn">function returning test data</param>
    /// <typeparam name="TData">data required to act on the test</typeparam>
    /// <returns>arranged scenario</returns>
    public static Scenario.Arranged<TData> Arrange<TData>(this string name, Func<TData> fn) =>
        name.Arrange(() => Task.FromResult(fn()));

    /// <summary>
    /// Acts on the test data and returns a result to assert against
    /// </summary>
    /// <param name="scenario">arranged scenario</param>
    /// <param name="fn">async act function</param>
    /// <typeparam name="TData">test data</typeparam>
    /// <typeparam name="TResult">result of acting on the test data</typeparam>
    /// <returns>acted scenario</returns>
    public static Scenario.Acted<TData, TResult> Act<TData, TResult>(
        this Scenario.Arranged<TData> scenario,
        Func<TData, Task<TResult>> fn
    ) => new(scenario.Name, scenario.ArrangeScenario, fn);

    /// <summary>
    /// Acts on the test data and returns a result to assert against
    /// </summary>
    /// <param name="scenario">arranged scenario</param>
    /// <param name="fn">async act function</param>
    /// <typeparam name="TData">test data</typeparam>
    /// <typeparam name="TResult">result of acting on the test data</typeparam>
    /// <returns>acted scenario</returns>
    public static Scenario.Acted<TData, TResult> Act<TData, TResult>(
        this Scenario.Arranged<TData> scenario,
        Func<TData, TResult> fn
    ) => scenario.Act(x => Task.FromResult(fn(x)));

    /// <summary>
    /// Asserts on the result of acting on the test
    /// </summary>
    /// <param name="scenario">acted on scenario</param>
    /// <param name="fn">async assert function</param>
    /// <typeparam name="TData">test data</typeparam>
    /// <typeparam name="TResult">test result</typeparam>
    /// <returns>asserted scenario</returns>
    public static Scenario.Asserted<TData, TResult> Assert<TData, TResult>(
        this Scenario.Acted<TData, TResult> scenario,
        Func<TData, TResult, Task> fn
    ) => new(scenario.Name, scenario.ArrangeScenario, scenario.ActOnScenario, fn);

    /// <summary>
    /// Asserts on the result of acting on the test
    /// </summary>
    /// <param name="scenario">acted on scenario</param>
    /// <param name="fn">async assert function</param>
    /// <typeparam name="TData">test data</typeparam>
    /// <typeparam name="TResult">test result</typeparam>
    /// <returns>asserted scenario</returns>
    public static Scenario.Asserted<TData, TResult> Assert<TData, TResult>(
        this Scenario.Acted<TData, TResult> scenario,
        Func<TResult, Task> fn
    ) => scenario.Assert((_, r) => fn(r));

    /// <summary>
    /// Asserts on the result of acting on the test
    /// </summary>
    /// <param name="scenario">acted on scenario</param>
    /// <param name="fn">assert function</param>
    /// <typeparam name="TData">test data</typeparam>
    /// <typeparam name="TResult">test result</typeparam>
    /// <returns>asserted scenario</returns>
    public static Scenario.Asserted<TData, TResult> Assert<TData, TResult>(
        this Scenario.Acted<TData, TResult> scenario,
        Action<TData, TResult> fn
    ) =>
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
    /// <returns>asserted scenario</returns>
    public static Scenario.Asserted<TData, TResult> Assert<TData, TResult>(
        this Scenario.Acted<TData, TResult> scenario,
        Action<TResult> fn
    ) => scenario.Assert((_, r) => fn(r));
}
