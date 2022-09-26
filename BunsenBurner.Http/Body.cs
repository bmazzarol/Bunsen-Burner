namespace BunsenBurner.Http;

/// <summary>
/// Request body
/// </summary>
/// <param name="ContentType">content type</param>
/// <param name="Data">data</param>
public sealed record Body(string ContentType, string Data);
