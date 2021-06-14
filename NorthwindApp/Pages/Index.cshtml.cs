using Microsoft.AspNetCore.Mvc;
using Microsoft.AspNetCore.Mvc.RazorPages;

namespace NorthwindApp.Pages
{
    public class IndexModel : PageModel
    {
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
