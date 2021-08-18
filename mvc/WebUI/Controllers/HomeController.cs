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

using System;
using System.Diagnostics;
using System.Security.Claims;
using System.Threading;
using System.Threading.Tasks;
using IdentityDomain;
using IdentityDomain.Infrastructure;
using Microsoft.AspNetCore.Authentication;
using System.Collections.Generic;
using System.Linq;
using System.Text.Encodings.Web;
using Microsoft.AspNetCore.Authorization;
using Microsoft.AspNetCore.Identity;
using Microsoft.AspNetCore.Mvc;
using Microsoft.Extensions.Logging;
using WebUI.Models;
using static WebUI.Helpers.LogSanitizer;

namespace WebUI.Controllers
{
    public class HomeController : Controller
    {
        private readonly UserManager<DemoUser> _userManager;
        private readonly IUserClaimsPrincipalFactory<DemoUser> _userClaimsPrincipalFactory;
        private readonly SignInManager<DemoUser> _signInManager;
        private readonly IDemoRepository _repository;
        private readonly UrlEncoder _urlEncoder;
        private readonly ILogger<HomeController> _logger;

        public HomeController(
            UserManager<DemoUser> userManager,
            IUserClaimsPrincipalFactory<DemoUser> userClaimsPrincipalFactory,
            SignInManager<DemoUser> signInManager,
            IDemoRepository repository,
            UrlEncoder urlEncoder,
            ILogger<HomeController> logger)
        {
            _userManager = userManager ?? throw new ArgumentNullException(nameof(userManager));
            _userClaimsPrincipalFactory = userClaimsPrincipalFactory ?? throw new ArgumentNullException(nameof(userClaimsPrincipalFactory));
            _signInManager = signInManager ?? throw new ArgumentNullException(nameof(signInManager));
            _repository = repository ?? throw new ArgumentNullException(nameof(repository));
            _urlEncoder = urlEncoder;
            _logger = logger ?? throw new ArgumentNullException(nameof(logger));
        }

        [HttpGet]
        public IActionResult Index()
        {
            return View();
        }

        [HttpGet]
        [AllowAnonymous]
        public IActionResult Register()
        {
            return View();
        }

        [HttpPost]
        [ValidateAntiForgeryToken]
        [AllowAnonymous]
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
                Country = await _repository.FindByIdAsync("CAN", CancellationToken.None) ?? Country.None,
            };
            user.CountryId = user.Country.Id;

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
            _logger.LogInformation("Todo: send {confirmEmailUrl} to {userEmail}", Sanitize(confirmEmailUrl), Sanitize(user.Email));
            TempData["ConfirmEmailUrl"] = confirmEmailUrl;

