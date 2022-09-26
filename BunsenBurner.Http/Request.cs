using System.Net.Mime;
using System.Text;
using System.Text.Json;
using Flurl;

// ReSharper disable InconsistentNaming

namespace BunsenBurner.Http;

using Headers = IEnumerable<Header>;

/// <summary>
/// HTTP Request
/// </summary>
public abstract record Request
{
    private const string HttpsLocalhost = "https://localhost";

    /// <summary>
    /// Verb
    /// </summary>
    public readonly string Verb;

    /// <summary>
    /// Url
    /// </summary>
    public readonly Url Url;

    /// <summary>
    /// Headers
    /// </summary>
    public Headers Headers { get; init; }

    private Request(string verb, Url url, Headers headers)
    {
        Verb = verb.ToUpper();
        Url = url.IsRelative ? HttpsLocalhost + url : url;
        Headers = headers;
    }

    internal abstract Body? InternalBody();

    public sealed record Get(Url Url, Headers Headers) : Request(nameof(Get), Url, Headers)
    {
        internal override Body? InternalBody() => default;
    }

    /// <summary>
    /// Create a new GET request
    /// </summary>
    /// <param name="url">url</param>
    /// <param name="headers">optional headers</param>
    /// <returns>get request</returns>
    [Pure]
    public static Get GET(Url url, Headers? headers = default) =>
        new(url, headers ?? Array.Empty<Header>());

    public sealed record Post(Url Url, Headers Headers, Body? Body)
        : Request(nameof(Post), Url, Headers)
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
    public static Post POST(Url url, Body? body, Headers? headers = default) =>
        new(url, headers ?? Array.Empty<Header>(), body);

    /// <summary>
    /// Create a new POST request
    /// </summary>
    /// <param name="url">url</param>
    /// <param name="data">post body</param>
    /// <param name="headers">optional headers</param>
    /// <returns>post request</returns>
    [Pure]
    public static Post POST<T>(Url url, T data, Headers? headers = default) =>
        POST(
            url,
            new Body(MediaTypeNames.Application.Json, JsonSerializer.Serialize(data)),
            headers
        );

    public sealed record Put(Url Url, Headers Headers, Body Body)
        : Request(nameof(Put), Url, Headers)
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
    public static Put PUT(Url url, Body body, Headers? headers = default) =>
        new(url, headers ?? Array.Empty<Header>(), body);

    /// <summary>
    /// Create a new PUT request
    /// </summary>
    /// <param name="url">url</param>
    /// <param name="data">put body</param>
    /// <param name="headers">optional headers</param>
    /// <returns>put request</returns>
    [Pure]
    public static Put PUT<T>(Url url, T data, Headers? headers = default) =>
        PUT(
            url,
            new Body(MediaTypeNames.Application.Json, JsonSerializer.Serialize(data)),
            headers
        );

    public sealed record Patch(Url Url, Headers Headers, Body Body)
        : Request(nameof(Patch), Url, Headers)
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
    public static Patch PATCH(Url url, Body body, Headers? headers = default) =>
        new(url, headers ?? Array.Empty<Header>(), body);

    /// <summary>
    /// Create a new PATCH request
    /// </summary>
    /// <param name="url">url</param>
    /// <param name="data">patch body</param>
    /// <param name="headers">optional headers</param>
    /// <returns>patch request</returns>
    [Pure]
    public static Patch PATCH<T>(Url url, T data, Headers? headers = default) =>
        PATCH(
            url,
            new Body(MediaTypeNames.Application.Json, JsonSerializer.Serialize(data)),
            headers
        );

    public sealed record Delete(Url Url, Headers Headers) : Request(nameof(Delete), Url, Headers)
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
    public static Delete DELETE(Url url, Headers? headers = default) =>
        new(url, headers ?? Array.Empty<Header>());

    public sealed record Option(Url Url, Headers Headers) : Request(nameof(Option), Url, Headers)
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
    public static Option OPTION(Url url, Headers? headers = default) =>
        new(url, headers ?? Array.Empty<Header>());

    public sealed record Head(Url Url, Headers Headers) : Request(nameof(Head), Url, Headers)
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
    public static Head HEAD(Url url, Headers? headers = default) =>
        new(url, headers ?? Array.Empty<Header>());

    public sealed record Trace(Url Url, Headers Headers) : Request(nameof(Trace), Url, Headers)
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
    public static Trace TRACE(Url url, Headers? headers = default) =>
        new(url, headers ?? Array.Empty<Header>());

    public sealed record Connect(Url Url, Headers Headers) : Request(nameof(Connect), Url, Headers)
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
    public static Connect CONNECT(Url url, Headers? headers = default) =>
        new(url, headers ?? Array.Empty<Header>());

    /// <summary>
    /// Converts a request to a http request message to send.
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

        foreach (var (key, value) in Headers)
            requestMessage.Headers.Add(key, value);

        return requestMessage;
    }
}

public static class RequestExt
{
    /// <summary>
    /// Add a new header to the request
    /// </summary>
    /// <remarks>if the key exists the value is appended, only unique values are maintained</remarks>
    /// <param name="request">request</param>
    /// <param name="key">header key/name</param>
    /// <param name="value">header value</param>
    /// <typeparam name="TRequest">valid request type</typeparam>
    /// <returns>request with header added</returns>
    public static TRequest WithHeader<TRequest>(this TRequest request, string key, string value)
        where TRequest : Request =>
        request with
        {
            Headers = request.Headers
                .Where(x => x.Key != key)
                .Append(
                    new Header(
                        key,
                        string.Join(
                            ",",
                            request.Headers
                                .Where(x => x.Key == key)
                                .SelectMany(x => x.Value.Split(","))
                                .Append(value)
                                .Select(x => x.Trim())
                                .Distinct(StringComparer.Ordinal)
                        )
                    )
                )
        };

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
    public static TRequest WithHeader<TRequest, T>(
        this TRequest request,
        string key,
        T value,
        Func<T, string> fn
    ) where TRequest : Request => request.WithHeader(key, fn(value));

    /// <summary>
    /// Add new headers to the request
    /// </summary>
    /// <remarks>if the key exists the value is appended, only unique values are maintained</remarks>
    /// <param name="request">request</param>
    /// <param name="headers">headers to add</param>
    /// <typeparam name="TRequest">valid request type</typeparam>
    /// <returns>request with header added</returns>
    public static TRequest WithHeaders<TRequest>(this TRequest request, params Header[] headers)
        where TRequest : Request =>
        headers.Aggregate(request, (r, h) => r.WithHeader(h.Key, h.Value));
}
