using Microsoft.AspNetCore.Mvc;
using Microsoft.AspNetCore.Mvc.RazorPages;
using Microsoft.Extensions.Configuration;
using NorthwindApp.Helpers;

namespace NorthwindApp.Pages
{
    public class IndexModel : PageModel
    {
        private readonly IConfiguration _configuration;

        public string SwaggerUIUrl { get; set; }

        public IndexModel(IConfiguration configuration)
        {
            _configuration = configuration;
            SwaggerUIUrl = _configuration.GetValue<string>(Constants.SwaggerUIUrlKey);
        }
        public IActionResult OnGet(int? code)
        {
            if (code != null)
            {
                switch (code)
                {
                    case 1:
                        int a = 1;
                        int b = 0;
                        int c = a / b;
                        break;
                }
            }

            return Page();
        }
    }
}
