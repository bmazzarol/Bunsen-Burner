﻿namespace BunsenBurner.Http;

/// <summary>
/// Dummy Http Factory that can be used to mock http clients for unit testing HTTP.
/// </summary>
public sealed class DummyHttpFactory : IHttpClientFactory
{
    private readonly HttpMessageStore _store;

    private DummyHttpFactory(HttpMessageStore store) => _store = store;

    /// <summary>
    /// Access to the underlying HTTP message store
    /// </summary>
    public HttpMessageStore Store => _store;

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
