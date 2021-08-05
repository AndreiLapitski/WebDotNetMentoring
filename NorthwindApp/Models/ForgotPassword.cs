using System.ComponentModel.DataAnnotations;

namespace NorthwindApp.Models
{
    public class ForgotPassword
    {
        [Required]
        [EmailAddress]
        [Display(Name = "Email")]
        public string Email { get; set; }
    }
}
