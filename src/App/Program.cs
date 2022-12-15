using App.Infrastructure;
using App.Models;
using DotNetEnv;
using Microsoft.OpenApi.Models;

Env.NoClobber().TraversePath().Load();

var builder = WebApplication.CreateBuilder(args);

// Add services to the container.
builder.Services.AddControllers();
builder.Services.AddScoped<Users>();
builder.Services.AddDbContext<UserContext>();
// Learn more about configuring Swagger/OpenAPI at https://aka.ms/aspnetcore/swashbuckle
builder.Services.AddEndpointsApiExplorer();
builder.Services.AddSwaggerGen(options =>
{
  options.SwaggerDoc("v1", new OpenApiInfo
  {
    Version = "1.0",
    Title = "SecTester JS Demo",
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
    options.DocumentTitle = "SecTester JS Demo";
  });
}

app.UseAuthorization();
app.MapControllers();

app.Run();
