using System.Globalization;
using System.Text;
using Flurl;

// ReSharper disable InconsistentNaming

namespace BunsenBurner.Http;

using InternalHeaders = Set<Header.OrdHeader, Header>;

/// <summary>
/// HTTP Request
/// </summary>
public abstract record Request
{
    /// <summary>
    /// Verb
    /// </summary>
    public string Verb { get; }

    /// <summary>
    /// Url
    /// </summary>
    public Url Url { get; init; }

    /// <summary>
    /// Headers
    /// </summary>
    public InternalHeaders Headers { get; init; }

    private Request(string verb, Url url, InternalHeaders headers)
    {
        Verb = verb.ToUpper(CultureInfo.InvariantCulture);
        Url = url.AsAbsoluteUrl();
        Headers = headers;
    }

    internal abstract Body? InternalBody();

    /// <summary>
    /// Request content
    /// </summary>
    /// <returns>request content</returns>
    public string? Content() => InternalBody()?.Data;

    /// <summary>
    /// Content type
    /// </summary>
    /// <returns>content type</returns>
    public string? ContentType() => InternalBody()?.ContentType;

    /// <summary>
    /// Content length
    /// </summary>
    /// <returns>content length</returns>
    public long? ContentLength() => Content()?.Length;

    /// <inheritdoc />
    public sealed override string ToString()
    {
        var headers = Headers.IsEmpty
            ? string.Empty
            : $" {string.Join(" ", Headers.Select(h => $@"-H ""{h.Name}: {h.Value}"""))}";
        var body = Content() != null ? $" -d {Content()}" : string.Empty;
        return $"Request: -X {Verb} {Url}{headers}{body}";
    }

    /// <summary>
    /// Get request
    /// </summary>
    /// <param name="Url">url</param>
    /// <param name="Headers">headers</param>
    public sealed record GetRequest(Url Url, InternalHeaders Headers)
        : Request(nameof(GET), Url, Headers)
    {
        internal override Body? InternalBody() => default;
    }

    private static InternalHeaders AsInternal(IEnumerable<Header> headers) =>
        InternalHeaders.Empty.TryAddRange(headers);

    /// <summary>
    /// Create a new GET request
    /// </summary>
    /// <param name="url">url</param>
    /// <param name="headers">optional headers</param>
    /// <returns>get request</returns>
    [Pure]
    public static GetRequest GET(Url url, params Header[] headers) => new(url, AsInternal(headers));

    /// <summary>
    /// Post request
    /// </summary>
    /// <param name="Url">url</param>
    /// <param name="Headers">headers</param>
    /// <param name="Body">post body</param>
    public sealed record PostRequest(Url Url, InternalHeaders Headers, Body? Body)
        : Request(nameof(POST), Url, Headers)
    {
        internal override Body? InternalBody() => Body;
    }

    /// <summary>
    /// Create a new POST request
    /// </summary>
    /// <param name="url">url</param>
    /// <param name="body">post body</param>
    /// <param name="headers">optional headers</param>
    /// <returns>post request</returns>
    [Pure]
    public static PostRequest POST(Url url, Body? body, params Header[] headers) =>
        new(url, AsInternal(headers), body);

    /// <summary>
    /// Create a new POST request
    /// </summary>
    /// <param name="url">url</param>
    /// <param name="data">post body</param>
    /// <param name="headers">optional headers</param>
    /// <returns>post request</returns>
    [Pure]
    public static PostRequest POST<T>(Url url, T data, params Header[] headers) =>
        POST(url, Body.Json(data), headers);

    /// <summary>
    /// Put request
    /// </summary>
    /// <param name="Url">url</param>
    /// <param name="Headers">headers</param>
    /// <param name="Body">put body</param>
    public sealed record PutRequest(Url Url, InternalHeaders Headers, Body Body)
        : Request(nameof(PUT), Url, Headers)
    {
        internal override Body? InternalBody() => Body;
    }

    /// <summary>
    /// Create a new PUT request
    /// </summary>
    /// <param name="url">url</param>
    /// <param name="body">put body</param>
    /// <param name="headers">optional headers</param>
    /// <returns>put request</returns>
    [Pure]
    public static PutRequest PUT(Url url, Body body, params Header[] headers) =>
        new(url, AsInternal(headers), body);

    /// <summary>
    /// Create a new PUT request
    /// </summary>
    /// <param name="url">url</param>
    /// <param name="data">put body</param>
    /// <param name="headers">optional headers</param>
    /// <returns>put request</returns>
    [Pure]
    public static PutRequest PUT<T>(Url url, T data, params Header[] headers) =>
        PUT(url, Body.Json(data), headers);

    /// <summary>
    /// Patch request
    /// </summary>
    /// <param name="Url">url</param>
    /// <param name="Headers">headers</param>
    /// <param name="Body">patch body</param>
    public sealed record PatchRequest(Url Url, InternalHeaders Headers, Body Body)
        : Request(nameof(PATCH), Url, Headers)
    {
        internal override Body? InternalBody() => Body;
    }

    /// <summary>
    /// Create a new PATCH request
    /// </summary>
    /// <param name="url">url</param>
    /// <param name="body">patch body</param>
    /// <param name="headers">optional headers</param>
    /// <returns>patch request</returns>
    [Pure]
    public static PatchRequest PATCH(Url url, Body body, params Header[] headers) =>
        new(url, AsInternal(headers), body);

    /// <summary>
    /// Create a new PATCH request
    /// </summary>
    /// <param name="url">url</param>
    /// <param name="data">patch body</param>
    /// <param name="headers">optional headers</param>
    /// <returns>patch request</returns>
    [Pure]
    public static PatchRequest PATCH<T>(Url url, T data, params Header[] headers) =>
        PATCH(url, Body.Json(data), headers);

    /// <summary>
    /// Delete request
    /// </summary>
    /// <param name="Url">url</param>
    /// <param name="Headers">headers</param>
    public sealed record DeleteRequest(Url Url, InternalHeaders Headers)
        : Request(nameof(DELETE), Url, Headers)
    {
        internal override Body? InternalBody() => default;
    }

    /// <summary>
    /// Create a new DELETE request
    /// </summary>
    /// <param name="url">url</param>
    /// <param name="headers">optional headers</param>
    /// <returns>delete request</returns>
    [Pure]
    public static DeleteRequest DELETE(Url url, params Header[] headers) =>
        new(url, AsInternal(headers));

    /// <summary>
    /// Option request
    /// </summary>
    /// <param name="Url">url</param>
    /// <param name="Headers">headers</param>
    public sealed record OptionRequest(Url Url, InternalHeaders Headers)
        : Request(nameof(OPTION), Url, Headers)
    {
        internal override Body? InternalBody() => default;
    }

    /// <summary>
    /// Create a new OPTION request
    /// </summary>
    /// <param name="url">url</param>
    /// <param name="headers">optional headers</param>
    /// <returns>option request</returns>
    [Pure]
    public static OptionRequest OPTION(Url url, params Header[] headers) =>
        new(url, AsInternal(headers));

    /// <summary>
    /// Head request
    /// </summary>
    /// <param name="Url">url</param>
    /// <param name="Headers">headers</param>
    public sealed record HeadRequest(Url Url, InternalHeaders Headers)
        : Request(nameof(HEAD), Url, Headers)
    {
        internal override Body? InternalBody() => default;
    }

    /// <summary>
    /// Create a new HEAD request
    /// </summary>
    /// <param name="url">url</param>
    /// <param name="headers">optional headers</param>
    /// <returns>head request</returns>
    [Pure]
    public static HeadRequest HEAD(Url url, params Header[] headers) =>
        new(url, AsInternal(headers));

    /// <summary>
    /// Trace request
    /// </summary>
    /// <param name="Url">url</param>
    /// <param name="Headers">headers</param>
    public sealed record TraceRequest(Url Url, InternalHeaders Headers)
        : Request(nameof(TRACE), Url, Headers)
    {
        internal override Body? InternalBody() => default;
    }

    /// <summary>
    /// Create a new TRACE request
    /// </summary>
    /// <param name="url">url</param>
    /// <param name="headers">optional headers</param>
    /// <returns>trace request</returns>
    [Pure]
    public static TraceRequest TRACE(Url url, params Header[] headers) =>
        new(url, AsInternal(headers));

    /// <summary>
    /// Connect request
    /// </summary>
    /// <param name="Url">url</param>
    /// <param name="Headers">headers</param>
    public sealed record ConnectRequest(Url Url, InternalHeaders Headers)
        : Request(nameof(CONNECT), Url, Headers)
    {
        internal override Body? InternalBody() => default;
    }

    /// <summary>
    /// Create a new CONNECT request
    /// </summary>
    /// <param name="url">url</param>
    /// <param name="headers">optional headers</param>
    /// <returns>connect request</returns>
    [Pure]
    public static ConnectRequest CONNECT(Url url, params Header[] headers) =>
        new(url, AsInternal(headers));

    /// <summary>
    /// Converts a request to a http request message to send
    /// </summary>
    /// <returns>http request message</returns>
    [Pure]
    public HttpRequestMessage HttpRequestMessage()
    {
        var requestMessage = new HttpRequestMessage
        {
            Method = new HttpMethod(Verb),
            RequestUri = new Uri(Url.ToString()),
            Content = InternalBody() is { Data: not null, ContentType: not null } x
                ? new StringContent(x.Data, Encoding.UTF8, x.ContentType)
                : default
        };

        foreach (var header in Headers)
            requestMessage.Headers.Add(header.Name, header);

        return requestMessage;
    }
}

/// <summary>
/// Extension for all requests
/// </summary>
public static class RequestExtensions
{
    private const string HttpsLocalhost = "https://localhost";

    /// <summary>
    /// Add a new header to the request
    /// </summary>
    /// <remarks>if the key exists the value is appended, only unique values are maintained</remarks>
    /// <param name="request">request</param>
    /// <param name="key">header key/name</param>
    /// <param name="value">header value</param>
    /// <typeparam name="TRequest">valid request type</typeparam>
    /// <returns>request with header added</returns>
    [Pure]
    public static TRequest WithHeader<TRequest>(this TRequest request, string key, string value)
        where TRequest : Request => request.WithHeader(key, new[] { value });

    /// <summary>
    /// Add a new header to the request
    /// </summary>
    /// <remarks>if the key exists the value is appended, only unique values are maintained</remarks>
    /// <param name="request">request</param>
    /// <param name="key">header key/name</param>
    /// <param name="value">header value</param>
    /// <param name="fn">T to string function</param>
    /// <typeparam name="TRequest">valid request type</typeparam>
    /// <typeparam name="T">some T</typeparam>
    /// <returns>request with header added</returns>
    [Pure]
    public static TRequest WithHeader<TRequest, T>(
        this TRequest request,
        string key,
        T value,
        Func<T, string> fn
    ) where TRequest : Request => request.WithHeader(key, fn(value));

    /// <summary>
    /// Add a new header to the request
    /// </summary>
    /// <remarks>if the key exists the value is appended, only unique values are maintained</remarks>
    /// <param name="request">request</param>
    /// <param name="key">header</param>
    /// <param name="values">values</param>
    /// <typeparam name="TRequest">valid request type</typeparam>
    /// <returns>request with header added</returns>
    [Pure]
    public static TRequest WithHeader<TRequest>(
        this TRequest request,
        string key,
        params string[] values
    ) where TRequest : Request => request.WithHeader(key, values.AsEnumerable());

    /// <summary>
    /// Add a new header to the request
    /// </summary>
    /// <remarks>if the key exists the value is appended, only unique values are maintained</remarks>
    /// <param name="request">request</param>
    /// <param name="key">header</param>
    /// <param name="values">values</param>
    /// <typeparam name="TRequest">valid request type</typeparam>
    /// <returns>request with header added</returns>
    [Pure]
    public static TRequest WithHeader<TRequest>(
        this TRequest request,
        string key,
        IEnumerable<string> values
    ) where TRequest : Request
    {
        bool Pred(Header h) => string.Equals(h.Name, key, StringComparison.Ordinal);
        var header = request.Headers.Find(Pred).IfNone(Header.New(key));
        var headers = request.Headers
            .Filter(h => !Pred(h))
            .TryAdd(header.WithValues(values.ToArray()));
        return request with { Headers = headers };
    }

    /// <summary>
    /// Add new headers to the request
    /// </summary>
    /// <remarks>if the key exists the value is appended, only unique values are maintained</remarks>
    /// <param name="request">request</param>
    /// <param name="headers">headers</param>
    /// <typeparam name="TRequest">valid request type</typeparam>
    /// <returns>request with headers added</returns>
    [Pure]
    public static TRequest WithHeaders<TRequest>(this TRequest request, params Header[] headers)
        where TRequest : Request =>
        headers.Aggregate(request, (req, header) => req.WithHeader(header.Name, header));

    [Pure]
    internal static Url AsAbsoluteUrl(this Url url) => url.IsRelative ? HttpsLocalhost + url : url;

    /// <summary>
    /// Sets the url for the request
    /// </summary>
    /// <param name="request">request</param>
    /// <param name="setter">url setter</param>
    /// <typeparam name="TRequest">valid request type</typeparam>
    /// <returns>request with the url set</returns>
    [Pure]
    public static TRequest WithUrl<TRequest>(this TRequest request, Func<Url, Url> setter)
        where TRequest : Request => request with { Url = setter(request.Url).AsAbsoluteUrl() };

    /// <summary>
    /// Sets the url for the request
    /// </summary>
    /// <param name="request">request</param>
    /// <param name="url">url to set</param>
    /// <typeparam name="TRequest">valid request type</typeparam>
    /// <returns>request with the url set</returns>
    [Pure]
    public static TRequest WithUrl<TRequest>(this TRequest request, Url url)
        where TRequest : Request => request.WithUrl(_ => url);

    /// <summary>
    /// Adds bearer authentication
    /// </summary>
    /// <param name="request">request</param>
    /// <param name="token">bearer token (JWT)</param>
    /// <typeparam name="TRequest">valid request type</typeparam>
    /// <returns>request with bearer authentication token</returns>
    [Pure]
    public static TRequest WithBearerToken<TRequest>(this TRequest request, Token token)
        where TRequest : Request => request.WithHeader("Authorization", $"Bearer {token.Encode()}");

    /// <summary>
    /// Adds bearer authentication
    /// </summary>
    /// <param name="request">request</param>
    /// <param name="fn">bearer token config function (JWT)</param>
    /// <typeparam name="TRequest">valid request type</typeparam>
    /// <returns>request with bearer authentication token</returns>
    [Pure]
    public static TRequest WithBearerToken<TRequest>(this TRequest request, Func<Token, Token> fn)
        where TRequest : Request => request.WithBearerToken(fn(Token.New()));

    /// <summary>
    /// Gets the header value for the key
    /// </summary>
    /// <param name="headers">headers</param>
    /// <param name="key">key</param>
    /// <returns>header value</returns>
    [Pure]
    public static string? Get(this InternalHeaders headers, string key) =>
        headers
            .Find(h => string.Equals(h.Name, key, StringComparison.Ordinal))
            .Select(x => x.Aggregate(string.Empty, (a, b) => a != string.Empty ? $"{a},{b}" : b))
            .IfNoneUnsafe(() => null);
}
