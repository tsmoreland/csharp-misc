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
using TSMoreland.Authorization.Demo.Authentication.Abstractions.Extensions;
using TSMoreland.Authorization.Demo.LocalUsers.Abstractions.Entities;

namespace TSMoreland.Authorization.Demo.BasicAuthentication;

public sealed class BasicAuthenticationHandler : AuthenticationHandler<AuthenticationSchemeOptions>
{
    private readonly SignInManager<DemoUser> _signInManager;
    private readonly IUserClaimsPrincipalFactory<DemoUser> _userClaimsPrincipalFactory;
    private const string AuthenticationFailedMessage = "Not Authorized.";
    
    /// <inheritdoc />
    public BasicAuthenticationHandler(
        SignInManager<DemoUser> signInManager,
        IUserClaimsPrincipalFactory<DemoUser> userClaimsPrincipalFactory,
        IOptionsMonitor<AuthenticationSchemeOptions> options,
        ILoggerFactory logger,
        UrlEncoder encoder,
        ISystemClock clock)
        : base(options, logger, encoder, clock)
    {
        _signInManager = signInManager ?? throw new ArgumentNullException(nameof(signInManager));
        _userClaimsPrincipalFactory = userClaimsPrincipalFactory ?? throw new ArgumentNullException(nameof(userClaimsPrincipalFactory));
    }

    /// <inheritdoc />
    protected override async Task<AuthenticateResult> HandleAuthenticateAsync()
    {
        try
        {
            (string username, string password) = Response.Headers[HeaderNames.Authorization].GetBasicUsernameAndPasswordOrThrow();
            DemoUser user = await GetUserFromUsernameOrThrow(username);
            
            await ValidateUserCredentialsOrThrow(user, password);
            
            ClaimsPrincipal claimsPrincipal = await CreateClaimsPrincipalFromUserOrThrow(user);            
            
            Context.User = claimsPrincipal;
            await _signInManager.UserManager.ResetAccessFailedCountAsync(user);
            
            Logger.LogInformation("User {UserId} successfully logged in", user.Id);
            return AuthenticateResult.Success(new AuthenticationTicket(claimsPrincipal, BasicAuthenticationDefaults.SchemeName));            
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

    private Task<DemoUser> GetUserFromUsernameOrThrow(string username)
    {
        return _signInManager.UserManager.FindByNameAsync(username)
            .ContinueWith(findTask =>
            {
                DemoUser? user = findTask.Result;
                findTask.Dispose();

                return user ??
                       throw new AuthenticationFailedException(AuthenticateResult.Fail(AuthenticationFailedMessage));

            }, TaskContinuationOptions.OnlyOnRanToCompletion);
    }
    private Task ValidateUserCredentialsOrThrow(DemoUser user, string password)
    {
        Task task = _signInManager.CheckPasswordSignInAsync(user, password, true)
            .ContinueWith(signInTask => 
            {
                SignInResult result = signInTask.Result;
                signInTask.Dispose();
                Logger.LogFailureReason(result);
                if (result.Succeeded)
                {
                    return;
                }

                Logger.LogInformation("Unable to sign in {UserId}, invalid crednetials.", user.Id);
                throw new AuthenticationFailedException(AuthenticateResult.Fail(AuthenticationFailedMessage));

            }, TaskContinuationOptions.OnlyOnRanToCompletion);
        return task;
    }
    private Task<ClaimsPrincipal> CreateClaimsPrincipalFromUserOrThrow(DemoUser user)
    {
        return _userClaimsPrincipalFactory.CreateAsync(user)
            .ContinueWith(createTask =>
            {
                ClaimsPrincipal principal = createTask.Result;
                createTask.Dispose();
                ClaimsIdentity? identity = principal.Identities.FirstOrDefault();
                if (identity is not null)
                {
                    identity.AddClaim(new Claim(ClaimDefinitions.AuthenticationMethod, ClaimDefinitions.PasswordBased)); 
                    identity.AddClaim(new Claim(ClaimTypes.AuthenticationMethod, BasicAuthenticationDefaults.SchemeName));
                }

                return principal;

            }, TaskContinuationOptions.OnlyOnRanToCompletion);
    }
}
