using System.Net;
using BunsenBurner.FunctionApp.Models;
using HttpBuildR;
using Microsoft.AspNetCore.Http;
using Microsoft.AspNetCore.Mvc;
using Microsoft.AspNetCore.Mvc.Infrastructure;

namespace BunsenBurner.FunctionApp;

/// <summary>
/// Extends the Request and Response types to work with http trigger functions
/// </summary>
public static class HttpFunctionAppExtensions
{
    /// <summary>
    /// Converts the request to a http request
    /// </summary>
    /// <param name="request">request</param>
    /// <returns>http request</returns>
    [Pure]
    public static async Task<HttpRequest> AsHttpRequest(this HttpRequestMessage request) =>
        new DummyHttpRequest(
            request,
            request.Content != null ? await request.Content.ReadAsStringAsync() : string.Empty
        );

    /// <summary>
    /// Convert object result to a response
    /// </summary>
    /// <param name="actionResult">result</param>
    /// <returns>response</returns>
    [Pure]
    public static HttpResponseMessage AsResponse(this IActionResult actionResult) =>
        actionResult switch
        {
            ObjectResult { Value: not null } objectResult
                => ((HttpStatusCode)objectResult.StatusCode.GetValueOrDefault())
                    .Result()
                    .WithTextContent(
                        objectResult.Value.ToString(),
                        objectResult.ContentTypes.FirstOrDefault()
                    ),
            IStatusCodeActionResult statusCodeResult
                => ((HttpStatusCode)statusCodeResult.StatusCode.GetValueOrDefault()).Result(),
            _
                => HttpStatusCode
                    .InternalServerError
                    .Result()
                    .WithTextContent("Failed to process action result")
        };
}
