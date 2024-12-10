
using System.Linq.Expressions;

namespace TodoList.Domain.Interfaces
{
    public interface IBaseRepository<T> where T : class
    {
        Task<T> AddAsync(T entity);
        Task<T?> GetByIdAsync(int id);
        Task<IEnumerable<T>> GetAllAsync();
        void Update(T entity);
        void Delete(T entity);
        Task SaveChangesAsync();
        IQueryable<T> GetAll(Expression<Func<T, bool>> filter = null);
        Task<bool> AnyAsync(Func<T, bool> predicate);
    }
}
