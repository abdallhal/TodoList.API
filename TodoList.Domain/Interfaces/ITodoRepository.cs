
using TodoList.Domain.Entities;

namespace TodoList.Domain.Interfaces
{
    public interface ITodoRepository : IBaseRepository<TodoItem>
    {
        IQueryable<TodoItem> GetAllPending();
        Task<bool> MarkAsCompletedAsync(int id);
    }
}
