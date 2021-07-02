using System.IO;
using System.Threading.Tasks;
using Microsoft.AspNetCore.Http;
using Microsoft.AspNetCore.Mvc;
using Microsoft.AspNetCore.Mvc.RazorPages;
using Microsoft.Extensions.Configuration;
using NorthwindApp.Helpers;
using NorthwindApp.Interfaces;
using NorthwindApp.Models;

namespace NorthwindApp.Pages.Categories
{
    public class EditModel : PageModel
    {
        private readonly IConfiguration _configuration;
        private readonly IRepository<Category> _categoryRepository;

        [BindProperty]
        public Category Category { get; set; }
        [BindProperty]
        public IFormFile Upload { get; set; }
        [BindProperty]
        public string CategoryPictureHref { get; set; }
        [BindProperty]
        public string AcceptedFiles { get; set; }

        public EditModel(IConfiguration configuration, IRepository<Category> categoryRepository)
        {
            _configuration = configuration;
            _categoryRepository = categoryRepository;
        }

        public async Task<IActionResult> OnGetAsync(int categoryId)
        {
            await Init(categoryId);
            return Page();
        }

        public async Task<IActionResult> OnPostAsync(int categoryId)
        {
            if (Upload != null)
            {
                await using MemoryStream stream = new MemoryStream();
                await Upload.CopyToAsync(stream);
                byte[] bytes = stream.ToArray();

                Category category = await _categoryRepository.GetByIdAsync(categoryId);
                category.Picture = bytes;

                await _categoryRepository.SaveAsync();
            }

            return RedirectToPage("./Index");
        }

        private async Task Init(int categoryId)
        {
            AcceptedFiles = _configuration.GetValue<string>(Constants.AcceptedFiles);
            Category = await _categoryRepository.GetByIdAsync(categoryId);
            CategoryPictureHref = 
                $"{Request.Scheme}://{Request.Host}/Picture/DownloadCategoryPicture?categoryId={Category.CategoryId}";
        }
    }
}
