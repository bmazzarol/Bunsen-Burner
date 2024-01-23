namespace BunsenBurner.Http;

/// <summary>
/// Dummy <see cref="IHttpClientFactory"/> that can be used to mock <see cref="HttpClient"/> for unit testing HTTP.
/// </summary>
public sealed class DummyHttpFactory : IHttpClientFactory
{
    private DummyHttpFactory(HttpMessageStore store) => Store = store;

    /// <summary>
    /// Access to the underlying HTTP message store
    /// </summary>
    public HttpMessageStore Store { get; }

    /// <summary>
    /// Creates a new instance of the factory to start setting up
    /// </summary>
    /// <param name="store">HTTP messages store</param>
    /// <returns>dummy factory</returns>
    public static DummyHttpFactory New(HttpMessageStore? store = default) =>
        new(store ?? HttpMessageStore.New());

    /// <inheritdoc />
    public HttpClient CreateClient(string name) => Store.CreateClient(name);
}
