using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;
using Microsoft.AspNetCore.Mvc;
using Microsoft.AspNetCore.Mvc.RazorPages;
using Microsoft.AspNetCore.Mvc.Rendering;
using NorthwindApp.DTO;
using NorthwindApp.Interfaces;
using NorthwindApp.Models;

namespace NorthwindApp.Pages.Products
{
    public class CreateModel : PageModel
    {
        private readonly IRepository<Product> _productRepository;
        private readonly IRepository<Category> _categoryRepository;
        private readonly IRepository<Supplier> _supplierRepository;
        private readonly IList<Category> _categories;
        private readonly IList<Supplier> _suppliers;

        [BindProperty]
        public Product Product { get; set; }
        public SelectList Categories { get; set; }
        public SelectList Suppliers { get; set; }
        public BreadcrumbsConfiguration BreadcrumbsConfiguration { get; private set; }

        public CreateModel(
            IRepository<Product> productRepository,
            IRepository<Category> categoryRepository,
            IRepository<Supplier> supplierRepository)
        {
            _productRepository = productRepository;
            _categoryRepository = categoryRepository;
            _supplierRepository = supplierRepository;
            _categories = _categoryRepository.GetAll().ToList();
            _suppliers = _supplierRepository.GetAll().ToList();
        }

        public async Task<IActionResult> OnGetAsync()
        {
            Product = new Product();
            InitProperties();
            return Page();
        }

        public async Task<IActionResult> OnPostAsync(Product product)
        {
            if (!ModelState.IsValid)
            {
                InitProperties();
                return Page();
            }

            await _productRepository.CreateAsync(product);
            return RedirectToPage("./Index");
        }

        private void InitProperties()
        {
            Categories = new SelectList(_categories, nameof(Category.CategoryId), nameof(Category.CategoryName));
            Suppliers = new SelectList(_suppliers, nameof(Supplier.SupplierId), nameof(Supplier.CompanyName));
            BreadcrumbsConfiguration = new BreadcrumbsConfiguration
            {
                PageName = nameof(Product),
                Mode = BreadcrumbsMode.Create
            };
        }
    }
}
