using System.Threading.Tasks;
using Microsoft.AspNetCore.Authorization;
using Microsoft.AspNetCore.Identity;
using Microsoft.AspNetCore.Mvc;
using Microsoft.AspNetCore.Mvc.RazorPages;
using NorthwindApp.Models;

namespace NorthwindApp.Pages.Account
{
    [AllowAnonymous]
    public class ResetPasswordModel : PageModel
    {
        private readonly UserManager<IdentityUser> _userManager;

        public ResetPasswordModel(UserManager<IdentityUser> userManager)
        {
            _userManager = userManager;
        }

        [BindProperty]
        public ResetPassword Input { get; set; }

        public IActionResult OnGet(string resetToken = null)
        {
            if (resetToken == null)
            {
                return BadRequest("A reset token must be supplied for password reset.");
            }

            Input = new ResetPassword
            {
                ResetToken = resetToken
            };

            return Page();
        }

        public async Task<IActionResult> OnPostAsync()
        {
            if (!ModelState.IsValid)
            {
                return Page();
            }

            IdentityUser user = await _userManager.FindByEmailAsync(Input.Email);
            if (user == null)
            {
                return Page();
            }

            IdentityResult result = await _userManager.ResetPasswordAsync(user, Input.ResetToken, Input.Password);
            if (result.Succeeded)
            {
                ViewData["Message"] = "Your password has been changed successfully.";
                return Page();
            }

            foreach (var error in result.Errors)
            {
                ModelState.AddModelError(string.Empty, error.Description);
            }

            return Page();
        }
    }
}
