namespace BunsenBurner.FunctionApp;

using BddScenario = Scenario<Syntax.Bdd>;

/// <summary>
/// DSL for building tests using a given, when, then syntax
/// </summary>
public static class Bdd
{
    /// <summary>
    /// Given a function app
    /// </summary>
    /// <typeparam name="TStartup">function app startup</typeparam>
    /// <typeparam name="TFunction">function app</typeparam>
    /// <returns>given scenario</returns>
    [Pure]
    public static BddScenario.Arranged<TFunction> GivenFunctionApp<TStartup, TFunction>()
        where TStartup : FunctionsStartup, new()
        where TFunction : class => Shared.ArrangeFunctionApp<TStartup, TFunction, Syntax.Bdd>();

    /// <summary>
    /// Given a function app
    /// </summary>
    /// <param name="name">name/description</param>
    /// <typeparam name="TStartup">function app startup</typeparam>
    /// <typeparam name="TFunction">function app</typeparam>
    /// <returns>given scenario</returns>
    [Pure]
    public static BddScenario.Arranged<TFunction> GivenFunctionApp<TStartup, TFunction>(
        this string name
    )
        where TStartup : FunctionsStartup, new()
        where TFunction : class => name.ArrangeFunctionApp<TStartup, TFunction, Syntax.Bdd>();

    /// <summary>
    /// Given a function app
    /// </summary>
    /// <typeparam name="TData">existing arranged data</typeparam>
    /// <typeparam name="TStartup">function app startup</typeparam>
    /// <typeparam name="TFunction">function app</typeparam>
    /// <returns>given scenario</returns>
    [Pure]
    public static BddScenario.Arranged<(TData Data, TFunction FunctionApp)> AndFunctionApp<
        TData,
        TStartup,
        TFunction
    >(this BddScenario.Arranged<TData> scenario)
        where TStartup : FunctionsStartup, new()
        where TFunction : class =>
        scenario.AndFunctionApp<TData, TStartup, TFunction, Syntax.Bdd>();

    /// <summary>
    /// Executes the function app
    /// </summary>
    /// <param name="scenario">given scenario</param>
    /// <param name="functionApp">function app to test</param>
    /// <param name="fn">execute the function app, returning a result</param>
    /// <typeparam name="TData">given data</typeparam>
    /// <typeparam name="TResult">result of executing the function app</typeparam>
    /// <typeparam name="TFunction">function app to execute</typeparam>
    /// <returns>scenario that is run</returns>
    [Pure]
    public static BddScenario.Acted<TData, TResult> WhenExecuted<TData, TResult, TFunction>(
        this BddScenario.Arranged<TData> scenario,
        Func<TData, TFunction> functionApp,
        Func<TData, TFunction, Task<TResult>> fn
    )
        where TFunction : class => scenario.ActAndExecute(functionApp, fn);

    /// <summary>
    /// Executes the function app
    /// </summary>
    /// <param name="scenario">given scenario</param>
    /// <param name="fn">execute the function app, returning a result</param>
    /// <typeparam name="TFunction">function app to execute</typeparam>
    /// <typeparam name="TResult">result of executing the function app</typeparam>
    /// <returns>scenario that is run</returns>
    [Pure]
    public static BddScenario.Acted<TFunction, TResult> WhenExecuted<TFunction, TResult>(
        this BddScenario.Arranged<TFunction> scenario,
        Func<TFunction, Task<TResult>> fn
    )
        where TFunction : class => scenario.ActAndExecute(fn);
}
