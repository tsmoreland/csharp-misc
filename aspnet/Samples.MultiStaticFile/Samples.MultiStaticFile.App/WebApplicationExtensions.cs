//
// Copyright © 2023 Terry Moreland
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

using Microsoft.AspNetCore.StaticFiles;
using Microsoft.Extensions.FileProviders;
using Microsoft.Identity.Web.Resource;

namespace Samples.MultiStaticFile.App;

public static class WebApplicationExtensions
{
    public static WebApplication ConfigurePipeline(this WebApplication app)
    {

        // Configure the HTTP request pipeline.
        if (app.Environment.IsDevelopment())
        {
            app.UseSwagger();
            app.UseSwaggerUI();
        }

        app.UseHttpsRedirection();

        app.UseAuthentication();
        app.UseAuthorization();

        PhysicalFileProvider fileProvider = new(Path.Combine(app.Environment.ContentRootPath, "Content"));
        IContentTypeProvider contentTypeProvider = new CustomContentTypeProvider();

        static void OnPrepareResponse(StaticFileResponseContext responseContext)
        {
            // return error here if file type not supported
        }

        StaticFileOptions options = new()
        {
            FileProvider = fileProvider,
            OnPrepareResponse = OnPrepareResponse,
            ContentTypeProvider = contentTypeProvider,
        };

        app
            .UseStaticFiles(new StaticFileOptions()
            {
                FileProvider = fileProvider,
                OnPrepareResponse = OnPrepareResponse,
                ContentTypeProvider = contentTypeProvider,
                RequestPath = "/alpha",
            })
            .UseStaticFiles(new StaticFileOptions
            {
                FileProvider = fileProvider,
                OnPrepareResponse = OnPrepareResponse,
                ContentTypeProvider = contentTypeProvider,
                RequestPath = "/beta",
            });



        string scopeRequiredByApi = app.Configuration["AzureAd:Scopes"] ?? "";
        string[] summaries = new[]
        {
            "Freezing", "Bracing", "Chilly", "Cool", "Mild", "Warm", "Balmy", "Hot", "Sweltering", "Scorching"
        };

        app
            .MapGet("/weatherforecast", (HttpContext httpContext) =>
            {
                httpContext.VerifyUserHasAnyAcceptedScope(scopeRequiredByApi);

                WeatherForecast[] forecast = Enumerable.Range(1, 5).Select(index =>
                    new WeatherForecast
                    (
                        DateOnly.FromDateTime(DateTime.Now.AddDays(index)),
                        Random.Shared.Next(-20, 55),
                        summaries[Random.Shared.Next(summaries.Length)]
                    ))
                    .ToArray();
                return forecast;
            })
            .WithName("GetWeatherForecast")
            .WithOpenApi()
            .RequireAuthorization();

        return app;
    }
}
