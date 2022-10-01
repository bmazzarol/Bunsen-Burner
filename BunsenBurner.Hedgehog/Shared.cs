using System.Diagnostics.CodeAnalysis;
using Hedgehog.Linq;
using Property = Hedgehog.Linq.Property;
using PropertyConfig = Hedgehog.PropertyConfig;

namespace BunsenBurner.Hedgehog;

/// <summary>
/// Shared builder function implementations
/// </summary>
internal static class Shared
{
    [Pure]
    internal static Scenario<TSyntax>.Arranged<Gen<TData>> ArrangeGenerator<TData, TSyntax>(
        this string name,
        Gen<TData> generator
    ) where TSyntax : struct, Syntax => new(name, () => Task.FromResult(generator));

    [Pure]
    internal static Scenario<TSyntax>.Arranged<Gen<TData>> ArrangeGenerator<TData, TSyntax>(
        this Gen<TData> generator
    ) where TSyntax : struct, Syntax => string.Empty.ArrangeGenerator<TData, TSyntax>(generator);

    [Pure]
    internal static Scenario<TSyntax>.Asserted<Gen<TData>, bool> AssertPropertyHolds<
        TData,
        TSyntax
    >(
        this Scenario<TSyntax>.Arranged<Gen<TData>> scenario,
        Func<TData, bool> fn,
        PropertyConfig? config = default
    ) where TSyntax : struct, Syntax =>
        new(
            scenario.Name,
            scenario.ArrangeScenario,
            gen =>
            {
                var property = Property.ForAll(gen).Select(fn);
                property.Check(config ?? global::Hedgehog.Linq.PropertyConfig.Default);
                return Task.FromResult(true);
            },
            (_, _) => Task.CompletedTask
        );

    [Pure]
    [ExcludeFromCodeCoverage]
    internal static Scenario<TSyntax>.Asserted<Gen<TData>, bool> AssertPropertyHolds<
        TData,
        TSyntax
    >(
        this Scenario<TSyntax>.Arranged<Gen<TData>> scenario,
        Action<TData> fn,
        PropertyConfig? config = default
    ) where TSyntax : struct, Syntax =>
        scenario.AssertPropertyHolds(
            data =>
            {
                try
                {
                    fn(data);
                    return true;
                }
                catch
                {
                    return false;
                }
            },
            config
        );
}
