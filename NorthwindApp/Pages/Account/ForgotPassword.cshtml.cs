using System.Threading.Tasks;
using Microsoft.AspNetCore.Identity;
using Microsoft.AspNetCore.Identity.UI.Services;
using Microsoft.AspNetCore.Mvc;
using Microsoft.AspNetCore.Mvc.RazorPages;
using NorthwindApp.Helpers;
using NorthwindApp.Models;
using Serilog;

namespace NorthwindApp.Pages.Account
{
    public class ForgotPasswordModel : PageModel
    {
        private readonly UserManager<IdentityUser> _userManager;
        private readonly IEmailSender _emailSender;

        public ForgotPasswordModel(
            UserManager<IdentityUser> userManager,
            IEmailSender emailSender)
        {
            _userManager = userManager;
            _emailSender = emailSender;
        }

        [BindProperty]
        public ForgotPassword Input { get; set; }

        public void OnGet()
        {
        }

        public async Task<IActionResult> OnPostAsync()
        {
            ViewData["Message"] = "The link has been sent, please check your email to reset your password.";

            if (!ModelState.IsValid)
            {
                Log.Information($"Forgot password email has not been sent to {Input.Email}.");
                return Page();
            }

            IdentityUser user = await _userManager.FindByEmailAsync(Input.Email);
            string resetToken = await _userManager.GeneratePasswordResetTokenAsync(user);
            string resetPasswordUrl = Url.Page(
                "/Account/ResetPassword",
                null,
                new { email = Input.Email, resetToken },
                Request.Scheme);

            string message = $"To reset your password use this <a href='{resetPasswordUrl}'>link</a>, please.";
            await _emailSender.SendEmailAsync(Input.Email, Constants.ResetEmailSubjectKey, message);

            Log.Information($"Forgot password email has been sent to {Input.Email}.");
            return Page();
        }
    }
}
