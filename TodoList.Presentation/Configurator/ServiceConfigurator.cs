using TodoList.Application.Interfaces;
using TodoList.Application.Mapping;
using TodoList.Application.Services;
using TodoList.Domain.Interfaces;
using TodoList.Domain.Repositories;
using TodoList.Infrastructure.Repositories;

namespace TodoList.Presentation.Configurator
{
    public static class ServiceConfigurator
    {
        public static void ConfigureServices(this WebApplicationBuilder builder)
        {
            builder.Services.AddScoped(typeof(IBaseRepository<>), typeof(BaseRepository<>));
            builder.Services.AddScoped<ITodoRepository, TodoRepository>();

           
            builder.Services.AddScoped<ITodoService, TodoService>();

          
            builder.Services.AddAutoMapper(typeof(TodoListMappingProfile));
        }
    }
}
