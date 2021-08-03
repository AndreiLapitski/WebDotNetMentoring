using System.Linq;
using System.Threading.Tasks;
using Microsoft.EntityFrameworkCore.ChangeTracking;
using NorthwindApp.Interfaces;
using NorthwindApp.Models;

namespace NorthwindApp.Repositories
{
    public class CategoryRepository : IRepository<Category>
    {
        private readonly NorthwindContext _context;

        public CategoryRepository(NorthwindContext context)
        {
            _context = context;
        }

        public async Task<Category> GetByIdAsync(int id)
        {
            return await _context.Categories.FindAsync(id);
        }

        public IQueryable<Category> GetAll()
        {
            return _context.Categories;
        }

        public async Task<int> CreateAsync(Category item)
        {
            EntityEntry<Category> newItem = await _context.Categories.AddAsync(item);
            await SaveAsync();
            return newItem.Entity.CategoryId;
        }

        public Task<int> DeleteAsync(Category item)
        {
            throw new System.NotImplementedException();
        }

        public Task<int> UpdateAsync(int id, Category item)
        {
            throw new System.NotImplementedException();
        }

        public async Task<int> SaveAsync()
        {
            return await _context.SaveChangesAsync();
        }
    }
}
