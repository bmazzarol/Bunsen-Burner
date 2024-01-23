using System.Collections;
using System.Diagnostics.CodeAnalysis;

namespace BunsenBurner.Http;

/// <summary>
/// Singleton store of http request, response setups.
/// A setup is a predicate that matches an incoming request paired with a response factory.
/// It also records all matches, so that they can be asserted against.
/// </summary>
public sealed class HttpMessageStore : IEnumerable<HttpMessageSetupMatch>
{
    private sealed record HttpMessageSetup(
        string ClientName,
        HttpRequestPredicate MatchPredicate,
        Func<HttpRequestMessage, HttpResponseMessage?> SetupResponses
    );

    private readonly List<HttpMessageSetup> _setups;
    private readonly List<HttpMessageSetupMatch> _matches;

    private HttpMessageStore()
    {
        _setups = new List<HttpMessageSetup>();
        _matches = new List<HttpMessageSetupMatch>();
    }

    /// <summary>
    /// Creates a new instance of the factory to start setting up
    /// </summary>
    /// <returns>dummy factory</returns>
    public static HttpMessageStore New() => new();

    private readonly object _lock = new();

    private async Task<HttpResponseMessage> MatchOrThrow(
        string clientName,
        HttpRequestMessage request
    )
    {
        var response = _setups
            .Where(
                setup =>
                    setup.ClientName.Equals(clientName, StringComparison.Ordinal)
                    && setup.MatchPredicate.IsMatch(request)
            )
            .Select(setup => setup.SetupResponses(request))
            .OfType<HttpResponseMessage>()
            .FirstOrDefault();

        if (response is null)
        {
            throw new HttpRequestException(
                $"No setup matches/generates a response for request: {await request.ToCurlString()}"
            );
        }

        lock (_lock)
        {
            _matches.Add(new HttpMessageSetupMatch(clientName, request, response));
        }

        return response;
    }

    /// <summary>
    /// Setup a request matcher
    /// </summary>
    /// <param name="name">client name to link the request matcher against</param>
    /// <param name="matchPredicate">predicate to match against</param>
    /// <param name="responder">responder function</param>
    /// <returns>factory</returns>
    public HttpMessageStore Setup(
        string name,
        HttpRequestPredicate matchPredicate,
        Func<HttpRequestMessage, HttpResponseMessage?> responder
    )
    {
        _setups.Add(new HttpMessageSetup(name, matchPredicate, responder));
        return this;
    }

    /// <summary>
    /// Setup a request matcher
    /// </summary>
    /// <param name="name">client name to link the request matcher against</param>
    /// <param name="matchPredicate">predicate to match against</param>
    /// <param name="responder">responder function</param>
    /// <returns>factory</returns>
    public HttpMessageStore Setup(
        string name,
        Func<HttpRequestMessage, bool> matchPredicate,
        Func<HttpRequestMessage, HttpResponseMessage?> responder
    ) => Setup(name, HttpRequestPredicate.New(matchPredicate), responder);

    /// <summary>
    /// Setup a request matcher
    /// </summary>
    /// <param name="name">client name to link the request matcher against</param>
    /// <param name="matchPredicate">predicate to match against</param>
    /// <param name="response">response to use for all matching requests</param>
    /// <returns>factory</returns>
    public HttpMessageStore Setup(
        string name,
        Func<HttpRequestMessage, bool> matchPredicate,
        HttpResponseMessage response
    ) =>
        Setup(
            name,
            matchPredicate,
            req =>
            {
                response.RequestMessage = req;
                return response;
            }
        );

    /// <summary>
    /// Setup a request matcher
    /// </summary>
    /// <param name="name">client name to link the request matcher against</param>
    /// <param name="matchPredicate">predicate to match against</param>
    /// <param name="responses">responses to use in order for matching requests that come in</param>
    /// <returns>factory</returns>
    public HttpMessageStore Setup(
        string name,
        Func<HttpRequestMessage, bool> matchPredicate,
        params HttpResponseMessage[] responses
    )
    {
        var i = 0;
        return Setup(
            name,
            matchPredicate,
            req =>
            {
                try
                {
                    var response = responses[i];
                    response.RequestMessage = req;
                    i++;
                    return response;
                }
                catch
                {
                    return null;
                }
            }
        );
    }

    /// <inheritdoc />
    public IEnumerator<HttpMessageSetupMatch> GetEnumerator()
    {
        lock (_lock)
        {
            foreach (var match in _matches)
            {
                yield return match;
            }
        }
    }

    [ExcludeFromCodeCoverage]
    IEnumerator IEnumerable.GetEnumerator() => GetEnumerator();

    /// <summary>
    /// Creates a client from the store
    /// </summary>
    /// <param name="name">name of the client</param>
    /// <returns>http client</returns>
    public HttpClient CreateClient(string name) => new(new DummyHttpMessageHandler(name, this));

    private sealed class DummyHttpMessageHandler(string clientName, HttpMessageStore store)
        : HttpMessageHandler
    {
        protected override Task<HttpResponseMessage> SendAsync(
            HttpRequestMessage request,
            CancellationToken cancellationToken
        ) => store.MatchOrThrow(clientName, request);
    }
}
