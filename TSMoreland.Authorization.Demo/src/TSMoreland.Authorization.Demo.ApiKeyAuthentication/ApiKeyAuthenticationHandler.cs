//
// Copyright (c) 2022 Terry Moreland
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

using System.Security.Claims;
using System.Text.Encodings.Web;
using Microsoft.AspNetCore.Authentication;
using Microsoft.AspNetCore.Identity;
using Microsoft.Extensions.DependencyInjection;
using Microsoft.Extensions.Logging;
using Microsoft.Extensions.Options;
using Microsoft.Net.Http.Headers;
using TSMoreland.Authorization.Demo.Authentication.Abstractions;
using TSMoreland.Authorization.Demo.LocalUsers.Abstractions;
using TSMoreland.Authorization.Demo.LocalUsers.Abstractions.Entities;
using TSMoreland.Authorization.Demo.LocalUsers.Abstractions.Repositories;

namespace TSMoreland.Authorization.Demo.ApiKeyAuthentication;

public sealed class ApiKeyAuthenticationHandler : AuthenticationHandler<AuthenticationSchemeOptions>
{
    private readonly IApiKeyRepository _repository;
    private readonly IUserClaimsPrincipalFactory<DemoUser> _userClaimsPrincipalFactory;

    /// <inheritdoc />
    public ApiKeyAuthenticationHandler(
        IApiKeyRepository repository,
        IUserClaimsPrincipalFactory<DemoUser> userClaimsPrincipalFactory,
        IOptionsMonitor<AuthenticationSchemeOptions> options,
        ILoggerFactory logger,
        UrlEncoder encoder,
        ISystemClock clock)
        : base(options, logger, encoder, clock)
    {
        _repository = repository ?? throw new ArgumentNullException(nameof(repository));
        _userClaimsPrincipalFactory = userClaimsPrincipalFactory ?? throw new ArgumentNullException(nameof(userClaimsPrincipalFactory));
    }

    /// <inheritdoc />
    protected override async Task<AuthenticateResult> HandleAuthenticateAsync()
    {
        try
        {
            await ValueTask.CompletedTask;
            string apiKey = Response.Headers[HeaderNames.Authorization].GetApiKeyOrThrow();
            await ValidateApiKeyOrThrow(apiKey);

            DemoUser user = await _repository.GetUserFromApiKeyAsync(apiKey, CancellationToken.None);

            ClaimsPrincipal claimsPrincipal = await CreateClaimsPrincipalFromUserOrThrow(user);            
            Context.User = claimsPrincipal;

            Logger.LogInformation("User {UserId} successfully logged in using Api-Key", user.Id);
            return AuthenticateResult.Success(new AuthenticationTicket(claimsPrincipal, ApiKeyAuthenticationDefaults.SchemeName));            

        }
        catch (AuthenticationFailedException ex)
        {
            Logger.LogError(ex, "Authentication was not valid for basic authentication or credentials were invalid.");
            return ex.Result;
        }
        catch (UserNotFoundException ex)
        {
            Logger.LogError(ex, "user for provided api key not found");
            return AuthenticateResult.NoResult();
        }
        catch (ApiKeyNotFoundException ex) 
        {
            Logger.LogError(ex, "api key not found");
            return AuthenticateResult.NoResult();
        }
        catch (Exception ex)
        {
            Logger.LogError(ex,
                "Unexpected error occurred, returning no result to allow other handlers to attempt authorization");
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

    private static ValueTask ValidateApiKeyOrThrow(string apiKey)
    {
        return apiKey switch
        {
            {Length: 0} => ValueTask.FromException(new AuthenticationFailedException(AuthenticateResult.NoResult(), "api key cannot be empty")),
            _ => ValueTask.CompletedTask,
        };
    }
    private Task<ClaimsPrincipal> CreateClaimsPrincipalFromUserOrThrow(DemoUser user)
    {
        return _userClaimsPrincipalFactory.CreateAsync(user)
            .ContinueWith(createTask =>
            {
                ClaimsPrincipal principal = createTask.Result;
                createTask.Dispose();
                ClaimsIdentity? identity = principal.Identities.FirstOrDefault();
                if (identity is null)
                {
                    return principal;
                }

                identity.AddClaim(new Claim(ClaimDefinitions.AuthenticationMethod, ClaimDefinitions.PersonalIdentificationNumber)); // TODO: see if there's something better than pin
                identity.AddClaim(new Claim(ClaimTypes.AuthenticationMethod, ApiKeyAuthenticationDefaults.SchemeName));

                return principal;

            }, TaskContinuationOptions.OnlyOnRanToCompletion);
    }
}
