using Serilog;

namespace TodoList.Presentation.Configurator
{
    public static class LoggingConfigurator
    {
        public static void ConfigureLogging(this WebApplicationBuilder builder)
        {
            Log.Logger = new LoggerConfiguration()
                .WriteTo.Console() 
                .WriteTo.File("logs\\log-.txt", rollingInterval: RollingInterval.Day) 
                .CreateLogger();


            builder.Host.UseSerilog();
        }
    }
}
