using System.Linq;
using System.Threading.Tasks;
using Microsoft.EntityFrameworkCore.ChangeTracking;
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
            EntityEntry<Supplier> newItem = await _context.Suppliers.AddAsync(item);
            await SaveAsync();
            return newItem.Entity.SupplierId;
        }

        public Task<int> DeleteAsync(Supplier item)
        {
            throw new System.NotImplementedException();
        }

        public Task<int> UpdateAsync(int id, Supplier item)
        {
            throw new System.NotImplementedException();
        }

        public async Task<int> SaveAsync()
        {
            return await _context.SaveChangesAsync();
        }
    }
}