            return Redirect(nameof(RegisterSuccess));
        }

        [HttpGet]
        [AllowAnonymous]
        public IActionResult RegisterSuccess()
        {
            return View();
        }

        [HttpGet]
        [AllowAnonymous]
        public async Task<IActionResult> ConfirmEmail(string token, string email)
        {
            var user = await _userManager.FindByEmailAsync(email);
            return (user != null && (await _userManager.ConfirmEmailAsync(user, token)).Succeeded)
                ? View("EmailConfirmed")
                : View("Error", new ErrorViewModel {RequestId = Activity.Current?.Id ?? "Unknown"});
        }

        [HttpGet]
        [AllowAnonymous]
        public IActionResult Login([FromQuery(Name="ReturnUrl")] string returnUrl = "")
        {
            ViewBag.ReturnUrl = returnUrl;
            if (HttpContext.User.Identity?.IsAuthenticated != true)
                return View();

            return Url.IsLocalUrl(ViewBag.ReturnUrl)
                ? Redirect(ViewBag.ReturnUrl)
                : RedirectToAction(nameof(Index), "Home");
        }

        [HttpPost]
        [ValidateAntiForgeryToken]
        [AllowAnonymous]
        public async Task<IActionResult> Login(LoginModel model, [FromQuery(Name = "ReturnUrl")] string returnUrl = "")
        {
            if (!ModelState.IsValid)
                return View();

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
                    _logger.LogInformation("ToDo: send e-mail notifying the user that they are now locked out"); 
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
                    _logger.LogInformation("ToDo: send {token} to {userEmail}", token, user.Email);
                    return await RedirectToTwoFactor("Email");
                }
            }
            var principal = await _userClaimsPrincipalFactory.CreateAsync(user);
            await HttpContext.SignInAsync(IdentityConstants.ApplicationScheme, principal);

            return !string.IsNullOrEmpty(returnUrl) && Url.IsLocalUrl(returnUrl)
                ? LocalRedirect(returnUrl)
                : RedirectToAction(nameof(Index));

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
        }

        [HttpGet]
        [AllowAnonymous]
        public IActionResult ForgotPassword() => View();

        [HttpPost]
        [ValidateAntiForgeryToken]
        [AllowAnonymous]
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
                _logger.LogInformation("ToDo: send the '{resetUrl}' to {userEmail}", resetUrl, user.Email);
            }
            else
            {
                _logger.LogInformation("ToDo: send message to {modelEmail} informing them that they do not have an account", model.Email);
            }

            return View("ResetPasswordRequestSuccess");
        }

        [HttpGet]
        [AllowAnonymous]
        public IActionResult ResetPassword(string token, string email) =>
            View(new ResetPasswordModel {Token = token, Email = email});

        [HttpPost]
        [ValidateAntiForgeryToken]
        [AllowAnonymous]
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
        [AllowAnonymous]
        public IActionResult TwoFactor()
        {
            return View();
        }

        [HttpPost]
        [ValidateAntiForgeryToken]
        [AllowAnonymous]
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
        [System.Diagnostics.CodeAnalysis.SuppressMessage("Minor Code Smell", "S1075:URIs should not be hardcoded", Justification = "Constant URI required for QR code")]
        public async Task<IActionResult> RegisterAuthenticator()
        {
            var user = await _userManager.GetUserAsync(User);
            if (user == null)
            {
                await HttpContext.SignOutAsync();
                return RedirectToAction(nameof(Login));
            }

            var authenticatorKey = await GetExistingOrNewAuthentictorKeyAsync();
            ViewBag.AuthenticatorUri = GenerateQrCodeUri(user.Email, authenticatorKey);

            return View(new RegisterAuthenticatorModel {AuthenticatorKey = authenticatorKey});

            async Task<string> GetExistingOrNewAuthentictorKeyAsync()
            {
                var key = await _userManager.GetAuthenticatorKeyAsync(user);
                if (key != null)
                {
                    ViewBag.RecoveryCodes = (await _userManager.GenerateNewTwoFactorRecoveryCodesAsync(user, 10)).ToArray();
                    return key;
                }

                await _userManager.ResetAuthenticatorKeyAsync(user);
                ViewBag.RecoveryCodes = (await _userManager.GenerateNewTwoFactorRecoveryCodesAsync(user, 10)).ToArray();

                return await _userManager.GetAuthenticatorKeyAsync(user);
            }

            string GenerateQrCodeUri(string email, string unformattedKey)
            {
                const string authenticatorUriFormat = "otpauth://totp/{0}:{1}?secret={2}&issuer={0}&digits=6";

                return string.Format(
                    authenticatorUriFormat, 
                    _urlEncoder.Encode("IdentityDemo"),
                    _urlEncoder.Encode(email), 
                    _urlEncoder.Encode(unformattedKey));
            }


        }

        [HttpPost]
        [ValidateAntiForgeryToken]
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
        public async Task<IActionResult> ExternalLogin(string provider)
        {
            var schemas = (await _signInManager.GetExternalAuthenticationSchemesAsync())
                .ToArray();

            if (schemas.All(schema => schema.Name != provider))
                return View("Error", new ErrorViewModel {RequestId = Activity.Current?.Id ?? "Unknown"});

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
            var provider = result.Properties?.Items["scheme"] ?? string.Empty;

            if (string.IsNullOrEmpty(externalUserId))
                return View("Error", new ErrorViewModel {RequestId = Activity.Current?.Id ?? "Unkown"});

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

        [HttpPost]
        [ValidateAntiForgeryToken]
        public async Task<IActionResult> Logout()
        {
            await HttpContext.SignOutAsync(IdentityConstants.ApplicationScheme);
            await HttpContext.SignOutAsync(IdentityConstants.ExternalScheme);
            await HttpContext.SignOutAsync(IdentityConstants.TwoFactorUserIdScheme);

            return RedirectToAction(nameof(Index));
        }


        [HttpGet]
        [ResponseCache(CacheProfileName = "Default")]
        public IActionResult Profile() 
        {
            // not implemented yet
            return View();
        }

        [HttpGet]
        [AllowAnonymous]
        public IActionResult Privacy() => View();

        [HttpGet]
        [AllowAnonymous]
        [ResponseCache(Duration = 0, Location = ResponseCacheLocation.None, NoStore = true)]
        public IActionResult Error() =>
            View(new ErrorViewModel { RequestId = Activity.Current?.Id ?? HttpContext.TraceIdentifier });

        [HttpGet]
        [AllowAnonymous]
        public IActionResult AccessDenied() =>
            View();

        private ViewResult FailWithMessage(string message)
        {
            ModelState.AddModelError(string.Empty, message);
            return View();
        }
    }
}
