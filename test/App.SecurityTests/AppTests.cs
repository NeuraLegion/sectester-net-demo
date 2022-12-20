namespace App.SecurityTests;

public class AppTests : IClassFixture<AppFixture>, IAsyncLifetime
{
  private readonly AppFixture _fixture;
  private SecRunner _runner;

  public AppTests(AppFixture fixture)
  {
    _fixture = fixture;
  }

  public async Task InitializeAsync()
  {
    // Loading environment variables from .env file using https://github.com/tonerdo/dotnet-env
    Env.NoClobber().TraversePath().Load();

    var hostname = Environment.GetEnvironmentVariable("BRIGHT_HOSTNAME")!;
    var config = new Configuration(hostname);

    _runner = await SecRunner.Create(config);

    await _runner.Init();
  }

  public async Task DisposeAsync()
  {
    await _runner.DisposeAsync();
    await _fixture.DisposeAsync();
    GC.SuppressFinalize(this);
  }

  [Fact]
  public async Task Post_Users_ShouldNotHaveXss()
  {
    var content = JsonContent.Create(new { Name = "Test" },
      options: new JsonSerializerOptions { PropertyNamingPolicy = JsonNamingPolicy.CamelCase });

    var target = new Target($"{_fixture.Url}/Users")
      .WithMethod(HttpMethod.Post)
      .WithBody(content);

    var builder = new ScanSettingsBuilder()
      .WithName(nameof(Post_Users_ShouldNotHaveXss))
      .WithAttackParamLocations(new List<AttackParamLocation>
      {
        AttackParamLocation.Body
      })
      .WithTests(new List<TestType> { TestType.Xss });

    await _runner
      .CreateScan(builder)
      .Threshold(Severity.Medium)
      .Run(target);
  }

  [Fact]
  public async Task Get_Users_ShouldNotHaveSqli()
  {
    var target = new Target($"{_fixture.Url}/Users")
      .WithMethod(HttpMethod.Get)
      .WithQuery(new Dictionary<string, string> { { "name", "Test" } });

    var builder = new ScanSettingsBuilder()
      .WithName(nameof(Get_Users_ShouldNotHaveSqli))
      .WithAttackParamLocations(new List<AttackParamLocation>
      {
        AttackParamLocation.Query
      })
      .WithTests(new List<TestType> { TestType.Sqli });

    await _runner
      .CreateScan(builder)
      .Threshold(Severity.Medium)
      .Run(target);
  }
}
