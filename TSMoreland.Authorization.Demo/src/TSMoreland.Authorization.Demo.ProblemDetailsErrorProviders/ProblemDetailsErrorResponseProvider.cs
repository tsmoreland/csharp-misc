//
// Copyright © 2022 Terry Moreland
// Permission is hereby granted, free of charge, to any person obtaining a copy of this software and associated documentation files (the "Software"), 
// to deal in the Software without restriction, including without limitation the rights to use, copy, modify, merge, publish, distribute, sublicense, 
// and/or sell copies of the Software, and to permit persons to whom the Software is furnished to do so, subject to the following conditions:
//
// The above copyright notice and this permission notice shall be included in all copies or substantial portions of the Software.
//
// THE SOFTWARE IS PROVIDED "AS IS", WITHOUT WARRANTY OF ANY KIND, EXPRESS OR IMPLIED, INCLUDING BUT NOT LIMITED TO THE WARRANTIES OF MERCHANTABILITY, 
// FITNESS FOR A PARTICULAR PURPOSE AND NONINFRINGEMENT. IN NO EVENT SHALL THE AUTHORS OR COPYRIGHT HOLDERS BE LIABLE FOR ANY CLAIM, DAMAGES OR OTHER LIABILITY, 
// WHETHER IN AN ACTION OF CONTRACT, TORT OR OTHERWISE, ARISING FROM, OUT OF OR IN CONNECTION WITH THE SOFTWARE OR THE USE OR OTHER DEALINGS IN THE SOFTWARE.
//

using System.Diagnostics;
using System.Text.Json;
using Microsoft.AspNetCore.Diagnostics;
using Microsoft.AspNetCore.Http;
using Microsoft.AspNetCore.Mvc;
using Microsoft.AspNetCore.Mvc.Infrastructure;
using TSMoreland.Authorization.Demo.Middleware.Abstractions;

using HttpStatusCode = System.Net.HttpStatusCode;

namespace TSMoreland.Authorization.Demo.ProblemDetailsErrorProviders;

public sealed class ProblemDetailsErrorResponseProvider : IErrorResponseProvider
{

    private readonly ProblemDetailsFactory _problemDetailsFactory;
    private const string ProblemJsonType = "application/problem+json; charset=UTF-8";

    public ProblemDetailsErrorResponseProvider(ProblemDetailsFactory problemDetailsFactory)
    {
        _problemDetailsFactory = problemDetailsFactory ?? throw new ArgumentNullException(nameof(problemDetailsFactory));
    }

    /// <inheritdoc />
    public Task WriteDebugErrorResponse(HttpContext httpContext, Func<Task> next) =>
        WriteErrorResponse(httpContext, next, true);

    /// <inheritdoc/>
    public Task WriteErrorResponse(HttpContext httpContext, Func<Task> next) =>
        WriteErrorResponse(httpContext, next, false);

    public Task WriteErrorResponse(HttpContext httpContext, Func<Task> next, bool includeDebug)
    {
        IExceptionHandlerFeature? exceptionDetails = httpContext.Features.Get<IExceptionHandlerFeature>();
        Exception? ex = exceptionDetails?.Error;
        if (ex is null)
        {
            return next();
        }

        string? instance = httpContext.ToRequestUriOrNull()?.ToString();
        httpContext.Response.ContentType = ProblemJsonType;

        switch (ex)
        {
            case InvalidModelStateException invalidModelStateException:
                {
                    string detail = includeDebug
                        ? "One or more fields has invalid values; " + ex.ToString().ReplaceLineEndings("%0A")
                        : "One or more fields has invalid values";
                    ValidationProblemDetails problem = _problemDetailsFactory
                        .CreateValidationProblemDetails(
                            httpContext,
                            invalidModelStateException.ModelState,
                            StatusCodes.Status422UnprocessableEntity,
                            "Invalid Entity",
                            detail: detail,
                            instance:  instance);
                    problem.Extensions["traceId"] = Activity.Current?.Id ?? httpContext.TraceIdentifier;
                    return JsonSerializer.SerializeAsync(httpContext.Response.Body, problem);
                }
            case HttpRequestException requestException:
                {
                    (int statusCode, ProblemDetails problem) = BuildProblemDetailsFrom(instance, httpContext,
                        requestException.StatusCode ?? HttpStatusCode.InternalServerError);
                    httpContext.Response.StatusCode = statusCode;
                    return JsonSerializer.SerializeAsync(httpContext.Response.Body, problem);
                }
            default:
                {
                    string detail = includeDebug
                        ? ex.ToString().ReplaceLineEndings("%0A")
                        : "unknown error occurred";

                    ProblemDetails problem = _problemDetailsFactory
                        .CreateProblemDetails(
                            httpContext,
                            title: "Internal Server Error",
                            detail: detail,
                            instance: instance);
                    problem.Extensions["traceId"] = Activity.Current?.Id ?? httpContext.TraceIdentifier;
                    return JsonSerializer.SerializeAsync(httpContext.Response.Body, problem);
                }
        }
    }

    private (int StatusCode, ProblemDetails Problem) BuildProblemDetailsFrom(string? instance, HttpContext httpContext, HttpStatusCode statusCode)
    {
        ProblemDetails problem = _problemDetailsFactory
            .CreateProblemDetails(
                httpContext,
                statusCode: (int) statusCode,
                instance: instance);

        return ((int)statusCode, problem);
    }
}
