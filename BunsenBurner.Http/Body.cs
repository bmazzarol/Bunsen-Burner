using System.Text.Json;

namespace BunsenBurner.Http;

/// <summary>
/// Request body
/// </summary>
public sealed record Body
{
    private const string TextContentType = "text/plain";
    private const string JsonContentType = "application/json";

    /// <summary>
    /// Content type
    /// </summary>
    public string ContentType { get; init; }

    /// <summary>
    /// Data
    /// </summary>
    public string Data { get; init; }

    private Body(string contentType, string data)
    {
        ContentType = contentType;
        Data = data;
    }

    /// <summary>
    /// Text body
    /// </summary>
    /// <param name="data">data</param>
    /// <returns>text body</returns>
    public static Body Text(string data) => New(TextContentType, data);

    /// <summary>
    /// Json body
    /// </summary>
    /// <param name="data">data</param>
    /// <returns>json body</returns>
    public static Body Json<T>(T data) => Json(JsonSerializer.Serialize(data));

    /// <summary>
    /// Json body
    /// </summary>
    /// <param name="data">data</param>
    /// <returns>json body</returns>
    public static Body Json(string data) => New(JsonContentType, data);

    /// <summary>
    /// New body
    /// </summary>
    /// <param name="contentType">content type</param>
    /// <param name="data">data</param>
    /// <returns>json body</returns>
    public static Body New(string contentType, string data) => new(contentType, data);
}
