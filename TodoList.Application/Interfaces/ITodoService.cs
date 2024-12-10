

using TodoList.Application.DTO;
using TodoList.Domain.Common;
using TodoList.Domain.Common.PageList;
using TodoList.Domain.Entities;

namespace TodoList.Application.Interfaces
{
    public interface ITodoService
    {
        BaseResponse<IPagedList<TodoItem>> GetAll(GetTodoSearchDTO getTodoSearchDTO, PaginationDto paginationDto);
        BaseResponse<IPagedList<TodoItem>> GetAllPending(GetTodoSearchDTO getTodoSearchDTO, PaginationDto paginationDto);
        Task<BaseResponse<TodoItem>> CreateAsync(CreateTodoDto dto);
        Task<BaseResponse<TodoItem>> UpdateAsync(int id, UpdateTodoDto dto);
        Task<BaseResponse<object>> CompleteAsync(int id);
        Task<BaseResponse<object>> DeleteAsync(int id);
    }
}
