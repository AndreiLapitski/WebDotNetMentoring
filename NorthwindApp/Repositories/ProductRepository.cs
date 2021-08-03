using System.Linq;
using System.Threading.Tasks;
using Microsoft.EntityFrameworkCore;
using Microsoft.EntityFrameworkCore.ChangeTracking;
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

        public async Task<Product> GetByIdAsync(int id)
        {
            return await _context.Products.FindAsync(id);
        }

        public IQueryable<Product> GetAll()
        {
            return _context.Products
                .Include(product => product.Category)
                .Include(product => product.Supplier);
        }

        public async Task<int> CreateAsync(Product item)
        {
            EntityEntry<Product> newItem = await _context.Products.AddAsync(item);
            await SaveAsync();
            return newItem.Entity.ProductId;
        }

        public async Task<int> DeleteAsync(Product item)
        {
            _context.Remove(item);
            return await SaveAsync();
        }

        public Task<int> UpdateAsync(int id, Product item)
        {
            _context.Update(item);
            return _context.SaveChangesAsync();
        }

        public async Task<int> SaveAsync()
        {
            return await _context.SaveChangesAsync();
        }
    }
}
