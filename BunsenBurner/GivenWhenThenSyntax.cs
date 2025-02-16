using System.Linq.Expressions;
using BunsenBurner.Extensions;

namespace BunsenBurner;

using static TestBuilder<GivenWhenThenSyntax>;

/// <summary>
/// Given, when, then style tests used in Behaviour Driven Development
/// </summary>
public readonly struct GivenWhenThenSyntax : ISyntax<GivenWhenThenSyntax>;

/// <summary>
/// Static class to support <see cref="GivenWhenThenSyntax"/> keywords
/// </summary>
public static class GivenWhenThen
{
    /// <summary>
    /// Given the scenario data
    /// </summary>
    /// <param name="fn">async function returning scenario data</param>
    /// <typeparam name="TData">data required to when the scenario is run</typeparam>
    /// <returns>scenario with the given data</returns>
    public static Arranged<TData> Given<TData>(Func<Task<TData>> fn) => New(fn);

    /// <summary>
    /// Given the scenario data
    /// </summary>
    /// <param name="fn">function returning scenario data</param>
    /// <typeparam name="TData">data required to when the scenario is run</typeparam>
    /// <returns>scenario with the given data</returns>
    public static Arranged<TData> Given<TData>(Func<TData> fn) => New(() => Task.FromResult(fn()));

    /// <summary>
    /// Given the scenario data
    /// </summary>
    /// <param name="data">scenario data</param>
    /// <typeparam name="TData">data required to when the scenario is run</typeparam>
    /// <returns>scenario with the given data</returns>
    public static Arranged<TData> Given<TData>(this TData data) => Given(() => data);

    /// <summary>
    /// When the scenario is run
    /// </summary>
    /// <param name="scenario">ready to run scenario</param>
    /// <param name="fn">async when function</param>
    /// <typeparam name="TData">scenario data</typeparam>
    /// <typeparam name="TResult">result of running the scenario</typeparam>
    /// <returns>scenario that is run</returns>
    public static Acted<TData, TResult> When<TData, TResult>(
        this Arranged<TData> scenario,
        Func<TData, Task<TResult>> fn
    ) => New(scenario.ArrangeStep, fn, scenario.Name);

    /// <summary>
    /// When the scenario is run
    /// </summary>
    /// <param name="scenario">ready to run scenario</param>
    /// <param name="fn">async when function</param>
    /// <typeparam name="TData">scenario data</typeparam>
    /// <typeparam name="TResult">result of running the scenario</typeparam>
    /// <returns>scenario that is run</returns>
    public static Acted<TData, TResult> When<TData, TResult>(
        this Arranged<TData> scenario,
        Func<TData, TResult> fn
    ) => scenario.When(data => Task.FromResult(fn(data)));

    /// <summary>
    /// Then verify the scenario
    /// </summary>
    /// <param name="scenario">run scenario</param>
    /// <param name="fn">async then function</param>
    /// <typeparam name="TData">scenario data</typeparam>
    /// <typeparam name="TResult">result of running the scenario</typeparam>
    /// <returns>completed scenario</returns>
    public static Asserted<TData, TResult> Then<TData, TResult>(
        this Acted<TData, TResult> scenario,
        Func<TData, TResult, Task> fn
    ) => New(scenario.ArrangeStep, scenario.ActStep, fn, scenario.Name);

    /// <summary>
    /// Then verify the scenario
    /// </summary>
    /// <param name="scenario">run scenario</param>
    /// <param name="fn">async then function</param>
    /// <typeparam name="TData">scenario data</typeparam>
    /// <typeparam name="TResult">result of running the scenario</typeparam>
    /// <returns>completed scenario</returns>
    public static Asserted<TData, TResult> Then<TData, TResult>(
        this Acted<TData, TResult> scenario,
        Func<TResult, Task> fn
    ) => scenario.Then((_, result) => fn(result));

    /// <summary>
    /// Then verify the scenario
    /// </summary>
    /// <param name="scenario">run scenario</param>
    /// <param name="fn">then function</param>
    /// <typeparam name="TData">scenario data</typeparam>
    /// <typeparam name="TResult">result of running the scenario</typeparam>
    /// <returns>completed scenario</returns>
    public static Asserted<TData, TResult> Then<TData, TResult>(
        this Acted<TData, TResult> scenario,
        Action<TData, TResult> fn
    ) =>
        scenario.Then(
            (data, result) =>
            {
                fn(data, result);
                return Task.CompletedTask;
            }
        );

    /// <summary>
    /// Then verify the scenario
    /// </summary>
    /// <param name="scenario">run scenario</param>
    /// <param name="fn">then function</param>
    /// <typeparam name="TData">scenario data</typeparam>
    /// <typeparam name="TResult">result of running the scenario</typeparam>
    /// <returns>completed scenario</returns>
    public static Asserted<TData, TResult> Then<TData, TResult>(
        this Acted<TData, TResult> scenario,
        Action<TResult> fn
    ) => scenario.Then((_, result) => fn(result));

    /// <summary>
    /// Then verify the scenario
    /// </summary>
    /// <param name="scenario">run scenario</param>
    /// <param name="expression">then expression</param>
    /// <typeparam name="TData">scenario data</typeparam>
    /// <typeparam name="TResult">result of running the scenario</typeparam>
    /// <returns>completed scenario</returns>
    public static Asserted<TData, TResult> Then<TData, TResult>(
        this Acted<TData, TResult> scenario,
        Expression<Func<TResult, bool>> expression
    ) => scenario.Then(expression.RunExpressionAssertion);

    /// <summary>
    /// Then verify the scenario
    /// </summary>
    /// <param name="scenario">run scenario</param>
    /// <param name="expression">then expression</param>
    /// <typeparam name="TData">scenario data</typeparam>
    /// <typeparam name="TResult">result of running the scenario</typeparam>
    /// <returns>completed scenario</returns>
    public static Asserted<TData, TResult> Then<TData, TResult>(
        this Acted<TData, TResult> scenario,
        Expression<Func<TData, TResult, bool>> expression
    ) => scenario.Then(expression.RunExpressionAssertion);
}
