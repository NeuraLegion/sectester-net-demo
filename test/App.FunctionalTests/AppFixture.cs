namespace App.FunctionalTests;

public class AppFixture : IDisposable
{
  private readonly WebApplicationFactory<Program> _app;

  public AppFixture()
  {
    var app = new WebApplicationFactory<Program>()
      .WithWebHostBuilder(ConfigureBuilder);

    app.Services
      .MigrateDbContext<UserContext>((context, services) => RunSeeds(services, context));

    _app = app;
  }

  public HttpClient Client => _app.CreateClient();

  public void Dispose()
  {
    _app.Dispose();
    GC.SuppressFinalize(this);
  }

  private static void RunSeeds(IServiceProvider services, UserContext context)
  {
    var env = services.GetRequiredService<IWebHostEnvironment>();
    var logger = services.GetRequiredService<ILogger<UserContextSeed>>();

    new UserContextSeed()
      .SeedAsync(context, env, logger)
      .Wait();
  }

  private static void ConfigureBuilder(IWebHostBuilder builder) =>
    builder.ConfigureAppConfiguration(cb =>
      {
        cb.AddJsonFile("appsettings.json", true)
          .AddEnvironmentVariables();
      })
      .ConfigureLogging(logging => logging.ClearProviders())
      .UseSolutionRelativeContentRoot("");
}
