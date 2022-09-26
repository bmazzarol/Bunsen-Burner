using Microsoft.AspNetCore.TestHost;

namespace BunsenBurner.Http;

using AaaScenario = Scenario<Syntax.Aaa>;

/// <summary>
/// DSL for building tests using an arrange, act, assert syntax
/// </summary>
public static class Aaa
{
    /// <summary>
    /// Arranges a http request
    /// </summary>
    /// <param name="request">request</param>
    /// <typeparam name="TRequest">request</typeparam>
    /// <returns>arranged scenario</returns>
    [Pure]
    public static AaaScenario.Arranged<TRequest> ArrangeRequest<TRequest>(this TRequest request)
        where TRequest : Request => request.ArrangeRequest<TRequest, Syntax.Aaa>();

    /// <summary>
    /// Arranges a http request
    /// </summary>
    /// <param name="name">name/description</param>
    /// <param name="request">request</param>
    /// <typeparam name="TRequest">request</typeparam>
    /// <returns>arranged scenario</returns>
    [Pure]
    public static AaaScenario.Arranged<TRequest> ArrangeRequest<TRequest>(
        this string name,
        TRequest request
    ) where TRequest : Request => name.ArrangeRequest<TRequest, Syntax.Aaa>(request);

    /// <summary>
    /// Makes a call to the test server provided
    /// </summary>
    /// <param name="scenario">arranged scenario</param>
    /// <param name="fn">get the request to use from the data</param>
    /// <param name="server">test server</param>
    /// <typeparam name="TData">arranged data</typeparam>
    /// <typeparam name="TRequest">request</typeparam>
    /// <returns>acted scenario</returns>
    [Pure]
    public static AaaScenario.Acted<TData, Response> ActAndCall<TData, TRequest>(
        this AaaScenario.Arranged<TData> scenario,
        Func<TData, TRequest> fn,
        TestServer server
    ) where TRequest : Request => scenario.ActAndCall<TData, TRequest, Syntax.Aaa>(fn, server);

    /// <summary>
    /// Makes a call to the test server provided
    /// </summary>
    /// <param name="scenario">arranged scenario</param>
    /// <param name="server">test server</param>
    /// <typeparam name="TRequest">request</typeparam>
    /// <returns>acted scenario</returns>
    [Pure]
    public static AaaScenario.Acted<TRequest, Response> ActAndCall<TRequest>(
        this AaaScenario.Arranged<TRequest> scenario,
        TestServer server
    ) where TRequest : Request => scenario.ActAndCall<TRequest, Syntax.Aaa>(server);

    /// <summary>
    /// Makes a call to the real server
    /// </summary>
    /// <param name="scenario">arranged scenario</param>
    /// <param name="fn">get the request to use from the data</param>
    /// <typeparam name="TData">arranged data</typeparam>
    /// <typeparam name="TRequest">request</typeparam>
    /// <returns>acted scenario</returns>
    [Pure]
    public static AaaScenario.Acted<TData, Response> ActAndCall<TData, TRequest>(
        this AaaScenario.Arranged<TData> scenario,
        Func<TData, TRequest> fn
    ) where TRequest : Request => scenario.ActAndCall<TData, TRequest, Syntax.Aaa>(fn);

    /// <summary>
    /// Makes a call to the real server
    /// </summary>
    /// <param name="scenario">arranged scenario</param>
    /// <typeparam name="TRequest">request</typeparam>
    /// <returns>acted scenario</returns>
    [Pure]
    public static AaaScenario.Acted<TRequest, Response> ActAndCall<TRequest>(
        this AaaScenario.Arranged<TRequest> scenario
    ) where TRequest : Request => scenario.ActAndCall<Syntax.Aaa, TRequest>();
}
