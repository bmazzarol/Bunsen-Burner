namespace BunsenBurner.FunctionApp;

using AaaScenario = Scenario<Syntax.Aaa>;

/// <summary>
/// DSL for building tests using an arrange, act, assert syntax
/// </summary>
public static class Aaa
{
    /// <summary>
    /// Executes the function app
    /// </summary>
    /// <param name="scenario">arranged scenario</param>
    /// <param name="fn">execute the function app, returning a result</param>
    /// <typeparam name="TData">arranged data</typeparam>
    /// <typeparam name="TResult">result of executing the function app</typeparam>
    /// <typeparam name="TStartup">supported startup class, required to bootstrap the function app</typeparam>
    /// <typeparam name="TFunction">function app to execute</typeparam>
    /// <returns>acted scenario</returns>
    [Pure]
    public static AaaScenario.Acted<TData, TResult> ActAndExecute<
        TData,
        TResult,
        TStartup,
        TFunction
    >(this AaaScenario.Arranged<TData> scenario, Func<TData, TFunction, Task<TResult>> fn)
        where TStartup : FunctionsStartup, new()
        where TFunction : class =>
        scenario.ActAndExecute<TData, TResult, TStartup, TFunction, Syntax.Aaa>(fn);
}
