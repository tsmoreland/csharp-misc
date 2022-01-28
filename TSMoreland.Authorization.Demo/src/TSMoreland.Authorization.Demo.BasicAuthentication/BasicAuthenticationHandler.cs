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

using System.Security.Claims;
using System.Text.Encodings.Web;
using Microsoft.AspNetCore.Authentication;
using Microsoft.AspNetCore.Identity;
using Microsoft.Extensions.Logging;
using Microsoft.Extensions.Options;
using Microsoft.Net.Http.Headers;
using TSMoreland.Authorization.Demo.LocalUsers.Abstractions.Entities;

namespace TSMoreland.Authorization.Demo.BasicAuthentication;

public sealed class BasicAuthenticationHandler : AuthenticationHandler<AuthenticationSchemeOptions>
{
    private readonly SignInManager<DemoUser> _signInManager;
    private readonly IUserClaimsPrincipalFactory<DemoUser> _userClaimsPrincipalFactory;
    
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
        Response.Headers[HeaderNames.WWWAuthenticate] = BasicAuthenticationDefaults.WwwAuthenticateHeader;
        return base.HandleChallengeAsync(properties);
    }
    
    private ValueTask<DemoUser> GetUserFromUsernameOrThrow(string username)
    {
        throw new NotImplementedException();
    }
    private Task ValidateUserCredentialsOrThrow(DemoUser user, string password)
    {
        return _signInManager.CheckPasswordSignInAsync(user, password, true)
            .ContinueWith(task => 
            {
                // 1. if task cancelled return or throw OperationCanceledException
                // 2. if task faulted, wrap in a failure
                // 3. otherwise check task.Result.Succeeded and throw an exception if it's false
            });
    }
    private ValueTask<ClaimsPrincipal> CreateClaimsPrincipalFromUserOrThrow(DemoUser user)
    {
        throw new NotImplementedException();
    }
}
