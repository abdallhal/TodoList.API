
using Serilog;
using TodoList.Presentation.Configurator;

var builder = WebApplication.CreateBuilder(args);


builder.ConfigureLogging();


builder.ConfigureDatabase();


builder.ConfigureServices();


builder.Services.AddControllers();
builder.Services.AddEndpointsApiExplorer();
builder.Services.AddSwaggerGen();


builder.Services.AddLogging();

var app = builder.Build();


app.UseSerilogRequestLogging();
app.UseAuthorization();
app.MapControllers();


app.ConfigureSwagger();

app.Run();
