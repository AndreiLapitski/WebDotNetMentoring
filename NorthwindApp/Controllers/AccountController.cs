using Microsoft.AspNetCore.Mvc;
using System;
using System.Linq;
using System.Security.Authentication;
using System.Threading.Tasks;
using Microsoft.AspNetCore.Authentication;
using Microsoft.AspNetCore.Authentication.OpenIdConnect;
using Microsoft.AspNetCore.Authorization;
using Microsoft.AspNetCore.Identity;
using Microsoft.Extensions.Configuration;
using NorthwindApp.Helpers;
using Serilog;

namespace NorthwindApp.Controllers
{
    [Route("[controller]/[action]")]
    [AllowAnonymous]
    public class AccountController : Controller
    {
        private readonly IConfiguration _configuration;
        private readonly UserManager<IdentityUser> _userManager;
        private readonly SignInManager<IdentityUser> _signInManager;

        public AccountController(
            IConfiguration configuration,
            UserManager<IdentityUser> userManager,
            SignInManager<IdentityUser> signInManager)
        {
            _configuration = configuration;
            _userManager = userManager;
            _signInManager = signInManager;
        }

        [HttpGet]
        public IActionResult ExternalSignIn()
        {
            return Challenge(new AuthenticationProperties
            {
                RedirectUri = _configuration.GetValue<string>(Constants.ExternalRedirectUrlKey)
            }, OpenIdConnectDefaults.AuthenticationScheme);
        }

        [HttpGet]
        public async Task<IActionResult> ExternalLoginHandler()
        {
            AuthenticateResult externalAuthenticateResult =
                await HttpContext.AuthenticateAsync(OpenIdConnectDefaults.AuthenticationScheme);

            if (externalAuthenticateResult.Succeeded)
            {
                string externalUserEmail = externalAuthenticateResult.Principal.Identity.Name;
                IdentityUser existedUser = await _userManager.FindByEmailAsync(externalUserEmail);
                if (existedUser == null)
                {
                    IdentityUser newUser = new IdentityUser
                    {
                        Email = externalUserEmail,
                        UserName = externalUserEmail
                    };

                    IdentityResult result = await _userManager.CreateAsync(newUser);
                    if (result.Succeeded)
                    {
                        Log.Information($"New account for {externalUserEmail} has been created successfully by the external login handler.");
                        existedUser = newUser;
                    }
                    else
                    {
                        Log.Error($"User with the {externalUserEmail} email wasn't be created. Identity errors: {result.Errors.Aggregate(string.Empty, (current, error) => current + $"Code: {error.Code}. Description: {error.Description}{Environment.NewLine}")}");
                        throw new AuthenticationException();
                    }
                }

                Log.Information($"{existedUser.Email} logged in by the external login handler.");
                await _signInManager.SignInAsync(existedUser, false);
            }

            return RedirectToAction("Index", "Home");
        }
    }
}
