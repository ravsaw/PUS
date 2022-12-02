using System;
using System.Collections.Generic;
using System.ComponentModel.DataAnnotations;
using System.Linq;
using System.Threading.Tasks;
using Microsoft.AspNetCore.Authorization;
using Microsoft.AspNetCore.Authentication;
using Microsoft.AspNetCore.Identity;
using Microsoft.AspNetCore.Identity.UI.Services;
using Microsoft.AspNetCore.Mvc;
using Microsoft.AspNetCore.Mvc.RazorPages;
using Microsoft.Extensions.Logging;
using PUS.Models;
using PUS.ViewModels;
using Microsoft.EntityFrameworkCore;
using System.Net.NetworkInformation;
using Microsoft.AspNetCore.Identity.EntityFrameworkCore;
using Microsoft.AspNetCore.WebUtilities;
using System.Text.Encodings.Web;
using System.Text;

namespace PUS.Controllers
{
    public class AccountController : Controller
    {
        private readonly SignInManager<Profile> _signInManager;
        private readonly UserManager<Profile> _userManager;
        private readonly IUserStore<Profile> _userStore;
        private readonly IUserEmailStore<Profile> _emailStore;
        private readonly ILogger<RegisterViewModel> _logger;
        private readonly IEmailSender _emailSender;

        public AccountController(
            SignInManager<Profile> signInManager,
            UserManager<Profile> userManager,
            IUserStore<Profile> userStore,
            ILogger<RegisterViewModel> logger,
            IEmailSender emailSender)
        {
            _signInManager = signInManager;
            _userManager = userManager;
            _userStore = userStore;
            _emailStore = GetEmailStore();
            _logger = logger;
            _emailSender = emailSender;
        }

        [HttpGet]
        public async Task<IActionResult> Login()
        {
            await HttpContext.SignOutAsync(IdentityConstants.ExternalScheme);
            return PartialView();
        }

        [HttpPost]
        public async Task<JsonResult> Login(LoginViewModel model)
        {
            LoginStatus status = new LoginStatus();

            if (ModelState.IsValid)
            {
                // This doesn't count login failures towards account lockout
                // To enable password failures to trigger account lockout, set lockoutOnFailure: true
                var result = await _signInManager.PasswordSignInAsync(model.Email, model.Password, model.RememberMe, lockoutOnFailure: false);
                if (result.Succeeded)
                {
                    _logger.LogInformation("User logged in.");
                    status.Status = Status.Success;
                    status.Message = "Sukces";
                }
                else if (result.IsLockedOut)
                {
                    _logger.LogWarning("User account locked out.");
                    status.Status = Status.Locked;
                    status.Message = "Użytkownik jest zablokowany.";
                }
                else
                {
                    ModelState.AddModelError(string.Empty, "Invalid login attempt.");
                    status.Status = Status.WrongEmailOrPassword;
                    status.Message = "Niewłaściwy email lub hasło";
                }
            }
            else
            {
                status.Status = Status.Unknow;
                status.Message = "model not valid" + ModelState.Values;
            }

            return Json(status);
        }

        [HttpGet]
        public async Task<IActionResult> Register()
        {
            return PartialView();
        }

        [HttpPost]
        public async Task<IActionResult> Register(RegisterViewModel model, string returnUrl = "/")
        {

            if (ModelState.IsValid)
            {
                var user = CreateUser();

                await _userStore.SetUserNameAsync(user, model.Email, CancellationToken.None);
                await _emailStore.SetEmailAsync(user, model.Email, CancellationToken.None);
                var result = await _userManager.CreateAsync(user, model.Password);

                if (result.Succeeded)
                {
                    _logger.LogInformation("User created a new account with password.");

                    var userId = await _userManager.GetUserIdAsync(user);
                    var code = await _userManager.GenerateEmailConfirmationTokenAsync(user);
                    code = WebEncoders.Base64UrlEncode(Encoding.UTF8.GetBytes(code));
                    var callbackUrl = Url.Page(
                        "/Account/ConfirmEmail",
                        pageHandler: null,
                        values: new { area = "Identity", userId = userId, code = code, returnUrl = returnUrl },
                        protocol: Request.Scheme);

                    await _emailSender.SendEmailAsync(model.Email, "Confirm your email",
                        $"Please confirm your account by <a href='{HtmlEncoder.Default.Encode(callbackUrl)}'>clicking here</a>.");

                    if (_userManager.Options.SignIn.RequireConfirmedAccount)
                    {
                        return RedirectToPage("RegisterConfirmation", new { email = model.Email, returnUrl = returnUrl });
                    }
                    else
                    {
                        await _signInManager.SignInAsync(user, isPersistent: false);
                        return LocalRedirect(returnUrl);
                    }
                }
                foreach (var error in result.Errors)
                {
                    ModelState.AddModelError(string.Empty, error.Description);
                }
            }

            // If we got this far, something failed, redisplay form
            return LocalRedirect(returnUrl);
        }


        private Profile CreateUser()
        {
            try
            {
                return Activator.CreateInstance<Profile>();
            }
            catch
            {
                throw new InvalidOperationException($"Can't create an instance of '{nameof(Profile)}'. " +
                    $"Ensure that '{nameof(Profile)}' is not an abstract class and has a parameterless constructor, or alternatively " +
                    $"override the register page in /Areas/Identity/Pages/Account/Register.cshtml");
            }
        }

        private IUserEmailStore<Profile> GetEmailStore()
        {
            if (!_userManager.SupportsUserEmail)
            {
                throw new NotSupportedException("The default UI requires a user store with email support.");
            }
            return (IUserEmailStore<Profile>)_userStore;
        }
    }
}
