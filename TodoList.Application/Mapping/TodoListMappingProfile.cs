

using AutoMapper;
using TodoList.Application.DTO;
using TodoList.Domain.Entities;

namespace TodoList.Application.Mapping
{
    public class TodoListMappingProfile:Profile
    {
        public TodoListMappingProfile()
        {
            CreateMap<CreateTodoDto, TodoItem>();
            CreateMap<UpdateTodoDto, TodoItem>();
        }
    }
}
