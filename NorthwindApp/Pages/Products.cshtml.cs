using System.Collections.Generic;
using System.Linq;
using Microsoft.AspNetCore.Mvc.RazorPages;
using NorthwindApp.Helpers;
using NorthwindApp.Interfaces;
using NorthwindApp.Models;

namespace NorthwindApp.Pages
{
    public class ProductsModel : PageModel
    {
        private readonly IRepository<Product> _productRepository;
        private readonly IList<string> _rowNames;
        private IList<Product> _products;

        public IList<string> RowNames => _rowNames ?? PropertyHelper.GetDisplayablePropertyNames(typeof(Product));
        public IList<Product> Products { get; set; } = new List<Product>();

        public ProductsModel(IRepository<Product> productRepository)
        {
            _productRepository = productRepository;
            _rowNames = PropertyHelper.GetDisplayablePropertyNames(typeof(Product));
            Init();
        }

        public void OnGet()
        {
            Products = _products;
        }

        private void Init()
        {
            _products = _productRepository.GetAll().ToList();
        }
    }
}
