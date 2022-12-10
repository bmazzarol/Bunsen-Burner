using System.Linq.Expressions;
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
        Func<TData, TestServer> server
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
    /// <param name="clientFactory">optional http client factory, can be used to customize the client</param>
    /// <typeparam name="TData">given data</typeparam>
    /// <typeparam name="TRequest">request</typeparam>
    /// <returns>scenario that is run</returns>
    [Pure]
    public static BddScenario.Acted<TData, Response> WhenCalled<TData, TRequest>(
        this BddScenario.Arranged<TData> scenario,
        Func<TData, TRequest> fn,
        Func<HttpClient>? clientFactory = default
    ) where TRequest : Request => scenario.ActAndCall(fn, clientFactory);

    /// <summary>
    /// Makes a call to the real server
    /// </summary>
    /// <param name="scenario">scenario</param>
    /// <param name="clientFactory">optional http client factory, can be used to customize the client</param>
    /// <typeparam name="TRequest">request</typeparam>
    /// <returns>scenario that is run</returns>
    [Pure]
    public static BddScenario.Acted<TRequest, Response> WhenCalled<TRequest>(
        this BddScenario.Arranged<TRequest> scenario,
        Func<HttpClient>? clientFactory = default
    ) where TRequest : Request => scenario.ActAndCall(clientFactory);

    /// <summary>
    /// Makes a call repeatedly to the real server, stops once the predicate is true or the schedule completes
    /// </summary>
    /// <param name="scenario">scenario</param>
    /// <param name="fn">get the request to use from the data</param>
    /// <param name="schedule">provided schedule, can be used to determine the number and duration of waits between each call</param>
    /// <param name="predicate">predicate against the responses returned, when true the response is returned</param>
    /// <param name="clientFactory">optional http client factory, can be used to customize the client</param>
    /// <exception cref="InvalidOperationException">if the schedule completes before the provided predicate returns true</exception>
    /// <typeparam name="TData">given data</typeparam>
    /// <typeparam name="TRequest">request</typeparam>
    /// <returns>scenario that is run</returns>
    [Pure]
    public static BddScenario.Acted<TData, Response> WhenCalledRepeatedly<TData, TRequest>(
        this BddScenario.Arranged<TData> scenario,
        Func<TData, TRequest> fn,
        Schedule schedule,
        Expression<Func<Response, bool>> predicate,
        Func<HttpClient>? clientFactory = default
    ) where TRequest : Request => scenario.ActAndCallUntil(fn, schedule, predicate, clientFactory);

    /// <summary>
    /// Makes a call repeatedly to the real server, stops once the predicate is true or the schedule completes
    /// </summary>
    /// <param name="scenario">scenario</param>
    /// <param name="schedule">provided schedule, can be used to determine the number and duration of waits between each call</param>
    /// <param name="predicate">predicate against the responses returned, when true the response is returned</param>
    /// <param name="clientFactory">optional http client factory, can be used to customize the client</param>
    /// <exception cref="InvalidOperationException">if the schedule completes before the provided predicate returns true</exception>
    /// <typeparam name="TRequest">request</typeparam>
    /// <returns>scenario that is run</returns>
    [Pure]
    public static BddScenario.Acted<TRequest, Response> WhenCalledRepeatedly<TRequest>(
        this BddScenario.Arranged<TRequest> scenario,
        Schedule schedule,
        Expression<Func<Response, bool>> predicate,
        Func<HttpClient>? clientFactory = default
    ) where TRequest : Request => scenario.ActAndCallUntil(schedule, predicate, clientFactory);
}
