using System.Net;

namespace BunsenBurner.Http;

using InternalHeaders = Set<Header.OrdHeader, Header>;

/// <summary>
/// HTTP Response
/// </summary>
public sealed record Response
{
    /// <summary>
    /// Raw status code
    /// </summary>
    public int RawStatusCode { get; }

    /// <summary>
    /// Content length
    /// </summary>
    public int? Length { get; }

    /// <summary>
    /// Status code
    /// </summary>
    public HttpStatusCode Code { get; init; }

    /// <summary>
    /// Response content
    /// </summary>
    public string? Content { get; init; }

    /// <summary>
    /// Media type
    /// </summary>
    public string? MediaType { get; init; }

    /// <summary>
    /// Response headers
    /// </summary>
    public InternalHeaders Headers { get; init; }

    private Response(
        HttpStatusCode code,
        string? content,
        string? mediaType,
        InternalHeaders headers
    )
    {
        Code = code;
        Content = content;
        MediaType = mediaType;
        Headers = headers;
        RawStatusCode = (int)code;
        Length = content?.Length;
    }

    /// <summary>
    /// Creates a new header
    /// </summary>
    /// <param name="code">response code</param>
    /// <param name="content">response content</param>
    /// <param name="mediaType">media type</param>
    /// <param name="headers">headers</param>
    /// <returns>response</returns>
    public static Response New(
        HttpStatusCode code,
        string? content,
        string? mediaType,
        params Header[] headers
    ) => new(code, content, mediaType, InternalHeaders.Empty.TryAddRange(headers));

    internal static async Task<Response> New(HttpResponseMessage httpResp)
    {
        var content = await httpResp.Content.ReadAsStringAsync();
        return new Response(
            httpResp.StatusCode,
            content,
            httpResp.Content.Headers.ContentType?.MediaType,
            InternalHeaders.Empty.TryAddRange(
                httpResp.Headers.Select(kv => Header.New(kv.Key, kv.Value.ToArray()))
            )
        );
    }
}
