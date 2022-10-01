using Microsoft.AspNetCore.TestHost;

namespace BunsenBurner.Http;

using BddScenario = Scenario<Syntax.Bdd>;

/// <summary>
/// DSL for building tests using a given, when, then syntax
/// </summary>
public static class Bdd
{
    /// <summary>
    /// Given a http request
    /// </summary>
    /// <param name="request">request</param>
    /// <typeparam name="TRequest">request</typeparam>
    /// <returns>scenario with the given request data</returns>
    [Pure]
    public static BddScenario.Arranged<TRequest> GivenRequest<TRequest>(this TRequest request)
        where TRequest : Request => request.ArrangeRequest<TRequest, Syntax.Bdd>();

    /// <summary>
    /// Given a http request
    /// </summary>
    /// <param name="name">name/description</param>
    /// <param name="request">request</param>
    /// <typeparam name="TRequest">request</typeparam>
    /// <returns>scenario with the given request data</returns>
    [Pure]
    public static BddScenario.Arranged<TRequest> GivenRequest<TRequest>(
        this string name,
        TRequest request
    ) where TRequest : Request => name.ArrangeRequest<TRequest, Syntax.Bdd>(request);

    /// <summary>
    /// Makes a call to the test server provided
    /// </summary>
    /// <param name="scenario">scenario</param>
    /// <param name="fn">get the request to use from the data</param>
    /// <param name="server">test server</param>
    /// <typeparam name="TData">given data</typeparam>
    /// <typeparam name="TRequest">request</typeparam>
    /// <returns>scenario that is run</returns>
    [Pure]
    public static BddScenario.Acted<TData, ResponseContext> WhenCalled<TData, TRequest>(
        this BddScenario.Arranged<TData> scenario,
        Func<TData, TRequest> fn,
        TestServer server
    ) where TRequest : Request => scenario.ActAndCall(fn, server);

    /// <summary>
    /// Makes a call to the test server provided
    /// </summary>
    /// <param name="scenario">scenario</param>
    /// <param name="server">test server</param>
    /// <typeparam name="TRequest">request</typeparam>
    /// <returns>scenario that is run</returns>
    [Pure]
    public static BddScenario.Acted<TRequest, ResponseContext> WhenCalled<TRequest>(
        this BddScenario.Arranged<TRequest> scenario,
        TestServer server
    ) where TRequest : Request => scenario.ActAndCall(server);

    /// <summary>
    /// Makes a call to the real server
    /// </summary>
    /// <param name="scenario">scenario</param>
    /// <param name="fn">get the request to use from the data</param>
    /// <typeparam name="TData">given data</typeparam>
    /// <typeparam name="TRequest">request</typeparam>
    /// <returns>scenario that is run</returns>
    [Pure]
    public static BddScenario.Acted<TData, Response> WhenCalled<TData, TRequest>(
        this BddScenario.Arranged<TData> scenario,
        Func<TData, TRequest> fn
    ) where TRequest : Request => scenario.ActAndCall(fn);

    /// <summary>
    /// Makes a call to the real server
    /// </summary>
    /// <param name="scenario">scenario</param>
    /// <typeparam name="TRequest">request</typeparam>
    /// <returns>scenario that is run</returns>
    [Pure]
    public static BddScenario.Acted<TRequest, Response> WhenCalled<TRequest>(
        this BddScenario.Arranged<TRequest> scenario
    ) where TRequest : Request => scenario.ActAndCall();
}
