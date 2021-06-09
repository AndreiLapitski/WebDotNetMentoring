using System.Linq;
using Microsoft.EntityFrameworkCore;
using NorthwindApp.Interfaces;
using NorthwindApp.Models;

namespace NorthwindApp.Repositories
{
    public class ProductRepository : IRepository<Product>
    {
        private readonly NorthwindContext _context;

        public ProductRepository(NorthwindContext context)
        {
            _context = context;
        }

        public Product GetById(int id)
        {
            return _context.Products.Find(id);
        }

        public IQueryable<Product> GetAll()
        {
            return _context.Products
                .Include(product => product.Category)
                .Include(product => product.Supplier);
        }
    }
}
