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

using Microsoft.AspNetCore.Http;
using Microsoft.Extensions.Logging;
using Microsoft.Extensions.Options;
using Microsoft.Extensions.Primitives;

namespace TSMoreland.Authorization.Demo.Middleware;

public sealed class SecurityHeadersMiddleware
{
    private readonly RequestDelegate _next;
    private readonly IOptions<SecurityHeadersOptions> _options;
    private readonly ILogger<SecurityHeadersMiddleware> _logger;

    public SecurityHeadersMiddleware(RequestDelegate next, IOptions<SecurityHeadersOptions> options, ILoggerFactory loggerFactory)
    {
        ArgumentNullException.ThrowIfNull(loggerFactory, nameof(loggerFactory));

        _next = next ?? throw new ArgumentNullException(nameof(next));
        _options = options ?? throw new ArgumentNullException(nameof(options));

        _logger = loggerFactory.CreateLogger<SecurityHeadersMiddleware>();
    }

    public async Task InvokeAsync(HttpContext context)
    {
        AddHeadersIfNotPresent(context.Response);
        
        await _next(context);
    }

    private static void AddHeadersIfNotPresent(HttpResponse response)
    {
        ArgumentNullException.ThrowIfNull(response, nameof(response));

        AddHeaderIfNotPresent(response, "Cache-Control", "no-store");
        AddHeaderIfNotPresent(response, "X-Content-Type-Options", "nosniff");
        AddHeaderIfNotPresent(response, "X-Frame-Options", "DENY");

        // may want to consider something in security options for these as they may be overkill
        AddHeaderIfNotPresent(response, "Content-Security-Policy",
            "default-src: 'none'; FeaturePolicy: 'none'; Referrer-Policy: no-referrer");
    }

    private static void AddHeaderIfNotPresent(HttpResponse response, string header, params string[] values)
    {
        switch (values.Length)
        {
            case > 1:
                response.Headers.Add(header, new StringValues(values));
                break;
            case > 0:
                response.Headers.Add(header, new StringValues(values.First()));
                break;
        }
    }

}
