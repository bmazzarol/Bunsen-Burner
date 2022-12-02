using static BunsenBurner.Shared;

namespace BunsenBurner.FunctionApp;

/// <summary>
/// Shared builder function implementations
/// </summary>
internal static class Shared
{
    [Pure]
    internal static Scenario<TSyntax>.Arranged<TFunction> ArrangeFunctionApp<
        TStartup,
        TFunction,
        TSyntax
    >()
        where TStartup : FunctionsStartup, new()
        where TFunction : class
        where TSyntax : struct, Syntax =>
        Arrange<TFunction, TSyntax>(FunctionAppBuilder.Create<TStartup, TFunction>());

    [Pure]
    internal static Scenario<TSyntax>.Arranged<TFunction> ArrangeFunctionApp<
        TStartup,
        TFunction,
        TSyntax
    >(this string name)
        where TStartup : FunctionsStartup, new()
        where TFunction : class
        where TSyntax : struct, Syntax =>
        name.Arrange<TFunction, TSyntax>(FunctionAppBuilder.Create<TStartup, TFunction>());

    [Pure]
    internal static Scenario<TSyntax>.Arranged<(TData Data, TFunction FunctionApp)> AndFunctionApp<
        TData,
        TStartup,
        TFunction,
        TSyntax
    >(this Scenario<TSyntax>.Arranged<TData> scenario)
        where TStartup : FunctionsStartup, new()
        where TFunction : class
        where TSyntax : struct, Syntax =>
        scenario.And(data =>
        {
            var ctx = FunctionAppBuilder.Create<TStartup, TFunction>();
            return (Data: data, FunctionApp: ctx);
        });

    [Pure]
    internal static Scenario<TSyntax>.Acted<TData, TResult> ActAndExecute<
        TData,
        TResult,
        TFunction,
        TSyntax
    >(
        this Scenario<TSyntax>.Arranged<TData> scenario,
        Func<TData, TFunction> functionApp,
        Func<TData, TFunction, Task<TResult>> fn
    )
        where TFunction : class
        where TSyntax : struct, Syntax => scenario.Act(data => fn(data, functionApp(data)));

    [Pure]
    internal static Scenario<TSyntax>.Acted<TFunction, TResult> ActAndExecute<
        TFunction,
        TResult,
        TSyntax
    >(this Scenario<TSyntax>.Arranged<TFunction> scenario, Func<TFunction, Task<TResult>> fn)
        where TFunction : class
        where TSyntax : struct, Syntax => scenario.Act(fn);
}
