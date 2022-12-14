using App.Infrastructure;
using App.Models;
using DotNetEnv;
using Microsoft.EntityFrameworkCore;
using Microsoft.OpenApi.Models;

Env.NoClobber().TraversePath().Load();

var builder = WebApplication.CreateBuilder(args);

// Add services to the container.
builder.Services.AddControllers();
builder.Services.AddScoped<IUsers, Users>();
builder.Services.AddDbContext<IUserContext, UserContext>(options => options.UseNpgsql(
  $"Host=localhost;Username={Environment.GetEnvironmentVariable("POSTGRES_USER")};Password={Environment.GetEnvironmentVariable("POSTGRES_PASSWORD")};Database=test")
);
// Learn more about configuring Swagger/OpenAPI at https://aka.ms/aspnetcore/swashbuckle
builder.Services.AddEndpointsApiExplorer();
builder.Services.AddSwaggerGen(options =>
{
  options.SwaggerDoc("v1", new OpenApiInfo
  {
    Version = "1.0",
    Title = "SecTester NET Demo",
    Description = "This is a demo project for the SecTester NET SDK framework, with some installation and usage examples."
  });
});

var app = builder.Build();

// Configure the HTTP request pipeline.
if (app.Environment.IsDevelopment())
{
  app.UseSwagger();
  app.UseSwaggerUI(options =>
  {
    options.DocumentTitle = "SecTester NET Demo";
  });
}

app.UseAuthorization();
app.MapControllers();

app.Run();

public partial class Program
{
  // Expose the Program class for use with WebApplicationFactory<T>
}
