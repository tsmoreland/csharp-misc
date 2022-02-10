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

using System.Text.Encodings.Web;
using Microsoft.AspNetCore.Authentication;
using Microsoft.Extensions.DependencyInjection;
using Microsoft.Extensions.Logging;
using Microsoft.Extensions.Options;
using Microsoft.Net.Http.Headers;
using TSMoreland.Authorization.Demo.Authentication.Abstractions;

namespace TSMoreland.Authorization.Demo.ApiKeyAuthentication;

public sealed class ApiKeyAuthenticationHandler : AuthenticationHandler<AuthenticationSchemeOptions>
{
    /// <inheritdoc />
    public ApiKeyAuthenticationHandler(
        IOptionsMonitor<AuthenticationSchemeOptions> options,
        ILoggerFactory logger,
        UrlEncoder encoder,
        ISystemClock clock)
        : base(options, logger, encoder, clock)
    {
    }

    /// <inheritdoc />
    protected override async Task<AuthenticateResult> HandleAuthenticateAsync()
    {
        try
        {

            await ValueTask.CompletedTask;

            string apiKey = Response.Headers[HeaderNames.Authorization].GetApiKeyOrThrow();
            await ValidateApiKeyOrThrow(apiKey);


            throw new NotImplementedException();
        }
        catch (AuthenticationFailedException ex)
        {
            Logger.LogError(ex, "Authentication was not valid for basic authentication or credentials were invalid.");
            return ex.Result;
        }
        catch (Exception ex)
        {
            Logger.LogError(ex, "Unexpected error occurred, returning no result to allow other handlers to attempt authorization");
            return AuthenticateResult.NoResult();
        }
    }

    /// <inheritdoc />
    protected override Task HandleChallengeAsync(AuthenticationProperties properties)
    {
        string headerValue = Context.RequestServices
            .GetServices<IChallengeSchemeProvider>()
            .Select(p => p.Challenge)
            .Aggregate((a, b) => string.Join(',', a, b));

        Response.Headers[HeaderNames.WWWAuthenticate] = headerValue;
        return base.HandleChallengeAsync(properties);
    }

    private ValueTask ValidateApiKeyOrThrow(string apiKey)
    {
        return ValueTask.FromException(new NotImplementedException());
    }
}
