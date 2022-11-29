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

namespace PUS.Controllers
{
    public class AccountController : Controller
    {
        private readonly SignInManager<Profile> _signInManager;
        private readonly ILogger<LoginViewModel> _logger;

        public AccountController(SignInManager<Profile> signInManager, ILogger<LoginViewModel> logger)
        {
            _signInManager = signInManager;
            _logger = logger;
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
    }
}
