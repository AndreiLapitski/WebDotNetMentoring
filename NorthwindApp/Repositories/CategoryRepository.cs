using System.Linq;
using System.Threading.Tasks;
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
            await _context.Categories.AddAsync(item);
            return await SaveAsync();
        }

        public async Task<int> SaveAsync()
        {
            return await _context.SaveChangesAsync();
        }
    }
}
