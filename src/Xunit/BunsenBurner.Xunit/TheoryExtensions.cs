using Xunit;

namespace BunsenBurner.Xunit;

/// <summary>
/// Extension methods for integrating scenarios into theories
/// </summary>
public static class TheoryExtensions
{
    /// <summary>
    /// Converts an enumerable of scenarios to a theory data of scenario
    /// </summary>
    /// <param name="scenarios">scenarios</param>
    /// <typeparam name="TSyntax">supported syntax</typeparam>
    /// <typeparam name="TScenario">scenario</typeparam>
    /// <returns>theory data of scenario</returns>
    public static TheoryData<TScenario> AsTheoryData<TSyntax, TScenario>(
        this IEnumerable<TScenario> scenarios
    )
        where TSyntax : struct, Syntax
        where TScenario : Scenario<TSyntax> =>
        scenarios.Aggregate(
            new TheoryData<TScenario>(),
            (data, scenario) =>
            {
                data.Add(scenario);
                return data;
            }
        );

    /// <summary>
    /// Constructor function to build theory data from scenarios
    /// </summary>
    /// <param name="scenarios">scenarios</param>
    /// <typeparam name="TSyntax">supported syntax</typeparam>
    /// <typeparam name="TData">some arranged data</typeparam>
    /// <returns>theory data of scenario</returns>
    public static TheoryData<Scenario<TSyntax>.Arranged<TData>> TheoryData<TSyntax, TData>(
        params Scenario<TSyntax>.Arranged<TData>[] scenarios
    )
        where TSyntax : struct, Syntax =>
        scenarios.AsTheoryData<TSyntax, Scenario<TSyntax>.Arranged<TData>>();

    /// <summary>
    /// Constructor function to build theory data from scenarios
    /// </summary>
    /// <param name="scenarios">scenarios</param>
    /// <typeparam name="TSyntax">supported syntax</typeparam>
    /// <typeparam name="TData">some arranged data</typeparam>
    /// <typeparam name="TResult">some result of acting</typeparam>
    /// <returns>theory data of scenario</returns>
    public static TheoryData<Scenario<TSyntax>.Acted<TData, TResult>> TheoryData<
        TSyntax,
        TData,
        TResult
    >(params Scenario<TSyntax>.Acted<TData, TResult>[] scenarios)
        where TSyntax : struct, Syntax =>
        scenarios.AsTheoryData<TSyntax, Scenario<TSyntax>.Acted<TData, TResult>>();

    /// <summary>
    /// Constructor function to build theory data from scenarios
    /// </summary>
    /// <param name="scenarios">scenarios</param>
    /// <typeparam name="TSyntax">supported syntax</typeparam>
    /// <typeparam name="TData">some arranged data</typeparam>
    /// <typeparam name="TResult">some result of acting</typeparam>
    /// <returns>theory data of scenario</returns>
    public static TheoryData<Scenario<TSyntax>.Asserted<TData, TResult>> TheoryData<
        TSyntax,
        TData,
        TResult
    >(params Scenario<TSyntax>.Asserted<TData, TResult>[] scenarios)
        where TSyntax : struct, Syntax =>
        scenarios.AsTheoryData<TSyntax, Scenario<TSyntax>.Asserted<TData, TResult>>();
}
