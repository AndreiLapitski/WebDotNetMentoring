using System.Linq;
using System.Threading.Tasks;

namespace NorthwindApp.Interfaces 
{
    public interface IRepository<T> where T : class
    {
        Task<T> GetByIdAsync(int id);

        IQueryable<T> GetAll();

        Task<int> CreateAsync(T item);

        Task<int> SaveAsync();
    }
}
