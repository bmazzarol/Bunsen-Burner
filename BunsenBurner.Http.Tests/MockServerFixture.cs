using WireMock.Server;

namespace BunsenBurner.Http.Tests;

public sealed class MockServerFixture : IDisposable
{
    public MockServerFixture() => Server = WireMockServer.Start();

    public WireMockServer Server { get; }

    public void Dispose() => Server.Stop();
}
