using Microsoft.OpenApi.Models;

var builder = WebApplication.CreateBuilder(args);

// Add services to the container.

builder.Services.AddControllers();
//builder.Services.AddEndpointsApiExplorer();
builder.Services.AddSwaggerGen(c =>
{ //<-- NOTE 'Add' instead of 'Configure'
    c.SwaggerDoc("v1", new OpenApiInfo
    {
        //"swagger": "2.0",
        Title = "CV",
        Version = "v2"
    });
    c.ResolveConflictingActions(apiDescriptions => apiDescriptions.First()); //This line
});

var app = builder.Build();

// Configure the HTTP request pipeline.
app.UseSwagger();
app.UseSwaggerUI(c =>
{
    c.SwaggerEndpoint("/swagger/v1/swagger.json", "My API V1");
    c.DefaultModelsExpandDepth(-1);
});
app.UseHttpsRedirection();

app.UseAuthorization();

app.MapControllers();

app.Run();
