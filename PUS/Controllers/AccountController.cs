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
using SixLabors.ImageSharp;
using SixLabors.ImageSharp.Processing;

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
        private readonly IWebHostEnvironment _appEnvironment;

        public AccountController(
            SignInManager<Profile> signInManager,
            UserManager<Profile> userManager,
            IUserStore<Profile> userStore,
            ILogger<RegisterViewModel> logger,
            IEmailSender emailSender,
            IWebHostEnvironment appEnvironment)
        {
            _signInManager = signInManager;
            _userManager = userManager;
            _userStore = userStore;
            _emailStore = GetEmailStore();
            _logger = logger;
            _emailSender = emailSender;
            _appEnvironment = appEnvironment;
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
            AccountRequestStatus status = new AccountRequestStatus();

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
        public IActionResult Register()
        {
            return PartialView("Register", new RegisterViewModel());
        }


        [HttpPost]
        public async Task<IActionResult> Register(RegisterViewModel vm)
        {
            string returnUrl = "/";
            AccountRequestStatus status = new AccountRequestStatus();

            if (ModelState.IsValid)
            {
                var user = CreateUser();
                user.FirstName = vm.FirstName;
                user.LastName = vm.LastName;
                user.PhoneNumber = vm.PhoneNumber;
                user.BirthDate = vm.BirthDate;

                var address = new Address() { 
                    Address1 = vm.Address1, 
                    Address2 = vm.Address2, 
                    PostCode = vm.PostCode, 
                    City = vm.City 
                };

                user.Addresses.Add(address);

                await _userStore.SetUserNameAsync(user, vm.Email, CancellationToken.None);
                await _emailStore.SetEmailAsync(user, vm.Email, CancellationToken.None);
                var result = await _userManager.CreateAsync(user, vm.Password);

                if (result.Succeeded)
                {
                    _logger.LogInformation("User created a new account with password.");

                    var userId = await _userManager.GetUserIdAsync(user);
                    var code = await _userManager.GenerateEmailConfirmationTokenAsync(user);
                    code = WebEncoders.Base64UrlEncode(Encoding.UTF8.GetBytes(code));
                    var callbackUrl = Url.Page(
                        "/Account/ConfirmEmail",
                        pageHandler: null,
                        values: new { area = "Identity", userId, code, returnUrl },
                        protocol: Request.Scheme);

                    await _emailSender.SendEmailAsync(vm.Email, "Confirm your email",
                        $"Please confirm your account by <a href='{HtmlEncoder.Default.Encode(callbackUrl)}'>clicking here</a>.");

                    if (vm.ProfileImage.Length > 0)
                    {
                        var filePath = Path.Combine(_appEnvironment.WebRootPath, "img", "users", userId + ".jpeg");

                        using var image = Image.Load(vm.ProfileImage.OpenReadStream());
                        var cropArea = new Rectangle(
                            (int)(vm.CropX / vm.CropScale), (int)(vm.CropY / vm.CropScale),
                            (int)(500 / vm.CropScale), (int)(500 / vm.CropScale)
                            );
                        image.Mutate(x => x.Crop(cropArea));
                        image.Mutate(x => x.Resize(500, 500));
                        await image.SaveAsJpegAsync(filePath);
                    }

                    if (_userManager.Options.SignIn.RequireConfirmedAccount)
                    {
                        _logger.LogInformation("User register and need confirm.");
                        return Json(new { success = true });
                    }
                    else
                    {
                        await _signInManager.SignInAsync(user, isPersistent: false);
                        return Json(new { success = true });
                    }
                }
                foreach (var error in result.Errors)
                {
                    ModelState.AddModelError(string.Empty, error.Description);
                }
            }

            // If we got this far, something failed, redisplay form
            return PartialView("Register", vm);
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
