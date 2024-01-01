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
    /// <returns>scenario with the given request data</returns>
    [Pure]
    public static BddScenario.Arranged<HttpRequestMessage> GivenRequest(
        this HttpRequestMessage request
    ) => request.ArrangeRequest<Syntax.Bdd>();

    /// <summary>
    /// Given a http request
    /// </summary>
    /// <param name="name">name/description</param>
    /// <param name="request">request</param>
    /// <returns>scenario with the given request data</returns>
    [Pure]
    public static BddScenario.Arranged<HttpRequestMessage> GivenRequest(
        this string name,
        HttpRequestMessage request
    ) => name.ArrangeRequest<Syntax.Bdd>(request);

    /// <summary>
    /// Makes a call to the test server provided
    /// </summary>
    /// <param name="scenario">scenario</param>
    /// <param name="fn">get the request to use from the data</param>
    /// <param name="server">test server</param>
    /// <typeparam name="TData">given data</typeparam>
    /// <returns>scenario that is run</returns>
    [Pure]
    public static BddScenario.Acted<TData, ResponseContext> WhenCalled<TData>(
        this BddScenario.Arranged<TData> scenario,
        Func<TData, HttpRequestMessage> fn,
        Func<TData, TestServer> server
    ) => scenario.ActAndCall(fn, server);

    /// <summary>
    /// Makes a call to the test server provided
    /// </summary>
    /// <param name="scenario">scenario</param>
    /// <param name="server">test server</param>
    /// <returns>scenario that is run</returns>
    [Pure]
    public static BddScenario.Acted<HttpRequestMessage, ResponseContext> WhenCalled(
        this BddScenario.Arranged<HttpRequestMessage> scenario,
        TestServer server
    ) => scenario.ActAndCall(server);

    /// <summary>
    /// Makes a call to the real server
    /// </summary>
    /// <param name="scenario">scenario</param>
    /// <param name="fn">get the request to use from the data</param>
    /// <param name="clientFactory">optional http client factory, can be used to customize the client</param>
    /// <typeparam name="TData">given data</typeparam>
    /// <returns>scenario that is run</returns>
    [Pure]
    public static BddScenario.Acted<TData, HttpResponseMessage> WhenCalled<TData>(
        this BddScenario.Arranged<TData> scenario,
        Func<TData, HttpRequestMessage> fn,
        Func<HttpClient>? clientFactory = default
    ) => scenario.ActAndCall(fn, clientFactory);

    /// <summary>
    /// Makes a call to the real server
    /// </summary>
    /// <param name="scenario">scenario</param>
    /// <param name="clientFactory">optional http client factory, can be used to customize the client</param>
    /// <returns>scenario that is run</returns>
    [Pure]
    public static BddScenario.Acted<HttpRequestMessage, HttpResponseMessage> WhenCalled(
        this BddScenario.Arranged<HttpRequestMessage> scenario,
        Func<HttpClient>? clientFactory = default
    ) => scenario.ActAndCall(clientFactory);

    /// <summary>
    /// Makes a call repeatedly to the real server, stops once the predicate is true or the schedule completes
    /// </summary>
    /// <param name="scenario">scenario</param>
    /// <param name="fn">get the request to use from the data</param>
    /// <param name="schedule">provided schedule, can be used to determine the number and duration of waits between each call</param>
    /// <param name="predicate">predicate against the responses returned, when true the response is returned</param>
    /// <param name="clientFactory">optional http client factory, can be used to customize the client</param>
    /// <param name="maxRunDuration">maximum time the operation will run for before failing; defaults to 1 minute</param>
    /// <exception cref="InvalidOperationException">if the schedule completes before the provided predicate returns true</exception>
    /// <typeparam name="TData">given data</typeparam>
    /// <returns>scenario that is run</returns>
    [Pure]
    public static BddScenario.Acted<TData, HttpResponseMessage> WhenCalledRepeatedly<TData>(
        this BddScenario.Arranged<TData> scenario,
        Func<TData, HttpRequestMessage> fn,
        Schedule schedule,
        Expression<Func<HttpResponseMessage, bool>> predicate,
        Func<HttpClient>? clientFactory = default,
        TimeSpan? maxRunDuration = default
    ) => scenario.ActAndCallUntil(fn, schedule, predicate, clientFactory, maxRunDuration);

    /// <summary>
    /// Makes a call repeatedly to the real server, stops once the predicate is true or the schedule completes
    /// </summary>
    /// <param name="scenario">scenario</param>
    /// <param name="schedule">provided schedule, can be used to determine the number and duration of waits between each call</param>
    /// <param name="predicate">predicate against the responses returned, when true the response is returned</param>
    /// <param name="clientFactory">optional http client factory, can be used to customize the client</param>
    /// <param name="maxRunDuration">maximum time the operation will run for before failing; defaults to 1 minute</param>
    /// <exception cref="InvalidOperationException">if the schedule completes before the provided predicate returns true</exception>
    /// <returns>scenario that is run</returns>
    [Pure]
    public static BddScenario.Acted<HttpRequestMessage, HttpResponseMessage> WhenCalledRepeatedly(
        this BddScenario.Arranged<HttpRequestMessage> scenario,
        Schedule schedule,
        Expression<Func<HttpResponseMessage, bool>> predicate,
        Func<HttpClient>? clientFactory = default,
        TimeSpan? maxRunDuration = default
    ) => scenario.ActAndCallUntil(schedule, predicate, clientFactory, maxRunDuration);
}
