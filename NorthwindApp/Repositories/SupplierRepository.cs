using System.Linq;
using System.Threading.Tasks;
using NorthwindApp.Interfaces;
using NorthwindApp.Models;

namespace NorthwindApp.Repositories
{
    public class SupplierRepository : IRepository<Supplier>
    {
        private readonly NorthwindContext _context;

        public SupplierRepository(NorthwindContext context)
        {
            _context = context;
        }

        public async Task<Supplier> GetByIdAsync(int id)
        {
            return await _context.Suppliers.FindAsync(id);
        }

        public IQueryable<Supplier> GetAll()
        {
            return _context.Suppliers;
        }

        public async Task<int> CreateAsync(Supplier item)
        {
            await _context.Suppliers.AddAsync(item);
            return await SaveAsync();
        }

        public async Task<int> SaveAsync()
        {
            return await _context.SaveChangesAsync();
        }
    }
}
