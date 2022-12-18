namespace App.FunctionalTests;

public class AppFixture: IAsyncLifetime
{
  private readonly WebApplicationFactory<Program> _webApplicationFactory;

  private readonly TestcontainerDatabase _postgresqlContainer =
    new TestcontainersBuilder<PostgreSqlTestcontainer>()
      .WithImage("postgres:14-alpine")
      .WithDatabase(new PostgreSqlTestcontainerConfiguration
      {
        Database = $"db_{Guid.NewGuid()}",
        Username = "postgres",
        Password = "postgres",
      })
      .Build();

  public HttpClient Client => _webApplicationFactory.CreateClient();

  public AppFixture()
  {
    _webApplicationFactory = new WebApplicationFactory<Program>()
      .WithWebHostBuilder(builder => builder.UseSolutionRelativeContentRoot("")
          .ConfigureAppConfiguration(cb => cb.AddJsonFile("appsettings.json", true).AddEnvironmentVariables())
          .ConfigureLogging(loggingBuilder => loggingBuilder.ClearProviders())
          .ConfigureTestServices(services =>
          {
            var descriptor = services.SingleOrDefault(
              d => d.ServiceType ==
                   typeof(DbContextOptions<UserContext>));

            if (descriptor is not null)
            {
              services.Remove(descriptor);
            }

            services.AddDbContext<IUserContext, UserContext>(options => options.UseNpgsql(
              _postgresqlContainer.ConnectionString)
            );
          })
      );
  }

  public async Task InitializeAsync()
  {
    await _postgresqlContainer.StartAsync();
    _webApplicationFactory.Services.MigrateDbContext<UserContext>((context, services) => RunSeeds(services, context));
  }

  public async Task DisposeAsync()
  {
    await _webApplicationFactory.DisposeAsync();
    await _postgresqlContainer.DisposeAsync();
  }

  private static void RunSeeds(IServiceProvider services, UserContext context)
  {
    var env = services.GetRequiredService<IWebHostEnvironment>();
    var logger = services.GetRequiredService<ILogger<UserContextSeed>>();

    new UserContextSeed()
      .SeedAsync(context, env, logger)
      .Wait();
  }
}
