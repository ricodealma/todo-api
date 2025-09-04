using Microsoft.OpenApi.Models;
using Todo.Api.App.Extensions;

var builder = WebApplication.CreateBuilder(args);


builder.Services.AddSwaggerGen();
builder.Services.AddEndpointsApiExplorer();
builder.Services.AddCustomServices(builder.Configuration);
builder.Services.AddHttpContextAccessor();

builder.Services.AddSwaggerGen(options =>
{
    options.EnableAnnotations();
    options.SwaggerDoc("v1", new OpenApiInfo
    {
        Version = "v1",
        Title = "To do Microservice",
        Description = "Microservice responsible for persisting todos",
    });
});


var app = builder.Build();

app.UseSwagger();
app.UseSwaggerUI(config =>
{
    config.SwaggerEndpoint("/swagger/v1/swagger.json", "To do Microservice");
    config.RoutePrefix = string.Empty;
});
app.UseHttpsRedirection();

app.FillEnvironmentVariables(app.Configuration);
app.AddEndpoints();
app.Run();