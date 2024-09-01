namespace BunsenBurner;

public abstract partial record TestBuilder<TSyntax>
{
    /// <summary>
    /// A <see cref="TestBuilder{TSyntax}"/> that has been arranged and is ready to act on
    /// </summary>
    /// <typeparam name="TData">type of `data` required to act on the <see cref="TestBuilder{TSyntax}"/></typeparam>
    public sealed record Arranged<TData> : TestBuilder<TSyntax>
    {
        private readonly Func<Task<TData>> _arrangeStep;

        internal Arranged(Func<Task<TData>> arrangeStep) => _arrangeStep = arrangeStep;

        /// <summary>
        /// Arrange step
        /// </summary>
        public Func<Task<TData>> ArrangeStep =>
            async () =>
            {
                var data = await _arrangeStep();
                TrackPotentialDisposal(data);
                return data;
            };

        /// <summary>
        /// Allows for additional arranging of test data
        /// </summary>
        /// <param name="fn">async function transforming test data into test data</param>
        /// <typeparam name="TDataNext">next data required to act on the test</typeparam>
        /// <returns>arranged test</returns>
        [Pure]
        public Arranged<TDataNext> And<TDataNext>(Func<TData, Task<TDataNext>> fn) =>
            new(async () =>
            {
                var result = await ArrangeStep();
                var nextResult = await fn(result);
                return nextResult;
            })
            {
                Name = Name,
            };

        /// <summary>
        /// Allows for additional arranging of test data
        /// </summary>
        /// <param name="fn">function transforming test data into test data</param>
        /// <typeparam name="TDataNext">next data required to act on the test</typeparam>
        /// <returns>arranged test</returns>
        [Pure]
        public Arranged<TDataNext> And<TDataNext>(Func<TData, TDataNext> fn) =>
            And(data => Task.FromResult(fn(data)));

        /// <summary>
        /// Allows for additional arranging of test data
        /// </summary>
        /// <param name="action">action to apply to the test data</param>
        /// <returns>arranged test</returns>
        [Pure]
        public Arranged<TData> And(Func<TData, Task> action) =>
            And(async data =>
            {
                await action(data);
                return data;
            });

        /// <summary>
        /// Allows for additional arranging of test data
        /// </summary>
        /// <param name="action">action to apply to the test data</param>
        /// <returns>arranged test</returns>
        [Pure]
        public Arranged<TData> And(Action<TData> action) =>
            And(data =>
            {
                action(data);
                return data;
            });
    }
}
