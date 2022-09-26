namespace BunsenBurner.Http;

/// <summary>
/// Request header
/// </summary>
/// <param name="Key">key</param>
/// <param name="Value">value</param>
public sealed record Header(string Key, string Value);
