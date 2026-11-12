using System.Text.Json.Serialization;
using Microsoft.EntityFrameworkCore;
using TodoApi.Api.ModelBinding;
using TodoApi.Application;
using TodoApi.Infrastructure;
using TodoApi.Infrastructure.Persistence;

var builder = WebApplication.CreateBuilder(args);

builder.Services.AddControllers(options =>
    {
        options.ModelBinderProviders.Insert(0, new StatusTarefaModelBinderProvider());
    })
    .AddJsonOptions(options =>
    {
        options.JsonSerializerOptions.Converters.Add(new JsonStringEnumConverter());
    });

builder.Services.AddEndpointsApiExplorer();
builder.Services.AddSwaggerGen(options =>
{
    options.SwaggerDoc("v1", new()
    {
        Title = "API de Tarefas",
        Version = "v1",
        Description = "API RESTful para gestão de tarefas (ToDo)."
    });
    options.UseInlineDefinitionsForEnums();
});

builder.Services.AddApplication();
builder.Services.AddInfrastructure(builder.Configuration);

var app = builder.Build();

using (var scope = app.Services.CreateScope())
{
    var dbContext = scope.ServiceProvider.GetRequiredService<AppDbContext>();
    dbContext.Database.Migrate();
}

app.UseSwagger();
app.UseSwaggerUI(options =>
{
    options.SwaggerEndpoint("/swagger/v1/swagger.json", "API de Tarefas v1");
    options.RoutePrefix = string.Empty;
});

app.MapControllers();
app.Run();

public partial class Program;
