namespace BunsenBurner;

public abstract partial record TestBuilder<TSyntax>
{
    /// <summary>
    /// A <see cref="TestBuilder{TSyntax}"/> that has been arranged and acted on ready for assertions
    /// </summary>
    /// <typeparam name="TData">type of `data` required to act on the <see cref="TestBuilder{TSyntax}"/></typeparam>
    /// <typeparam name="TResult">`result` of acting</typeparam>
    public sealed record Acted<TData, TResult> : TestBuilder<TSyntax>
    {
        private readonly Func<Task<TData>> _arrangeStep;
        private readonly Func<TData, Task<TResult>> _actStep;

        internal Acted(
            Func<Task<TData>> arrangeStep,
            Func<TData, Task<TResult>> actStep,
            HashSet<object> disposables
        )
            : base(disposables)
        {
            _arrangeStep = arrangeStep;
            _actStep = actStep;
        }

        /// <summary>
        /// Arrange step
        /// </summary>
        public Func<Task<TData>> ArrangeStep =>
            async () =>
            {
                var result = await _arrangeStep();
                TrackPotentialDisposal(result);
                return result;
            };

        /// <summary>
        /// Act step
        /// </summary>
        public Func<TData, Task<TResult>> ActStep =>
            async data =>
            {
                var result = await _actStep(data);
                TrackPotentialDisposal(result);
                return result;
            };

        /// <summary>
        /// Flips the result of the act step to the error side
        /// </summary>
        /// <typeparam name="TException">exception type to expect</typeparam>
        /// <returns>an acted test</returns>
        /// <exception cref="NoFailureException">thrown when the act step does not throw an exception</exception>
        [Pure]
        public Acted<TData, TException> Throw<TException>()
            where TException : Exception =>
            new(
                ArrangeStep,
                async data =>
                {
                    try
                    {
                        await ActStep(data);
                        throw new NoFailureException();
                    }
                    catch (TException e) when (e is not NoFailureException)
                    {
                        return e;
                    }
                },
                Disposables
            );

        /// <summary>
        /// Flips the result of the act step to the error side
        /// </summary>
        /// <returns>an acted test</returns>
        /// <exception cref="NoFailureException">thrown when the act step does not throw an exception</exception>
        [Pure]
        public Acted<TData, Exception> Throw() => Throw<Exception>();
    }
}
