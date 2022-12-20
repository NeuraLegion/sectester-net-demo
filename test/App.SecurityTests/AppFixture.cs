namespace App.SecurityTests;

// Based on https://github.com/martincostello/dotnet-minimal-api-integration-testing.
// For details see https://github.com/dotnet/aspnetcore/issues/4892
public class AppFixture : WebApplicationFactory<Program>
{
  private readonly Lazy<Uri> _urlInitializer;
  private bool _disposed;
  private IHost _host;

  public AppFixture()
  {
    _urlInitializer = new Lazy<Uri>(GetUrl);
  }

  public Uri Url => _urlInitializer.Value;

  protected override void Dispose(bool disposing)
  {
    base.Dispose(disposing);

    if (!_disposed)
    {
      if (disposing)
      {
        _host?.Dispose();
      }

      _disposed = true;
    }

    GC.SuppressFinalize(this);
  }

  private Uri GetUrl()
  {
    EnsureServer();
    return ClientOptions.BaseAddress;
  }

  protected override void ConfigureWebHost(IWebHostBuilder builder) =>
    builder.ConfigureAppConfiguration(cb => cb.AddJsonFile("appsettings.json", true)
        .AddEnvironmentVariables())
      .ConfigureLogging(loggingBuilder => loggingBuilder.ClearProviders())
      .UseSolutionRelativeContentRoot("")
      .UseKestrel()
      .UseUrls("http://127.0.0.1:0");

  protected override IHost CreateHost(IHostBuilder builder)
  {
    // Create the host for TestServer now before we
    // modify the builder to use Kestrel instead.
    var testHost = builder.Build();

    // Modify the host builder to use Kestrel instead
    // of TestServer so we can listen on a real address.
    builder.ConfigureWebHost(webHostBuilder => webHostBuilder.UseKestrel());

    // Create and start the Kestrel server before the test server,
    // otherwise due to the way the deferred host builder works
    // for minimal hosting, the server will not get "initialized
    // enough" for the address it is listening on to be available.
    // See https://github.com/dotnet/aspnetcore/issues/33846.
    _host = builder.Build();

    // Seed DB to prepare for further testing
    _host.Services.MigrateDbContext<UserContext>((context, services) => RunSeeds(services, context));

    _host.Start();

    // Extract the selected dynamic port out of the Kestrel server
    // and assign it onto the client options for convenience so it
    // "just works" as otherwise it'll be the default http://localhost
    // URL, which won't route to the Kestrel-hosted HTTP server.
    var server = _host.Services.GetRequiredService<IServer>();
    var addresses = server.Features.Get<IServerAddressesFeature>();

    ClientOptions.BaseAddress = addresses!.Addresses
      .Select(x => new Uri(x))
      .Last();

    // Return the host that uses TestServer, rather than the real one.
    // Otherwise the internals will complain about the host's server
    // not being an instance of the concrete type TestServer.
    // See https://github.com/dotnet/aspnetcore/pull/34702.
    testHost.Start();
    return testHost;
  }

  private static void RunSeeds(IServiceProvider services, UserContext context)
  {
    var logger = services.GetRequiredService<ILogger<UserContextSeed>>();

    new UserContextSeed()
      .SeedAsync(context, logger)
      .Wait();
  }

  private void EnsureServer()
  {
    if (_host is null)
    {
      // This forces WebApplicationFactory to bootstrap the server
      using var _ = CreateDefaultClient();
    }
  }
}
