namespace BunsenBurner.Http;

/// <summary>
/// Defines a successful match against a HTTP messages store setup.
/// </summary>
public readonly record struct HttpMessageSetupMatch
{
    /// <summary>
    /// HTTP client name
    /// </summary>
    public string ClientName { get; }

    /// <summary>
    /// Sent HTTP request
    /// </summary>
    public HttpRequestMessage Request { get; }

    /// <summary>
    /// Generated HTTP response
    /// </summary>
    public HttpResponseMessage Response { get; }

    internal HttpMessageSetupMatch(
        string clientName,
        HttpRequestMessage request,
        HttpResponseMessage response
    )
    {
        ClientName = clientName;
        Request = request;
        Response = response;
    }
}
