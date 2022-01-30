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
using Microsoft.AspNetCore.Builder;
using Microsoft.Extensions.DependencyInjection;
using Microsoft.Extensions.Hosting;
using TSMoreland.Authorization.Demo.Middleware.Abstractions;

namespace TSMoreland.Authorization.Demo.Middleware;

public static class ApplicationBuilderExtensions
{
    public static IApplicationBuilder UseSecurityHeaders(this IApplicationBuilder app)
    {
        ArgumentNullException.ThrowIfNull(app, nameof(app));

        app.UseMiddleware<SecurityHeadersMiddleware>();

        return app;
    }

    public static void UseErrorReponseProvider(this IApplicationBuilder app)
    {
        ArgumentNullException.ThrowIfNull(app, nameof(app));
        IHostEnvironment environment = app.ApplicationServices.GetRequiredService<IHostEnvironment>();
        UseErrorReponseProvider(app, environment);
    }

    public static void UseErrorReponseProvider(this IApplicationBuilder app, IHostEnvironment environment)
    {
        ArgumentNullException.ThrowIfNull(app, nameof(app));
        IErrorResponseProvider provider = app.ApplicationServices.GetRequiredService<IErrorResponseProvider>();

        if (environment.IsDevelopment())
        {
            app.Use((context, next) => provider.WriteDebugErrorResponse(context, next));
        }
        else
        {
            app.Use((context, next) => provider.WriteErrorResponse(context, next));
        }
    }

    public static IApplicationBuilder UseStatusCodeErrorToException(this IApplicationBuilder app)
    {
        return app.UseMiddleware<StatusCodeErrorMiddleware>();
    }

    public static IApplicationBuilder UseErrorRepsonseProvidedExceptionHandler(this IApplicationBuilder app)
    {
        ArgumentNullException.ThrowIfNull(app, nameof(app));

        app.UseExceptionHandler(appBuilder => appBuilder.UseErrorReponseProvider());
        app.UseStatusCodeErrorToException();
        return app;
    }
}
