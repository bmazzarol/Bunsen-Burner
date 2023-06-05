namespace BunsenBurner.FunctionApp;

using AaaScenario = Scenario<Syntax.Aaa>;

/// <summary>
/// DSL for building tests using an arrange, act, assert syntax
/// </summary>
public static class Aaa
{
    /// <summary>
    /// Arranges a function app
    /// </summary>
    /// <typeparam name="TStartup">function app startup</typeparam>
    /// <typeparam name="TFunction">function app</typeparam>
    /// <returns>arranged scenario</returns>
    [Pure]
    public static AaaScenario.Arranged<TFunction> ArrangeFunctionApp<TStartup, TFunction>()
        where TStartup : FunctionsStartup, new()
        where TFunction : class => Shared.ArrangeFunctionApp<TStartup, TFunction, Syntax.Aaa>();

    /// <summary>
    /// Arranges a function app
    /// </summary>
    /// <param name="name">name/description</param>
    /// <typeparam name="TStartup">function app startup</typeparam>
    /// <typeparam name="TFunction">function app</typeparam>
    /// <returns>arranged scenario</returns>
    [Pure]
    public static AaaScenario.Arranged<TFunction> ArrangeFunctionApp<TStartup, TFunction>(
        this string name
    )
        where TStartup : FunctionsStartup, new()
        where TFunction : class => name.ArrangeFunctionApp<TStartup, TFunction, Syntax.Aaa>();

    /// <summary>
    /// Arranges a function app
    /// </summary>
    /// <typeparam name="TData">existing arranged data</typeparam>
    /// <typeparam name="TStartup">function app startup</typeparam>
    /// <typeparam name="TFunction">function app</typeparam>
    /// <returns>arranged scenario</returns>
    [Pure]
    public static AaaScenario.Arranged<(TData Data, TFunction FunctionApp)> AndFunctionApp<
        TData,
        TStartup,
        TFunction
    >(this AaaScenario.Arranged<TData> scenario)
        where TStartup : FunctionsStartup, new()
        where TFunction : class =>
        scenario.AndFunctionApp<TData, TStartup, TFunction, Syntax.Aaa>();

    /// <summary>
    /// Executes the function app
    /// </summary>
    /// <param name="scenario">arranged scenario</param>
    /// <param name="functionApp">function app to test</param>
    /// <param name="fn">execute the function app, returning a result</param>
    /// <typeparam name="TData">arranged data</typeparam>
    /// <typeparam name="TResult">result of executing the function app</typeparam>
    /// <typeparam name="TFunction">function app to execute</typeparam>
    /// <returns>acted scenario</returns>
    [Pure]
    public static AaaScenario.Acted<TData, TResult> ActAndExecute<TData, TResult, TFunction>(
        this AaaScenario.Arranged<TData> scenario,
        Func<TData, TFunction> functionApp,
        Func<TData, TFunction, Task<TResult>> fn
    )
        where TFunction : class =>
        scenario.ActAndExecute<TData, TResult, TFunction, Syntax.Aaa>(functionApp, fn);

    /// <summary>
    /// Executes the function app
    /// </summary>
    /// <param name="scenario">arranged scenario</param>
    /// <param name="fn">execute the function app, returning a result</param>
    /// <typeparam name="TFunction">function app to execute</typeparam>
    /// <typeparam name="TResult">result of executing the function app</typeparam>
    /// <returns>acted scenario</returns>
    [Pure]
    public static AaaScenario.Acted<TFunction, TResult> ActAndExecute<TFunction, TResult>(
        this AaaScenario.Arranged<TFunction> scenario,
        Func<TFunction, Task<TResult>> fn
    )
        where TFunction : class => scenario.ActAndExecute<TFunction, TResult, Syntax.Aaa>(fn);
}
