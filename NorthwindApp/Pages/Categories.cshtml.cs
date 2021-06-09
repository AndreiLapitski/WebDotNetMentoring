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
        private IList<Category> _categories;

        public IList<string> RowNames => _rowNames ?? PropertyHelper.GetDisplayablePropertyNames(typeof(Category));
        public IList<Category> Categories { get; set; } = new List<Category>();

        public CategoriesModel(IRepository<Category> categoryRepository)
        {
            _categoryRepository = categoryRepository;
            _rowNames = PropertyHelper.GetDisplayablePropertyNames(typeof(Category));
            Init();
        }

        public void OnGet()
        {
            Categories = _categories;
        }

        private void Init()
        {
            _categories = _categoryRepository.GetAll().ToList();
        }
    }
}
