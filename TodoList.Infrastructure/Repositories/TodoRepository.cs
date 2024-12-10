using Microsoft.EntityFrameworkCore;
using TodoList.Domain.Entities;
using TodoList.Domain.Interfaces;
using TodoList.Domain.Repositories;
namespace TodoList.Infrastructure.Repositories
{
    public class TodoRepository : BaseRepository<TodoItem>, ITodoRepository
    {
        public TodoRepository(TodoDbContext context) : base(context) { }

        public IQueryable<TodoItem> GetAllPending()
        {
            return  _dbSet.Where(t => !t.IsCompleted).AsQueryable();
        }

        public async Task<bool> MarkAsCompletedAsync(int id)
        {
            var item = await GetByIdAsync(id);
            if (item == null) return false;

            item.IsCompleted = true;
             Update(item);
           
            return true;
        }
    }
}
