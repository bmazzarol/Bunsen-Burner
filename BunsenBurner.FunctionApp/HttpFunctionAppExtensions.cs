using System.Net;
using System.Net.Mime;
using BunsenBurner.FunctionApp.Models;
using BunsenBurner.Http;
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
    public static HttpRequest AsHttpRequest(this Request request) => new DummyHttpRequest(request);

    /// <summary>
    /// Convert object result to a response
    /// </summary>
    /// <param name="actionResult">result</param>
    /// <returns>response</returns>
    [Pure]
    public static Response AsResponse(this IActionResult actionResult) =>
        actionResult switch
        {
            ObjectResult { Value: not null } objectResult
                => Response.New(
                    (HttpStatusCode)objectResult.StatusCode.GetValueOrDefault(),
                    objectResult.Value.ToString(),
                    objectResult.ContentTypes.FirstOrDefault()
                ),
            IStatusCodeActionResult statusCodeResult
                => Response.New(
                    (HttpStatusCode)statusCodeResult.StatusCode.GetValueOrDefault(),
                    string.Empty,
                    string.Empty
                ),
            _
                => Response.New(
                    HttpStatusCode.InternalServerError,
                    "Failed to process action result",
                    MediaTypeNames.Text.Plain
                )
        };
}
