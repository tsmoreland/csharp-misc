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
using Microsoft.Extensions.DependencyInjection;
using Microsoft.Extensions.Hosting;
using Microsoft.Extensions.Logging;
using Microsoft.Extensions.Options;
using TSMoreland.Authorization.Demo.Middleware.Abstractions;

using HttpStatusCode = System.Net.HttpStatusCode;

namespace TSMoreland.Authorization.Demo.ProblemDetailsErrorProviders;

public sealed class ProblemDetailsErrorResponseProvider : IErrorResponseProvider
{

    private readonly ProblemDetailsFactory _problemDetailsFactory;
    private readonly ApiBehaviorOptions _options;
    private readonly ILogger<ProblemDetailsErrorResponseProvider> _logger;
    private const string ProblemJsonType = "application/problem+json; charset=UTF-8";
    private sealed record class PartialProblemDetails(int StatusCode, string Title, string Description);

    public ProblemDetailsErrorResponseProvider(
        ProblemDetailsFactory problemDetailsFactory,
        IOptions<ApiBehaviorOptions> options,
        ILoggerFactory loggerFactory)
    {
        ArgumentNullException.ThrowIfNull(loggerFactory, nameof(loggerFactory));
        _problemDetailsFactory = problemDetailsFactory ?? throw new ArgumentNullException(nameof(problemDetailsFactory));
        _options = options?.Value ?? throw new ArgumentNullException(nameof(options));
        _logger = loggerFactory.CreateLogger<ProblemDetailsErrorResponseProvider>();
    }

    /// <inheritdoc />
    public Task WriteDebugErrorResponse(HttpContext httpContext, Func<Task> next) =>
        WriteErrorResponse(httpContext, next, true);

    /// <inheritdoc />
    public ValueTask<IActionResult> BuildResponse(ControllerBase controller)
    {
        if (controller == null!)
        {
            return ValueTask.FromException<IActionResult>(new ArgumentNullException(nameof(controller)));
        }
        HttpContext context = controller.HttpContext;
        IHostEnvironment? env = context.RequestServices.GetService<IHostEnvironment>();
        bool isDevelopment = env?.IsDevelopment() == true;

        IExceptionHandlerFeature? exceptionDetails = controller.HttpContext.Features.Get<IExceptionHandlerFeature>();
        Exception? ex = exceptionDetails?.Error;
        ProblemDetails problem = ex is not null &&
                  TryGetExceptionToProblemDetailsConverter(context, ex) is { } converter
            ? converter.Convert(context, ex)
            : new ProblemDetails();

        ApplyProblemDetailsDefaults(
            context,
            problem,
            problem is ValidationProblemDetails
                ? StatusCodes.Status400BadRequest
                : StatusCodes.Status500InternalServerError,
            ex,
            isDevelopment);

        return ValueTask.FromResult<IActionResult>(new ObjectResult(problem));
    }

    private IExceptionToProblemDetailsConverter? TryGetExceptionToProblemDetailsConverter(
        HttpContext context,
        Exception exception)
    {
        Type type = typeof(IExceptionToProblemDetailsConverter<>);
        Type[] genericType = new[] {exception.GetType()};
        Type converterType = type.MakeGenericType(genericType);
        return context.RequestServices.GetService(converterType) as IExceptionToProblemDetailsConverter;
    }

    private void ApplyProblemDetailsDefaults(
        HttpContext httpContext,
        ProblemDetails problemDetails,
        int statusCode,
        Exception? ex,
        bool isDevelopment)
    {
        problemDetails.Status ??= statusCode;

        if (_options.ClientErrorMapping.TryGetValue(statusCode, out ClientErrorData? clientErrorData))
        {
            problemDetails.Title ??= clientErrorData.Title;
            problemDetails.Type ??= clientErrorData.Link;
        }

        string? traceId = Activity.Current?.Id ?? httpContext?.TraceIdentifier;
        if (traceId != null)
        {
            problemDetails.Extensions["traceId"] = traceId;
        }

        if (!isDevelopment || ex is null)
        {
            return;
        }

        problemDetails.Extensions["exceptionMessage"] = ex.Message;
        problemDetails.Extensions["exceptionTrace"] = ex.StackTrace;
    }

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
