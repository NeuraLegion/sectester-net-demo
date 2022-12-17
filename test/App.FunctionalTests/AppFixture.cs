namespace App.FunctionalTests;

public class AppFixture : WebApplicationFactory<Program>
{
  public HttpClient Client => CreateClient();

  protected override void ConfigureWebHost(IWebHostBuilder builder) =>
    builder.ConfigureAppConfiguration(cb => cb.AddJsonFile("appsettings.json", true)
        .AddEnvironmentVariables())
      .ConfigureLogging(loggingBuilder => loggingBuilder.ClearProviders())
      .UseSolutionRelativeContentRoot("");


  protected override IHost CreateHost(IHostBuilder builder)
  {
    var host = builder.Build();
    host.Services.MigrateDbContext<UserContext>((context, services) => RunSeeds(services, context));
    host.Start();
    return host;
  }

  private static void RunSeeds(IServiceProvider services, UserContext context)
  {
    var logger = services.GetRequiredService<ILogger<UserContextSeed>>();

    new UserContextSeed()
      .SeedAsync(context, logger)
      .Wait();
  }
}
