﻿namespace BunsenBurner.Http;

/// <summary>
/// Shared constants
/// </summary>
public static class Constants
{
    /// <summary>
    /// Default test environment name
    /// </summary>
    public const string TestEnvironmentName = "testing";

    /// <summary>
    /// Test issuer for the in memory JWT tokens
    /// </summary>
    public const string TestIssuer = "https://localhost/dev/";

    /// <summary>
    /// Test signing key used for test authentication
    /// </summary>
    public const string TestSigningKey = "SECRET_SIGNING_KEY";
}
