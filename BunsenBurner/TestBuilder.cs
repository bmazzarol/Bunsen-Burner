namespace BunsenBurner;

/// <summary>
/// <para>A <see cref="TestBuilder{TSyntax}"/> defines a blueprint for an executable test.</para>
/// <para>When complete, can have up to 2 generic parameters,</para>
/// <para>
/// <ul>
/// <li>Data - the `data` required before acting</li>
/// <li>Result - the `result` of acting to assert against</li>
/// </ul>
/// </para>
/// <para>This construct can represent any single test, and provides the foundation for building tests as data.</para>
/// <para>The <see cref="TestBuilder{TSyntax}"/> will also manage disposal of all `data` and `result` values
/// that are used as long as they implement <see cref="IDisposable"/> or <see cref="IAsyncDisposable"/>.</para>
/// </summary>
/// <typeparam name="TSyntax">Supported syntax</typeparam>
public abstract partial record TestBuilder<TSyntax>
    where TSyntax : struct, ISyntax<TSyntax>
{
    /// <summary>
    /// Optional name for the test
    /// </summary>
    public string? Name { get; init; }

    /// <inheritdoc/>
    public sealed override string ToString() => Name ?? base.ToString();

    /// <summary>
    /// Stores any disposables for cleanup after the <see cref="TestBuilder{TSyntax}"/> is run
    /// </summary>
    private HashSet<object>? _disposables;

    private void TrackPotentialDisposal<T>(T potentialDisposable)
    {
        if (potentialDisposable is IDisposable or IAsyncDisposable)
        {
            _disposables ??= new();
            _disposables.Add(potentialDisposable);
        }
    }

    private TestBuilder() { }

    /// <summary>
    /// Builds a new <see cref="Arranged{TData}"/>
    /// </summary>
    /// <param name="fn">arrange function</param>
    /// <typeparam name="TData">data required to act on the <see cref="TestBuilder{TSyntax}"/></typeparam>
    /// <returns>arranged test</returns>
    public static Arranged<TData> BuildArranged<TData>(Func<Task<TData>> fn) => new(fn);

    /// <summary>
    /// Builds a new <see cref="Arranged{TData}"/>
    /// </summary>
    /// <param name="arrangeStep">arrange step</param>
    /// <param name="actStep">act step</param>
    /// <typeparam name="TData">data required to act on the <see cref="TestBuilder{TSyntax}"/></typeparam>
    /// <typeparam name="TResult">result of acting</typeparam>
    /// <returns>acted test</returns>
    public static Acted<TData, TResult> BuildActed<TData, TResult>(
        Func<Task<TData>> arrangeStep,
        Func<TData, Task<TResult>> actStep
    ) => new(arrangeStep, actStep);

    /// <summary>
    /// Builds a new <see cref="Asserted{TData, TResult}"/>
    /// </summary>
    /// <param name="arrangeStep">arrange step</param>
    /// <param name="actStep">act step</param>
    /// <param name="assertStep">assert step</param>
    /// <typeparam name="TData">data required to act on the <see cref="TestBuilder{TSyntax}"/></typeparam>
    /// <typeparam name="TResult">result of acting</typeparam>
    /// <returns>asserted test</returns>
    public static Asserted<TData, TResult> BuildAsserted<TData, TResult>(
        Func<Task<TData>> arrangeStep,
        Func<TData, Task<TResult>> actStep,
        Func<TData, TResult, Task> assertStep
    ) => new(arrangeStep, actStep, assertStep);
}
