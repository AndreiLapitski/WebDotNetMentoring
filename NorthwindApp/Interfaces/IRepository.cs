using System.Linq;

namespace NorthwindApp.Interfaces 
{
    public interface IRepository<T> where T : class
    {
        T GetById(int id);

        IQueryable<T> GetAll();
    }
}
