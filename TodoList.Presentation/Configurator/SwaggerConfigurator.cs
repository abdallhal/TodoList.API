namespace TodoList.Presentation.Configurator
{

    public static class SwaggerConfigurator
    {
        public static void ConfigureSwagger(this WebApplication app)
        {
            if (app.Environment.IsDevelopment())
            {
                app.UseSwagger();
                app.UseSwaggerUI();
            }
        }
    }

}
