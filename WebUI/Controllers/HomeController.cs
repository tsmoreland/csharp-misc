//
// Copyright © 2020 Terry Moreland
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

//#define USING_SIGN_IN_MANAGER

using System;
using System.Diagnostics;
using System.Security.Claims;
using System.Threading.Tasks;
using IdentityDomain;
using Microsoft.AspNetCore.Authentication;
#if !USING_SIGN_IN_MANAGER
using System.Collections.Generic;
#endif
using Microsoft.AspNetCore.Authorization;
using Microsoft.AspNetCore.Identity;
using Microsoft.AspNetCore.Mvc;
using Microsoft.Extensions.Logging;
using WebUI.Models;

namespace WebUI.Controllers
{
    public class HomeController : Controller
    {
        private readonly UserManager<DemoUser> _userManager;
        private readonly IUserClaimsPrincipalFactory<DemoUser> _userClaimsPrincipalFactory;
#       if USING_SIGN_IN_MANAGER
        private readonly SignInManager<DemoUser> _signInManager;
#       endif
        private readonly ILogger<HomeController> _logger;

        public HomeController(
            UserManager<DemoUser> userManager,
            IUserClaimsPrincipalFactory<DemoUser> userClaimsPrincipalFactory,
#           if USING_SIGN_IN_MANAGER
            SignInManager<DemoUser> signInManager,
#           endif
            ILogger<HomeController> logger)
        {
            _userManager = userManager ?? throw new ArgumentNullException(nameof(userManager));
            _userClaimsPrincipalFactory = userClaimsPrincipalFactory ?? throw new ArgumentNullException(nameof(userClaimsPrincipalFactory));
#           if USING_SIGN_IN_MANAGER
            _signInManager = signInManager ?? throw new ArgumentNullException(nameof(signInManager));
#           endif
            _logger = logger ?? throw new ArgumentNullException(nameof(logger));
        }

        [HttpGet]
        public IActionResult Index()
        {
            return View();
        }

        [HttpGet]
        public IActionResult Register()
        {
            return View();
        }

        [HttpPost]
        [ValidateAntiForgeryToken]
        public async Task<IActionResult> Register(RegisterModel model)
        {
            if (!ModelState.IsValid)
                return View();

            var user = await _userManager.FindByNameAsync(model.UserName);
            if (user != null)
            {
                _logger.LogError($"{model.UserName} already exists, returning success to user without taking action");
                return Redirect(nameof(RegisterSuccess));
            }

            user = new DemoUser
            {
                Id =  Guid.NewGuid().ToString(),
                UserName =  model.UserName,
                Email = model.Email,
                Locale = "en-CA",
                CountryId = "CA", // Hack until we have a country manager to import this, or better yet let it be configured on the registration page - that or allow it to be optional
            };

            var result = await _userManager.CreateAsync(user, model.Password ?? string.Empty);
            if (!result.Succeeded)
            {
                foreach (var error in result.Errors)
                    ModelState.AddModelError(string.Empty, error.Description); // should use Code to look up correct localized string
                return View();
            }

            var token = await _userManager.GenerateEmailConfirmationTokenAsync(user);
            var confirmEmailUrl = Url.Action("ConfirmEmail", "Home", new {token, email = user.Email},
                Request.Scheme);
            _logger.LogDebug($"Todo: send {confirmEmailUrl} to {user.Email}");

            return Redirect(nameof(RegisterSuccess));
        }

        [HttpGet]
        public IActionResult RegisterSuccess()
        {
            return View();
        }

        [HttpGet]
        public async Task<IActionResult> ConfirmEmail(string token, string email)
        {
            var user = await _userManager.FindByEmailAsync(email);
            return (user != null && (await _userManager.ConfirmEmailAsync(user, token)).Succeeded)
                ? View("EmailConfirmed")
                : View("Error");
        }

        [HttpGet]
        public IActionResult Login()
        {
            return View();
        }

