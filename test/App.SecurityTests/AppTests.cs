namespace App.SecurityTests;

public class AppTests : IClassFixture<AppFixture>, IAsyncLifetime
{
  private static readonly IEnumerable<KeyValuePair<string, string>> _ = Env.NoClobber().TraversePath().Load();
  private static readonly string Hostname = Environment.GetEnvironmentVariable("BRIGHT_HOSTNAME")!;
  private static readonly string Token = Environment.GetEnvironmentVariable("BRIGHT_TOKEN")!;
  private readonly Configuration _config = new(Hostname, new Credentials(Token));
  private readonly AppFixture _fixture;
  private readonly SecRunner _runner;

  public AppTests(AppFixture fixture)
  {
    _fixture = fixture;
    _runner = SecRunner.Create(_config);
  }

  public async Task InitializeAsync() => await _runner.Init();

  public async Task DisposeAsync()
  {
    await _runner.DisposeAsync();
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

