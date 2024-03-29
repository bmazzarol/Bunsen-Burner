﻿using System.Diagnostics.CodeAnalysis;
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
    internal static Scenario<TSyntax>.Acted<Gen<TData>, Property<TData>> ArrangeGenerator<
        TData,
        TSyntax
    >(this string name, Gen<TData> generator)
        where TSyntax : struct, Syntax =>
        name.Arrange<Gen<TData>, TSyntax>(generator).Act(Property.ForAll);

    [Pure]
    internal static Scenario<TSyntax>.Acted<Gen<TData>, Property<TData>> ArrangeGenerator<
        TData,
        TSyntax
    >(this Gen<TData> generator)
        where TSyntax : struct, Syntax => string.Empty.ArrangeGenerator<TData, TSyntax>(generator);

    [Pure]
    internal static Scenario<TSyntax>.Asserted<Gen<TData>, Property<TData>> AssertPropertyHolds<
        TData,
        TSyntax
    >(
        this Scenario<TSyntax>.Acted<Gen<TData>, Property<TData>> scenario,
        Func<TData, bool> fn,
        PropertyConfig? config = default
    )
        where TSyntax : struct, Syntax =>
        scenario.Assert(
            r => r.Select(fn).Check(config ?? global::Hedgehog.Linq.PropertyConfig.Default)
        );

    [Pure]
    [ExcludeFromCodeCoverage]
    internal static Scenario<TSyntax>.Asserted<Gen<TData>, Property<TData>> AssertPropertyHolds<
        TData,
        TSyntax
    >(
        this Scenario<TSyntax>.Acted<Gen<TData>, Property<TData>> scenario,
        Action<TData> fn,
        PropertyConfig? config = default
    )
        where TSyntax : struct, Syntax =>
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
