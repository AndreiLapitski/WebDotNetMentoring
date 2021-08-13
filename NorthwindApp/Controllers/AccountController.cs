using Microsoft.AspNetCore.Mvc;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;
using Microsoft.AspNetCore.Authentication;
using Microsoft.AspNetCore.Authentication.AzureAD.UI;
using Microsoft.AspNetCore.Authentication.Cookies;
using Microsoft.AspNetCore.Authentication.OpenIdConnect;
using Microsoft.AspNetCore.Authorization;
using Microsoft.AspNetCore.Identity;

namespace NorthwindApp.Controllers
{
    [Route("[controller]/[action]")]
    public class AccountController : Controller
    {
        private readonly SignInManager<IdentityUser> _signInManager;

        public AccountController(SignInManager<IdentityUser> signInManager)
        {
            _signInManager = signInManager;
        }

        //[HttpGet]// [HttpGet("internal-signin")]
        //public ChallengeResult SignIn(string returnUrl = "/")
        //{
        //    return Challenge(
        //        new AuthenticationProperties {RedirectUri = returnUrl},
        //        AzureADDefaults.AuthenticationScheme);
        //}

        //[HttpGet]// [HttpGet("signout")]
        //[Authorize]
        //public async Task<IActionResult> SignOut()
        //{
        //    await _signInManager.SignOutAsync();
        //    return RedirectToAction("Index", "Home");
        //}

        [HttpGet]
        public ChallengeResult SignIn()
        {
            //https://stackoverflow.com/questions/60198623/how-to-use-both-azure-ad-authentication-and-identity-on-asp-net-core-3
            AuthenticationProperties authenticationProperties =
                _signInManager.ConfigureExternalAuthenticationProperties(
                    OpenIdConnectDefaults.AuthenticationScheme,
                    "/");

            return new ChallengeResult(OpenIdConnectDefaults.AuthenticationScheme, authenticationProperties);
        }


        //[HttpGet]
        //public ChallengeResult SignIn()
        //{
        //    var authenticationProperties =
        //        _signInManager.ConfigureExternalAuthenticationProperties(OpenIdConnectDefaults.AuthenticationScheme, "/");

        //    //return Challenge(
        //    //    new AuthenticationProperties { RedirectUri = "/" },
        //    //    OpenIdConnectDefaults.AuthenticationScheme);

        //    return new ChallengeResult(OpenIdConnectDefaults.AuthenticationScheme, authenticationProperties);
        //    //return Challenge(
        //    //    new AuthenticationProperties { RedirectUri = "/" },
        //    //    AzureADDefaults.AuthenticationScheme);
        //}

        [HttpGet]
        public async Task<IActionResult> SignOut()
        {
            await HttpContext.SignOutAsync("Cookies");
            return RedirectToPage("/Index");
        }

        //[HttpGet]
        //public IActionResult SignOut()
        //{
        //    string callbackUrl = Url.Page("/Account/SignedOut", null, null, Request.Scheme);

        //    //return SignOut(
        //    //    new AuthenticationProperties { RedirectUri = callbackUrl },
        //    //    CookieAuthenticationDefaults.AuthenticationScheme,
        //    //    AzureADDefaults.AuthenticationScheme);

        //    return SignOut(
        //        new AuthenticationProperties { RedirectUri = callbackUrl },
        //        CookieAuthenticationDefaults.AuthenticationScheme,
        //        OpenIdConnectDefaults.AuthenticationScheme);
        //}

        [HttpGet]
        public IActionResult SignedOut()
        {
            if (HttpContext.User.Identity.IsAuthenticated)
            {
                //return RedirectToAction(nameof(HomeController.Index), "Home");
                return BadRequest();
            }

            //return View();
            return Ok();
        }

        [HttpGet]
        public IActionResult AccessDenied()
        {
            return BadRequest();
        }
    }
}
