namespace BunsenBurner.Http;

/// <summary>
/// Predicate used to match incoming HTTP requests.
/// </summary>
public abstract record HttpRequestPredicate
{
    private readonly Func<HttpRequestMessage, bool> _predicate;

    private HttpRequestPredicate(Func<HttpRequestMessage, bool> predicate) =>
        _predicate = predicate;

    private sealed record HttpRequestPredicateImpl : HttpRequestPredicate
    {
        public HttpRequestPredicateImpl(Func<HttpRequestMessage, bool> predicate)
            : base(predicate) { }
    }

    /// <summary>
    /// Creates a new predicate against incoming HTTP requests.
    /// </summary>
    /// <param name="predicate">predicate</param>
    /// <returns>predicate</returns>
    public static HttpRequestPredicate New(Func<HttpRequestMessage, bool> predicate) =>
        new HttpRequestPredicateImpl(predicate);

    /// <summary>
    /// Returns true if the HTTP request matches the predicate.
    /// </summary>
    /// <param name="message">message</param>
    /// <returns>true if match</returns>
    public bool IsMatch(HttpRequestMessage message) => _predicate(message);

    /// <summary>
    /// Combines a second matchers with this matcher using an And operator
    /// </summary>
    /// <param name="b">second matcher</param>
    /// <returns>match that succeeds when a and b succeed</returns>
    public HttpRequestPredicate And(HttpRequestPredicate b) => New(x => IsMatch(x) && b.IsMatch(x));

    /// <summary>
    /// Combines a second matchers with this matcher using an And operator
    /// </summary>
    /// <param name="a">first matcher</param>
    /// <param name="b">second matcher</param>
    /// <returns>match that succeeds when a and b succeed</returns>
    public static HttpRequestPredicate operator &(HttpRequestPredicate a, HttpRequestPredicate b) =>
        a.And(b);

    /// <summary>
    /// Combines a second matchers with this matcher using an Or operator
    /// </summary>
    /// <param name="b">second matcher</param>
    /// <returns>match that succeeds when a or b succeed</returns>
    public HttpRequestPredicate Or(HttpRequestPredicate b) => New(x => IsMatch(x) || b.IsMatch(x));

    /// <summary>
    /// Combines a second matchers with this matcher using an Or operator
    /// </summary>
    /// <param name="a">first matcher</param>
    /// <param name="b">second matcher</param>
    /// <returns>match that succeeds when a or b succeed</returns>
    public static HttpRequestPredicate operator |(HttpRequestPredicate a, HttpRequestPredicate b) =>
        a.Or(b);
}
