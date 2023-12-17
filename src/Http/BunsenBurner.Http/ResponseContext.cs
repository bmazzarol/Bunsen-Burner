using BunsenBurner.Logging;

namespace BunsenBurner.Http;

/// <summary>
/// Result of running a request against a test server.
/// </summary>
/// <param name="Response">response</param>
/// <param name="Store">log messages store, will be populated if the test service has a singleton LogMessageStore registered</param>
public sealed record ResponseContext(HttpResponseMessage Response, LogMessageStore Store);
