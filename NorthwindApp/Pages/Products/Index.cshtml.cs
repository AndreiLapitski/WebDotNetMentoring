using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;
using Microsoft.AspNetCore.Mvc;
using Microsoft.AspNetCore.Mvc.RazorPages;
using Microsoft.Extensions.Configuration;
using NorthwindApp.Helpers;
using NorthwindApp.Interfaces;
using NorthwindApp.Models;

namespace NorthwindApp.Pages.Products
{
    public class IndexModel : PageModel
    {
        private readonly IConfiguration _configuration;
        private readonly IRepository<Product> _productRepository;
        private readonly IList<string> _rowNames;

        public IList<string> RowNames => _rowNames ?? PropertyHelper.GetDisplayablePropertyNames(typeof(Product));
        public PaginatedList<Product> Products { get; set; }

        public IndexModel(IConfiguration configuration, IRepository<Product> productRepository)
        {
            _configuration = configuration;
            _productRepository = productRepository;
            _rowNames = PropertyHelper.GetDisplayablePropertyNames(typeof(Product));
        }

        public async Task OnGetAsync(int? pageIndex)
        {
            int productsPageSize = _configuration.GetValue<int>(Constants.ProductsPageSize);
            ViewData["ProductsPageSize"] = productsPageSize;
            await Init(productsPageSize, pageIndex);
        }

        private async Task Init(int productsPageSize, int? pageIndex)
        {
            Products = await PaginatedList<Product>.CreateAsync(_productRepository.GetAll(), pageIndex ?? 1, productsPageSize);
        }
    }
}