        [HttpPost]
        [ValidateAntiForgeryToken]
        public async Task<IActionResult> Login(LoginModel model)
        {
            if (!ModelState.IsValid)
                return View();


#           if !USING_SIGN_IN_MANAGER
            var user = await _userManager.FindByNameAsync(model.UserName);
            if (user == null)
                return FailWithMessage("Invalid username or password");

            if (await _userManager.IsLockedOutAsync(user))
                return FailWithMessage("Invalid username or password");

            if (!await _userManager.IsEmailConfirmedAsync(user))
                return FailWithMessage("Invalid username or password");

            if (!(await _userManager.IsEmailConfirmedAsync(user)) || !(await _userManager.CheckPasswordAsync(user, model.Password)))
            {
                await _userManager.AccessFailedAsync(user);
                if (await _userManager.IsLockedOutAsync(user))
                {
                    _logger.LogDebug("ToDo: send e-mail notifying the user that they are now locked out"); 
                }
                return FailWithMessage("Invalid username or password");
            }


            await _userManager.ResetAccessFailedCountAsync(user);

            if (await _userManager.GetTwoFactorEnabledAsync(user))
            {
                var providers = await _userManager.GetValidTwoFactorProvidersAsync(user);
                if (providers.Contains(_userManager.Options.Tokens.AuthenticatorTokenProvider))
                    return await RedirectToTwoFactor(_userManager.Options.Tokens.AuthenticatorTokenProvider);

                if (providers.Contains("Email"))
                {
                    var token = _userManager.GenerateTwoFactorTokenAsync(user, "Email");
                    _logger.LogDebug($"ToDo: send {token} to {user.Email}");
                    return await RedirectToTwoFactor("Email");
                }
            }
            var principal = await _userClaimsPrincipalFactory.CreateAsync(user);
            await HttpContext.SignInAsync(IdentityConstants.ApplicationScheme, principal);
            return RedirectToAction(nameof(Index));

            static ClaimsPrincipal Store2FactorAuth(string userId, string provider) =>
                new ClaimsPrincipal(new ClaimsIdentity(new List<Claim>
                {
                    new Claim("sub", userId),
                    new Claim("amr", provider)
                }, IdentityConstants.TwoFactorUserIdScheme));

            async Task<IActionResult> RedirectToTwoFactor(string tokenProvider)
            {
                await HttpContext.SignInAsync(IdentityConstants.TwoFactorUserIdScheme, Store2FactorAuth(user.Id, tokenProvider));
                return RedirectToAction("TwoFactor");
            }
#           else

            // if customization is required consider moving to userManager instead of signInManager
            // while it is good signInManager can obscure a lot of the details which may be needed for more complex
            // systems

            var user = await _userManager.FindByNameAsync(model.UserName);
            if (user == null)
                return FailWithMessage("Invalid username or password");

            // TODO: determine how to setup 2FA using signInManager

            var result = await _signInManager.PasswordSignInAsync(model.UserName, model.Password, false, false);
            if (result.Succeeded)
            {
                return RedirectToAction(nameof(Index));
            } 
            else if (result.RequiresTwoFactor)
            {
                // ...return as error, showing this way to highlight the options ...
            }
            else if (result.IsLockedOut)
            {
                // ...return as error, showing this way to highlight the options ...
            }
            else if (result.IsNotAllowed)
            {
                // ...return as error, showing this way to highlight the options ...
            }

            return View();
#           endif

        }

        [HttpGet]
        public IActionResult ForgotPassword() => View();

        [HttpPost]
        public async Task<IActionResult> ForgotPassword(ForgotPasswordModel model)
        {
            if (!ModelState.IsValid)
                return View();

            var user = await _userManager.FindByEmailAsync(model.Email);
            if (user != null)
            {
                // redirect to e-mail sent page
                var token = await _userManager.GeneratePasswordResetTokenAsync(user);
                var resetUrl = Url.Action("ResetPassword", "Home", new {token, email = user.Email}, Request.Scheme);
                _logger.LogDebug($"ToDo: send the '{resetUrl}' to {user.Email}");
            }
            else
            {
                _logger.LogDebug($"ToDo: send message to {model.Email} informing them that they do not have an account");
            }

            // not convinced of this, thinking this should redirect to success page
            return View("ResetPasswordRequestSuccess");
        }

        [HttpGet]
        public IActionResult ResetPassword(string token, string email) =>
            View(new ResetPasswordModel {Token = token, Email = email});

        [HttpPost]
        public async Task<IActionResult> ResetPassword(ResetPasswordModel model)
        {
            if (!ModelState.IsValid)
            {
                ModelState.AddModelError(string.Empty, "Invalid Request");
                return View(); // add extension method to sanitize output of ModelState errors
            }

            var user = await _userManager.FindByEmailAsync(model.Email);
            if (user == null)
            {
                return View();
            }

            var result = await _userManager.ResetPasswordAsync(user, model.Token, model.Password);
            if (!result.Succeeded)
            {
                foreach (var error in result.Errors)
                    ModelState.AddModelError(string.Empty, error.Description);
                return View();
            }

            if (await _userManager.IsLockedOutAsync(user))
                await _userManager.SetLockoutEndDateAsync(user, DateTimeOffset.UtcNow);

            return View("ResetPasswordSuccess");
        }

        [HttpGet]
        public IActionResult TwoFactor()
        {
            return View();
        }

