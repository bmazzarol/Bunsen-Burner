using BunsenBurner.Logging;
using Microsoft.AspNetCore.TestHost;
using Microsoft.Extensions.DependencyInjection;

namespace BunsenBurner.Http.Extensions;

using TestServerResult = Task<(HttpResponseMessage Response, LogMessageStore Store)>;

/// <summary>
/// Extension methods for integrating <see cref="TestServer"/> into Bunsen Burner
/// </summary>
public static class TestServerExtensions
{
    /// <summary>
    /// Returns a delegate that can be used in an Act step
    /// </summary>
    /// <remarks>Assumes the test server was built using <see cref="TestServerBuilder.Create"/></remarks>
    /// <param name="testServer"><see cref="TestServer"/></param>
    /// <param name="request"><see cref="HttpRequestMessage"/> to send</param>
    /// <returns>act step</returns>
    public static Task<HttpResponseMessage> CallTestServer(
        this TestServer testServer,
        HttpRequestMessage request
    )
    {
        return testServer.CreateClient().SendAsync(request);
    }

    /// <summary>
    /// Returns a delegate that can be used in an Act step
    /// </summary>
    /// <remarks>Assumes the test server was built using <see cref="TestServerBuilder.Create"/></remarks>
    /// <param name="testServer"><see cref="TestServer"/></param>
    /// <returns>act step</returns>
    public static Func<HttpRequestMessage, TestServerResult> CallTestServer(
        this TestServer testServer
    ) =>
        async req =>
            (
                Response: await testServer.CallTestServer(req),
                Store: testServer.Services.GetRequiredService<LogMessageStore>()
            );
}
