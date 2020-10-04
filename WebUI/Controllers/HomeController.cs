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
using System.Threading.Tasks;
using IdentityDomain;
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
        private readonly SignInManager<DemoUser> _signInManager;
        private readonly ILogger<HomeController> _logger;

        public HomeController(
            UserManager<DemoUser> userManager, 
            IUserClaimsPrincipalFactory<DemoUser> userClaimsPrincipalFactory,
            SignInManager<DemoUser> signInManager,
            ILogger<HomeController> logger)
        {
            _userManager = userManager ?? throw new ArgumentNullException(nameof(userManager));
            _userClaimsPrincipalFactory = userClaimsPrincipalFactory ?? throw new ArgumentNullException(nameof(userClaimsPrincipalFactory));
            _signInManager = signInManager ?? throw new ArgumentNullException(nameof(signInManager));
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

            var result = await _userManager.CreateAsync(user, model.Password);
            if (!result.Succeeded) 
                return View();

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


#           if USING_USER_MANAGER
            var user = await _userManager.FindByNameAsync(model.UserName);
            if (user != null && await _userManager.IsEmailConfirmedAsync(user))
            {
                ModelState.AddModelError(string.Empty, "E-mail is not confirmed");
                return View()e
            }
#           endif

            // if customization is required consider moving to userManager instead of signInManager
            // while it is good signInManager can obscure a lot of the details which may be needed for more complex
            // systems
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

            return View("ResetPasswordSuccess");
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
    }
}