        [HttpPost]
        public async Task<IActionResult> TwoFactor(TwoFactorModel model)
        {
            var result = await HttpContext.AuthenticateAsync(IdentityConstants.TwoFactorUserIdScheme);
            if (!result.Succeeded)
                return FailWithMessage("Invalid token");

            if (!ModelState.IsValid)
                return View();

            var user = await _userManager.FindByIdAsync(result.Principal.FindFirstValue("sub"));
            if (user == null)
                return FailWithMessage("Invalid request");

            var isValid = await _userManager
                .VerifyTwoFactorTokenAsync(user, result.Principal.FindFirstValue("amr"), model.Token);
            if (!isValid)
                return FailWithMessage("Invalid token");

            await CompleteSignInAsync();
            return RedirectToAction(nameof(Index));

            async Task CompleteSignInAsync()
            {
                await HttpContext.SignOutAsync(IdentityConstants.TwoFactorUserIdScheme);
                var  principal = await _userClaimsPrincipalFactory.CreateAsync(user);
                await HttpContext.SignInAsync(IdentityConstants.ApplicationScheme, principal);
            }
        }

        [HttpGet]
        [Authorize]
        public async Task<IActionResult> RegisterAuthenticator()
        {
            var user = await _userManager.GetUserAsync(User);
            if (user == null)
            {
                await HttpContext.SignOutAsync();
                return RedirectToAction(nameof(Login));
            }

            var authenticatorKey = await GetExistingOrNewAuthentictorKeyAsync();

            return View(new RegisterAuthenticatorModel {AuthenticatorKey = authenticatorKey});

            async Task<string> GetExistingOrNewAuthentictorKeyAsync()
            {
                var key = await _userManager.GetAuthenticatorKeyAsync(user);
                if (key != null)
                    return key;

                await _userManager.ResetAuthenticatorKeyAsync(user);
                return await _userManager.GetAuthenticatorKeyAsync(user);
            }
        }

        [HttpPost]
        [Authorize]
        public async Task<IActionResult> RegisterAuthenticator(RegisterAuthenticatorModel model)
        {
            var user = await _userManager.GetUserAsync(User);
            if (user == null)
            {
                await HttpContext.SignOutAsync();
                return RedirectToAction(nameof(Login));
            }

            var isValid = await _userManager.VerifyTwoFactorTokenAsync(user,
                _userManager.Options.Tokens.AuthenticatorTokenProvider, model.Code);

            if (!isValid)
            {
                ModelState.AddModelError(string.Empty, "Invalid Code");
                return View(model);
            }

            await _userManager.SetTwoFactorEnabledAsync(user, true);
            return View("RegisterAuthenticatorSuccess");

        }

        [HttpGet]
        public IActionResult ExternalLogin(string provider)
        {
            var properties = new AuthenticationProperties
            {
                RedirectUri = Url.Action(nameof(ExternalLoginCallback)),
                Items = {{"scheme", provider}}
            };
            return Challenge(properties, provider);
        }

        [HttpGet]
        public async Task<IActionResult> ExternalLoginCallback()
        {
            var result = await HttpContext.AuthenticateAsync(IdentityConstants.ExternalScheme);
            var externalUserId = result.Principal.FindFirstValue("sub")
                                 ?? result.Principal.FindFirstValue(ClaimTypes.NameIdentifier)
                                 ?? string.Empty; // alternately throw exception because it's invalid
            var provider = result.Properties.Items["scheme"] ?? string.Empty;

            if (string.IsNullOrEmpty(externalUserId))
                return View("Error");

            var user = await _userManager.FindByLoginAsync(provider, externalUserId);
            if (user != null)
                return await CompleteSignin();

            var email = result.Principal.FindFirstValue("email") ??
                        result.Principal.FindFirstValue(ClaimTypes.Email);
            if (email == null)
                return View(nameof(Error));

            user = await _userManager.FindByEmailAsync(email);
            if (user == null)
            {
                user = new DemoUser {UserName = email, Email = email};
                await _userManager.CreateAsync(user);
            }
            await _userManager.AddLoginAsync(user,
                new UserLoginInfo(provider, externalUserId, provider));


            return await CompleteSignin();

            async Task<IActionResult> CompleteSignin()
            {
                await HttpContext.SignOutAsync(IdentityConstants.ExternalScheme);
                var principal = await _userClaimsPrincipalFactory.CreateAsync(user);
                await HttpContext.SignInAsync(IdentityConstants.ApplicationScheme, principal);
                return RedirectToAction(nameof(Index));
            }
        }


        [HttpGet]
        [Authorize]
        public IActionResult Profile() 
        {
            // not implemented yet
            return View();
        }

        [HttpGet]
        public IActionResult Privacy() => View();

        [HttpGet]
        [ResponseCache(Duration = 0, Location = ResponseCacheLocation.None, NoStore = true)]
        public IActionResult Error() =>
            View(new ErrorViewModel { RequestId = Activity.Current?.Id ?? HttpContext.TraceIdentifier });

        private ViewResult FailWithMessage(string message)
        {
            ModelState.AddModelError(string.Empty, message);
            return View();
        }
    }
}
