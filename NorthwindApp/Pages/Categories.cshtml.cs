using System.Collections.Generic;
using System.Linq;
using Microsoft.AspNetCore.Mvc.RazorPages;
using NorthwindApp.Helpers;
using NorthwindApp.Interfaces;
using NorthwindApp.Models;

namespace NorthwindApp.Pages
{
    public class CategoriesModel : PageModel
    {
        private readonly IRepository<Category> _categoryRepository;
        private readonly IList<string> _rowNames;

        public IList<Category> Categories { get; set; }
        public IList<string> RowNames => _rowNames ?? PropertyHelper.GetDisplayablePropertyNames(typeof(Category));

        public CategoriesModel(IRepository<Category> categoryRepository)
        {
            _categoryRepository = categoryRepository;
            _rowNames = PropertyHelper.GetDisplayablePropertyNames(typeof(Category));
        }

        public void OnGet()
        {
            Categories = _categoryRepository.GetAll().ToList();
        }
    }
}
