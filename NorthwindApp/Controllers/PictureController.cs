using System.Linq;
using System.Threading.Tasks;
using Microsoft.AspNetCore.Mvc;
using NorthwindApp.Helpers;
using NorthwindApp.Interfaces;
using NorthwindApp.Models;

namespace NorthwindApp.Controllers
{
    public class PictureController : Controller
    {
        private readonly IRepository<Category> _categoryRepository;

        public PictureController(IRepository<Category> categoryRepository)
        {
            _categoryRepository = categoryRepository;
        }

        public async Task<IActionResult> DownloadCategoryPicture(int categoryId)
        {
            Category category = await _categoryRepository.GetByIdAsync(categoryId);
            if(category?.Picture == null || !category.Picture.Any())
            {
                return NotFound();
            }

            return File(
                category.Picture,
                Constants.ContentTypeDownload,
                $"{category.CategoryName}.jpg");
        }
    }
}
