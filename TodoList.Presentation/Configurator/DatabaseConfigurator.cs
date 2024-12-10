using Microsoft.EntityFrameworkCore;
using TodoList.Infrastructure;

namespace TodoList.Presentation.Configurator
{
    public static class DatabaseConfigurator
    {
        public static void ConfigureDatabase(this WebApplicationBuilder builder)
        {
            builder.Services.AddDbContext<TodoDbContext>(options =>
                options.UseInMemoryDatabase("TodoDb"));
        }
    }
}
