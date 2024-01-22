namespace BunsenBurner.Http;

/// <summary>
/// Shared constants
/// </summary>
public static class TestServerConstants
{
    /// <summary>
    /// Default test environment name
    /// </summary>
    public const string EnvironmentName = "testing";

    /// <summary>
    /// Test issuer for the in memory JWT tokens
    /// </summary>
    public const string Issuer = "https://localhost/dev/";

    /// <summary>
    /// Test signing key used for test authentication
    /// </summary>
    public const string SigningKey = "SECRET_SIGNING_KEY";
}
