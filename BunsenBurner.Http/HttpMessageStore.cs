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
        Func<HttpRequestMessage, bool> MatchPredicate,
        Func<HttpRequestMessage, Option<HttpResponseMessage>> SetupResponses
    );

    private Seq<HttpMessageSetup> _setups;
    private readonly AtomSeq<HttpMessageSetupMatch> _matches;

    private HttpMessageStore()
    {
        _setups = LanguageExt.Seq<HttpMessageSetup>.Empty;
        _matches = AtomSeq<HttpMessageSetupMatch>();
    }

    /// <summary>
    /// Creates a new instance of the factory to start setting up
    /// </summary>
    /// <returns>dummy factory</returns>
    public static HttpMessageStore New() => new();

    private async Task<HttpResponseMessage> MatchOrThrow(
        string clientName,
        HttpRequestMessage request
    )
    {
        await Task.Yield();

        var response =
            from handler in _setups.Filter(
                x =>
                    x.ClientName.Equals(clientName, StringComparison.Ordinal)
                    && x.MatchPredicate(request)
            )
            from result in handler.SetupResponses(request)
            select result;

        if (response.HeadOrNone().Case is not HttpResponseMessage resp)
        {
            throw new HttpRequestException(
                $"No setup matches/generates a response for request: {await request.AsCurlString()}"
            );
        }

        _matches.Add(new HttpMessageSetupMatch(clientName, request, resp));

        return resp;
    }

    /// <summary>
    /// Setup a response matcher
    /// </summary>
    /// <param name="name">client name to link the response matcher against</param>
    /// <param name="matchPredicate">predicate to match against</param>
    /// <param name="responder">responder function</param>
    /// <returns>factory</returns>
    public HttpMessageStore Setup(
        string name,
        Func<HttpRequestMessage, bool> matchPredicate,
        Func<HttpRequestMessage, Option<HttpResponseMessage>> responder
    )
    {
        _setups = _setups.Add(new HttpMessageSetup(name, matchPredicate, responder));
        return this;
    }

    /// <summary>
    /// Setup a response matcher
    /// </summary>
    /// <param name="name">client name to link the response matcher against</param>
    /// <param name="matchPredicate">predicate to match against</param>
    /// <param name="responder">responder function</param>
    /// <returns>factory</returns>
    public HttpMessageStore Setup(
        string name,
        Func<HttpRequestMessage, bool> matchPredicate,
        Func<HttpRequestMessage, HttpResponseMessage?> responder
    ) => Setup(name, matchPredicate, req => Optional(responder(req)));

    /// <summary>
    /// Setup a response matcher
    /// </summary>
    /// <param name="name">client name to link the response matcher against</param>
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
    /// Setup a response matcher
    /// </summary>
    /// <param name="name">client name to link the response matcher against</param>
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
                    return Some(response);
                }
                catch
                {
                    return Option<HttpResponseMessage>.None;
                }
            }
        );
    }

    /// <inheritdoc />
    public IEnumerator<HttpMessageSetupMatch> GetEnumerator() => _matches.GetEnumerator();

    [ExcludeFromCodeCoverage]
    IEnumerator IEnumerable.GetEnumerator() => GetEnumerator();

    /// <summary>
    /// Creates a client from the store
    /// </summary>
    /// <param name="name">name of the client</param>
    /// <returns>http client</returns>
    public HttpClient CreateClient(string name) => new(new DummyHttpMessageHandler(name, this));

    private sealed class DummyHttpMessageHandler : HttpMessageHandler
    {
        private readonly string _clientName;
        private readonly HttpMessageStore _store;

        public DummyHttpMessageHandler(string clientName, HttpMessageStore store)
        {
            _clientName = clientName;
            _store = store;
        }

        protected override Task<HttpResponseMessage> SendAsync(
            HttpRequestMessage request,
            CancellationToken cancellationToken
        ) => _store.MatchOrThrow(_clientName, request);
    }
}
