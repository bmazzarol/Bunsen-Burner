using System.Linq.Expressions;
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
    /// <returns>arranged scenario</returns>
    [Pure]
    public static AaaScenario.Arranged<HttpRequestMessage> ArrangeRequest(
        this HttpRequestMessage request
    ) => request.ArrangeRequest<Syntax.Aaa>();

    /// <summary>
    /// Arranges a http request
    /// </summary>
    /// <param name="name">name/description</param>
    /// <param name="request">request</param>
    /// <returns>arranged scenario</returns>
    [Pure]
    public static AaaScenario.Arranged<HttpRequestMessage> ArrangeRequest(
        this string name,
        HttpRequestMessage request
    ) => name.ArrangeRequest<Syntax.Aaa>(request);

    /// <summary>
    /// Makes a call to the test server provided
    /// </summary>
    /// <param name="scenario">arranged scenario</param>
    /// <param name="fn">get the request to use from the data</param>
    /// <param name="server">test server</param>
    /// <typeparam name="TData">arranged data</typeparam>
    /// <returns>acted scenario</returns>
    [Pure]
    public static AaaScenario.Acted<TData, ResponseContext> ActAndCall<TData>(
        this AaaScenario.Arranged<TData> scenario,
        Func<TData, HttpRequestMessage> fn,
        Func<TData, TestServer> server
    ) => scenario.ActAndCall<TData, Syntax.Aaa>(fn, server);

    /// <summary>
    /// Makes a call to the test server provided
    /// </summary>
    /// <param name="scenario">arranged scenario</param>
    /// <param name="server">test server</param>
    /// <returns>acted scenario</returns>
    [Pure]
    public static AaaScenario.Acted<HttpRequestMessage, ResponseContext> ActAndCall(
        this AaaScenario.Arranged<HttpRequestMessage> scenario,
        TestServer server
    ) => scenario.ActAndCall<Syntax.Aaa>(server);

    /// <summary>
    /// Makes a call to the real server
    /// </summary>
    /// <param name="scenario">arranged scenario</param>
    /// <param name="fn">get the request to use from the data</param>
    /// <param name="clientFactory">optional http client factory, can be used to customize the client</param>
    /// <typeparam name="TData">arranged data</typeparam>
    /// <returns>acted scenario</returns>
    [Pure]
    public static AaaScenario.Acted<TData, HttpResponseMessage> ActAndCall<TData>(
        this AaaScenario.Arranged<TData> scenario,
        Func<TData, HttpRequestMessage> fn,
        Func<HttpClient>? clientFactory = default
    ) => scenario.ActAndCall<TData, Syntax.Aaa>(fn, clientFactory);

    /// <summary>
    /// Makes a call to the real server
    /// </summary>
    /// <param name="scenario">arranged scenario</param>
    /// <param name="clientFactory">optional http client factory, can be used to customize the client</param>
    /// <returns>acted scenario</returns>
    [Pure]
    public static AaaScenario.Acted<HttpRequestMessage, HttpResponseMessage> ActAndCall(
        this AaaScenario.Arranged<HttpRequestMessage> scenario,
        Func<HttpClient>? clientFactory = default
    ) => scenario.ActAndCall<Syntax.Aaa>(clientFactory);

    /// <summary>
    /// Makes a call repeatedly to the real server, stops once the predicate is true or the schedule completes
    /// </summary>
    /// <param name="scenario">arranged scenario</param>
    /// <param name="fn">get the request to use from the data</param>
    /// <param name="schedule">provided schedule, can be used to determine the number and duration of waits between each call</param>
    /// <param name="predicate">predicate against the responses returned, when true the response is returned</param>
    /// <param name="clientFactory">optional http client factory, can be used to customize the client</param>
    /// <param name="maxRunDuration">maximum time the operation will run for before failing; defaults to 1 minute</param>
    /// <exception cref="InvalidOperationException">if the schedule completes before the provided predicate returns true</exception>
    /// <typeparam name="TData">arranged data</typeparam>
    /// <returns>acted scenario</returns>
    [Pure]
    public static AaaScenario.Acted<TData, HttpResponseMessage> ActAndCallUntil<TData>(
        this AaaScenario.Arranged<TData> scenario,
        Func<TData, HttpRequestMessage> fn,
        Schedule schedule,
        Expression<Func<HttpResponseMessage, bool>> predicate,
        Func<HttpClient>? clientFactory = default,
        TimeSpan? maxRunDuration = default
    ) =>
        scenario.ActAndCallUntil<TData, Syntax.Aaa>(
            fn,
            schedule,
            predicate,
            clientFactory,
            maxRunDuration
        );

    /// <summary>
    /// Makes a call repeatedly to the real server, stops once the predicate is true or the schedule completes
    /// </summary>
    /// <param name="scenario">arranged scenario</param>
    /// <param name="schedule">provided schedule, can be used to determine the number and duration of waits between each call</param>
    /// <param name="predicate">predicate against the responses returned, when true the response is returned</param>
    /// <param name="clientFactory">optional http client factory, can be used to customize the client</param>
    /// <param name="maxRunDuration">maximum time the operation will run for before failing; defaults to 1 minute</param>
    /// <exception cref="InvalidOperationException">if the schedule completes before the provided predicate returns true</exception>
    /// <returns>acted scenario</returns>
    [Pure]
    public static AaaScenario.Acted<HttpRequestMessage, HttpResponseMessage> ActAndCallUntil(
        this AaaScenario.Arranged<HttpRequestMessage> scenario,
        Schedule schedule,
        Expression<Func<HttpResponseMessage, bool>> predicate,
        Func<HttpClient>? clientFactory = default,
        TimeSpan? maxRunDuration = default
    ) => scenario.ActAndCallUntil<Syntax.Aaa>(schedule, predicate, clientFactory, maxRunDuration);
}
