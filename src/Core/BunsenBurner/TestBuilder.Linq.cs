namespace BunsenBurner;

/// <summary>
/// Adds linq support to <see cref="TestBuilder{TSyntax}"/>
/// </summary>
public static class TestBuilder
{
    /// <summary>
    /// Select (Map) for <see cref="TestBuilder{TSyntax}.Arranged{TData}"/>
    /// </summary>
    /// <param name="test">arranged test of TA</param>
    /// <param name="fn">fn from TA to TB</param>
    /// <typeparam name="TSyntax">syntax</typeparam>
    /// <typeparam name="TA">some TA</typeparam>
    /// <typeparam name="TB">some TB</typeparam>
    /// <returns>arranged test of TB</returns>
    [Pure]
    public static TestBuilder<TSyntax>.Arranged<TB> Select<TSyntax, TA, TB>(
        this TestBuilder<TSyntax>.Arranged<TA> test,
        Func<TA, TB> fn
    )
        where TSyntax : struct, Syntax =>
        new(async () => fn(await test.ArrangeStep()), test.Disposables);

    /// <summary>
    /// Select (Map) for <see cref="TestBuilder{TSyntax}.Acted{TData,TResult}"/>
    /// </summary>
    /// <param name="test">acted test of TA</param>
    /// <param name="fn">fn from TA to TB</param>
    /// <typeparam name="TSyntax">syntax</typeparam>
    /// <typeparam name="TData">arranged data</typeparam>
    /// <typeparam name="TA">some TA</typeparam>
    /// <typeparam name="TB">some TB</typeparam>
    /// <returns>acted test of TB</returns>
    [Pure]
    public static TestBuilder<TSyntax>.Acted<TData, TB> Select<TSyntax, TData, TA, TB>(
        this TestBuilder<TSyntax>.Acted<TData, TA> test,
        Func<TA, TB> fn
    )
        where TSyntax : struct, Syntax =>
        new(test.ArrangeStep, async x => fn(await test.ActStep(x)), test.Disposables);

    /// <summary>
    /// SelectMany (Bind) for <see cref="TestBuilder{TSyntax}.Arranged{TData}"/>
    /// </summary>
    /// <param name="test">arranged test of TA</param>
    /// <param name="fn">fn from TA to arranged test of TB</param>
    /// <typeparam name="TSyntax">syntax</typeparam>
    /// <typeparam name="TA">some TA</typeparam>
    /// <typeparam name="TB">some TB</typeparam>
    /// <returns>arranged test of TB</returns>
    [Pure]
    public static TestBuilder<TSyntax>.Arranged<TB> SelectMany<TSyntax, TA, TB>(
        this TestBuilder<TSyntax>.Arranged<TA> test,
        Func<TA, TestBuilder<TSyntax>.Arranged<TB>> fn
    )
        where TSyntax : struct, Syntax =>
        new(async () => await fn(await test.ArrangeStep()).ArrangeStep(), test.Disposables);

    /// <summary>
    /// SelectMany (Bind) for <see cref="TestBuilder{TSyntax}.Arranged{TData}"/>
    /// </summary>
    /// <param name="test">arranged test of TA</param>
    /// <param name="mapFn">fn from TA to arranged test of TB</param>
    /// <param name="projectFn">projection function from TA, TB to TC</param>
    /// <typeparam name="TSyntax">syntax</typeparam>
    /// <typeparam name="TA">some TA</typeparam>
    /// <typeparam name="TB">some TB</typeparam>
    /// <typeparam name="TC">some TC</typeparam>
    /// <returns>arranged test of TC</returns>
    [Pure]
    public static TestBuilder<TSyntax>.Arranged<TC> SelectMany<TSyntax, TA, TB, TC>(
        this TestBuilder<TSyntax>.Arranged<TA> test,
        Func<TA, TestBuilder<TSyntax>.Arranged<TB>> mapFn,
        Func<TA, TB, TC> projectFn
    )
        where TSyntax : struct, Syntax =>
        new(
            async () =>
            {
                var ta = await test.ArrangeStep();
                var tb = await mapFn(ta).ArrangeStep();
                return projectFn(ta, tb);
            },
            test.Disposables
        );

