namespace FluentTests.Dsl;

/// <summary>
/// DSL for building tests using a given, when, then syntax
/// </summary>
public static class Bdd
{
    /// <summary>
    /// Given the scenario data
    /// </summary>
    /// <param name="fn">async function returning scenario data</param>
    /// <typeparam name="TData">data required to when the scenario is run</typeparam>
    /// <returns>scenario with the given data</returns>
    public static Scenario.Arranged<TData> Given<TData>(Func<Task<TData>> fn) => Aaa.Arrange(fn);

    /// <summary>
    /// Given the scenario data
    /// </summary>
    /// <param name="fn">function returning scenario data</param>
    /// <typeparam name="TData">data required to when the scenario is run</typeparam>
    /// <returns>scenario with the given data</returns>
    public static Scenario.Arranged<TData> Given<TData>(Func<TData> fn) => Aaa.Arrange(fn);

    /// <summary>
    /// Given the scenario data
    /// </summary>
    /// <param name="name">name/description for the scenario</param>
    /// <param name="fn">async function returning scenario data</param>
    /// <typeparam name="TData">data required to when the scenario is run</typeparam>
    /// <returns>scenario with the given data</returns>
    public static Scenario.Arranged<TData> Given<TData>(this string name, Func<Task<TData>> fn) =>
        name.Arrange(fn);

    /// <summary>
    /// Given the scenario data
    /// </summary>
    /// <param name="name">name/description for the scenario</param>
    /// <param name="fn">async function returning scenario data</param>
    /// <typeparam name="TData">data required to when the scenario is run</typeparam>
    /// <returns>scenario with the given data</returns>
    public static Scenario.Arranged<TData> Given<TData>(this string name, Func<TData> fn) =>
        name.Arrange(fn);

    /// <summary>
    /// When the scenario is run
    /// </summary>
    /// <param name="scenario">ready to run scenario</param>
    /// <param name="fn">async when function</param>
    /// <typeparam name="TData">scenario data</typeparam>
    /// <typeparam name="TResult">result of running the scenario</typeparam>
    /// <returns>scenario that is run</returns>
    public static Scenario.Acted<TData, TResult> When<TData, TResult>(
        this Scenario.Arranged<TData> scenario,
        Func<TData, Task<TResult>> fn
    ) => scenario.Act(fn);

    /// <summary>
    /// When the scenario is run
    /// </summary>
    /// <param name="scenario">ready to run scenario</param>
    /// <param name="fn">async when function</param>
    /// <typeparam name="TData">scenario data</typeparam>
    /// <typeparam name="TResult">result of running the scenario</typeparam>
    /// <returns>scenario that is run</returns>
    public static Scenario.Acted<TData, TResult> When<TData, TResult>(
        this Scenario.Arranged<TData> scenario,
        Func<TData, TResult> fn
    ) => scenario.Act(fn);

    /// <summary>
    /// Then verify the scenario
    /// </summary>
    /// <param name="scenario">run scenario</param>
    /// <param name="fn">async then function</param>
    /// <typeparam name="TData">scenario data</typeparam>
    /// <typeparam name="TResult">result of running the scenario</typeparam>
    /// <returns>completed scenario</returns>
    public static Scenario.Asserted<TData, TResult> Then<TData, TResult>(
        this Scenario.Acted<TData, TResult> scenario,
        Func<TData, TResult, Task> fn
    ) => scenario.Assert(fn);

    /// <summary>
    /// Then verify the scenario
    /// </summary>
    /// <param name="scenario">run scenario</param>
    /// <param name="fn">async then function</param>
    /// <typeparam name="TData">scenario data</typeparam>
    /// <typeparam name="TResult">result of running the scenario</typeparam>
    /// <returns>completed scenario</returns>
    public static Scenario.Asserted<TData, TResult> Then<TData, TResult>(
        this Scenario.Acted<TData, TResult> scenario,
        Func<TResult, Task> fn
    ) => scenario.Assert(fn);

    /// <summary>
    /// Then verify the scenario
    /// </summary>
    /// <param name="scenario">run scenario</param>
    /// <param name="fn">then function</param>
    /// <typeparam name="TData">scenario data</typeparam>
    /// <typeparam name="TResult">result of running the scenario</typeparam>
    /// <returns>completed scenario</returns>
    public static Scenario.Asserted<TData, TResult> Then<TData, TResult>(
        this Scenario.Acted<TData, TResult> scenario,
        Action<TData, TResult> fn
    ) => scenario.Assert(fn);

    /// <summary>
    /// Then verify the scenario
    /// </summary>
    /// <param name="scenario">run scenario</param>
    /// <param name="fn">then function</param>
    /// <typeparam name="TData">scenario data</typeparam>
    /// <typeparam name="TResult">result of running the scenario</typeparam>
    /// <returns>completed scenario</returns>
    public static Scenario.Asserted<TData, TResult> Then<TData, TResult>(
        this Scenario.Acted<TData, TResult> scenario,
        Action<TResult> fn
    ) => scenario.Assert(fn);

    /// <summary>
    /// Then verify the scenario fails
    /// </summary>
    /// <param name="scenario">run scenario</param>
    /// <param name="fn">then on failure function</param>
    /// <typeparam name="TData">scenario data</typeparam>
    /// <typeparam name="TResult">result of running the scenario</typeparam>
    /// <returns>completed scenario</returns>
    public static Scenario.Asserted<TData, Exception> ThenFailsWith<TData, TResult>(
        this Scenario.Acted<TData, TResult> scenario,
        Func<TData, Exception, Task> fn
    ) => scenario.AssertFailsWith(fn);

    /// <summary>
    /// Then verify the scenario fails
    /// </summary>
    /// <param name="scenario">run scenario</param>
    /// <param name="fn">then on failure function</param>
    /// <typeparam name="TData">scenario data</typeparam>
    /// <typeparam name="TResult">result of running the scenario</typeparam>
    /// <returns>completed scenario</returns>
    public static Scenario.Asserted<TData, Exception> ThenFailsWith<TData, TResult>(
        this Scenario.Acted<TData, TResult> scenario,
        Func<Exception, Task> fn
    ) => scenario.AssertFailsWith(fn);

    /// <summary>
    /// Then verify the scenario fails
    /// </summary>
    /// <param name="scenario">run scenario</param>
    /// <param name="fn">then on failure function</param>
    /// <typeparam name="TData">scenario data</typeparam>
    /// <typeparam name="TResult">result of running the scenario</typeparam>
    /// <returns>completed scenario</returns>
    public static Scenario.Asserted<TData, Exception> ThenFailsWith<TData, TResult>(
        this Scenario.Acted<TData, TResult> scenario,
        Action<TData, Exception> fn
    ) => scenario.AssertFailsWith(fn);

    /// <summary>
    /// Then verify the scenario fails
    /// </summary>
    /// <param name="scenario">run scenario</param>
    /// <param name="fn">then on failure function</param>
    /// <typeparam name="TData">scenario data</typeparam>
    /// <typeparam name="TResult">result of running the scenario</typeparam>
    /// <returns>completed scenario</returns>
    public static Scenario.Asserted<TData, Exception> ThenFailsWith<TData, TResult>(
        this Scenario.Acted<TData, TResult> scenario,
        Action<Exception> fn
    ) => scenario.AssertFailsWith(fn);
}
