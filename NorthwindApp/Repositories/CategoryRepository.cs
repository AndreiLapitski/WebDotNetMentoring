using System.Linq;
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

        public Category GetById(int id)
        {
            return _context.Categories.Find(id);
        }

        public IQueryable<Category> GetAll()
        {
            return _context.Categories;
        }
    }
}