    /// <summary>
    /// SelectMany (Bind) for <see cref="TestBuilder{TSyntax}.Acted{TData,TResult}"/>
    /// </summary>
    /// <param name="test">acted test of TA</param>
    /// <param name="fn">fn from TA to arranged test of TB</param>
    /// <typeparam name="TSyntax">syntax</typeparam>
    /// <typeparam name="TData">arranged data</typeparam>
    /// <typeparam name="TA">some TA</typeparam>
    /// <typeparam name="TB">some TB</typeparam>
    /// <returns>acted test of TB</returns>
    [Pure]
    public static TestBuilder<TSyntax>.Acted<TData, TB> SelectMany<TSyntax, TData, TA, TB>(
        this TestBuilder<TSyntax>.Acted<TData, TA> test,
        Func<TA, TestBuilder<TSyntax>.Acted<TData, TB>> fn
    )
        where TSyntax : struct, Syntax =>
        new(
            test.ArrangeStep,
            async x => await fn(await test.ActStep(x)).ActStep(x),
            test.Disposables
        );

    /// <summary>
    /// SelectMany (Bind) for <see cref="TestBuilder{TSyntax}.Acted{TData,TResult}"/>
    /// </summary>
    /// <param name="test">acted test of TA</param>
    /// <param name="mapFn">fn from TA to arranged test of TB</param>
    /// <param name="projectFn">projection function from TA, TB to TC</param>
    /// <typeparam name="TSyntax">syntax</typeparam>
    /// <typeparam name="TData">arranged data</typeparam>
    /// <typeparam name="TA">some TA</typeparam>
    /// <typeparam name="TB">some TB</typeparam>
    /// <typeparam name="TC">some TC</typeparam>
    /// <returns>acted test of TC</returns>
    [Pure]
    public static TestBuilder<TSyntax>.Acted<TData, TC> SelectMany<TSyntax, TData, TA, TB, TC>(
        this TestBuilder<TSyntax>.Acted<TData, TA> test,
        Func<TA, TestBuilder<TSyntax>.Acted<TData, TB>> mapFn,
        Func<TA, TB, TC> projectFn
    )
        where TSyntax : struct, Syntax =>
        new(
            test.ArrangeStep,
            async x =>
            {
                var ta = await test.ActStep(x);
                var tb = await mapFn(ta).ActStep(x);
                return projectFn(ta, tb);
            },
            test.Disposables
        );

    /// <summary>
    /// Converts n number of <see cref="TestBuilder{TSyntax}.Arranged{TData}"/> to a single <see cref="TestBuilder{TSyntax}.Arranged{TData}"/>
    /// </summary>
    /// <param name="tests">n number of arranged tests</param>
    /// <typeparam name="TSyntax">syntax</typeparam>
    /// <typeparam name="TData">arranged data</typeparam>
    /// <returns>single arranged test</returns>
    public static TestBuilder<TSyntax>.Arranged<IEnumerable<TData>> Combine<TSyntax, TData>(
        this IEnumerable<TestBuilder<TSyntax>.Arranged<TData>> tests
    )
        where TSyntax : struct, Syntax
    {
        var testsToCombine = tests.ToArray();
        return new TestBuilder<TSyntax>.Arranged<IEnumerable<TData>>(
            async () => await Task.WhenAll(testsToCombine.Select(x => x.ArrangeStep())),
            testsToCombine.SelectMany(test => test.Disposables).ToHashSet()
        );
    }

    /// <summary>
    /// Converts n number of <see cref="TestBuilder{TSyntax}.Asserted{TData,TResult}"/> to a single <see cref="TestBuilder{TSyntax}.Asserted{TData,TResult}"/>
    /// </summary>
    /// <param name="tests">n number of tests</param>
    /// <typeparam name="TSyntax">syntax</typeparam>
    /// <typeparam name="TData">arranged data</typeparam>
    /// <typeparam name="TResult">result of acting</typeparam>
    /// <returns>single asserted test</returns>
    public static TestBuilder<TSyntax>.Asserted<IEnumerable<TData>, IEnumerable<TResult>> Combine<
        TSyntax,
        TData,
        TResult
    >(this IEnumerable<TestBuilder<TSyntax>.Asserted<TData, TResult>> tests)
        where TSyntax : struct, Syntax
    {
        var testsToCombine = tests.ToArray();
        return new TestBuilder<TSyntax>.Asserted<IEnumerable<TData>, IEnumerable<TResult>>(
            async () => await Task.WhenAll(testsToCombine.Select(x => x.ArrangeStep())),
            async data => await Task.WhenAll(testsToCombine.Zip(data, (s, d) => s.ActStep(d))),
            async (context, result) =>
                await Task.WhenAll(
                    testsToCombine.Zip(
                        context.Zip(result, (d, r) => (d, r)),
                        (s, t) => s.AssertStep(t.d, t.r)
                    )
                ),
            testsToCombine.SelectMany(test => test.Disposables).ToHashSet()
        );
    }
}
