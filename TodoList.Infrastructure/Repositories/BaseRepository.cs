using Microsoft.EntityFrameworkCore;
using System.Linq.Expressions;
using TodoList.Domain.Interfaces;
using TodoList.Infrastructure;

namespace TodoList.Domain.Repositories
{
    public class BaseRepository<T> : IBaseRepository<T> where T : class
    {
        protected readonly TodoDbContext _context;
        protected readonly DbSet<T> _dbSet;

        public BaseRepository(TodoDbContext context)
        {
            _context = context;
            _dbSet = context.Set<T>();
        }

        public async Task<T> AddAsync(T entity)
        {
            await _dbSet.AddAsync(entity);
            return entity;
        }

        public async Task<T?> GetByIdAsync(int id)
        {
            return await _dbSet.FindAsync(id);
        }

        public async Task<IEnumerable<T>> GetAllAsync()
        {
            return await _dbSet.ToListAsync();
        }
        public IQueryable<T> GetAll(Expression<Func<T, bool>> filter = null)
        {
            IQueryable<T> query = _dbSet.AsNoTracking();

            if (filter != null)
            {
                query = query.Where(filter);
            }
            return query;
        }

        public void Update(T entity)
        {
            _dbSet.Update(entity);
        }

        public void  Delete(T entity)
        {
            _dbSet.Remove(entity);
        }

     
        public async Task SaveChangesAsync()
        {
            await _context.SaveChangesAsync();
        }

        public async Task<bool> AnyAsync(Func<T, bool> predicate)
        {
            return await Task.FromResult(_dbSet.AsQueryable().Any(predicate));
        }

    }

}
