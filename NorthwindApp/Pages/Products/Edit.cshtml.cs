using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;
using Microsoft.AspNetCore.Mvc;
using Microsoft.AspNetCore.Mvc.RazorPages;
using Microsoft.AspNetCore.Mvc.Rendering;
using NorthwindApp.Interfaces;
using NorthwindApp.Models;

namespace NorthwindApp.Pages.Products
{
    public class EditModel : PageModel
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

        public EditModel(
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

        public async Task<IActionResult> OnGetAsync(int id)
        {
            Product = await _productRepository.GetByIdAsync(id);
            if (Product == null)
            {
                throw new Exception($"Product not found by ID = {id}");
            }

            InitProperties();

            return Page();
        }

        public async Task<IActionResult> OnPostAsync(int id)
        {
            if (!ModelState.IsValid)
            {
                InitProperties();
                return Page();
            }

            Product productForUpdate = await _productRepository.GetByIdAsync(id);
            if (productForUpdate == null)
            {
                throw new Exception($"Product not found by ID = {id}");
            }

            if (!await TryUpdateModelAsync(productForUpdate, "product",
                p => p.ProductName,
                p => p.SupplierId,
                p => p.CategoryId,
                p => p.QuantityPerUnit,
                p => p.UnitPrice,
                p => p.UnitsInStock,
                p => p.UnitsOnOrder,
                p => p.ReorderLevel,
                p => p.Discontinued))
            {
                return Page();
            }

            await _productRepository.SaveAsync();
            return RedirectToPage("./Index");
        }

        private void InitProperties()
        {
            Categories = new SelectList(_categories, nameof(Category.CategoryId), nameof(Category.CategoryName));
            Suppliers = new SelectList(_suppliers, nameof(Supplier.SupplierId), nameof(Supplier.CompanyName));
        }
    }
}
