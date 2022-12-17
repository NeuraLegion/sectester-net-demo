namespace App.FunctionalTests;

public class AppTests : IClassFixture<AppFixture>, IAsyncDisposable
{
  private readonly JsonSerializerOptions _serializerOptions = new() { PropertyNamingPolicy = JsonNamingPolicy.CamelCase };
  private readonly AppFixture _fixture;

  public AppTests(AppFixture fixture)
  {
    _fixture = fixture;
  }

  public async ValueTask DisposeAsync()
  {
    await _fixture.DisposeAsync();
    GC.SuppressFinalize(this);
  }

  [Fact]
  public async Task Post_Users_CreatesUser()
  {
    // arrange
    var user = new CreateUserDto
    {
      Name = "Oliver Logan"
    };

    // act
    using var res = await _fixture.Client.PostAsJsonAsync("/Users", user, _serializerOptions);

    // assert
    res.Should().BeEquivalentTo(new
    {
      StatusCode = HttpStatusCode.Created
    });
  }

  [Fact]
  public async Task Get_Users_ReturnsListOfUsers()
  {
    // arrange
    const string name = "Van Owen";

    // act
    var res = await _fixture.Client.GetFromJsonAsync<User[]>($"/Users?name={name}");

    // assert
    res.Should().ContainEquivalentOf(
      new { Name = name }
    );
  }

  [Fact]
  public async Task Get_Users_BooleanBasedBlindIsUsed_ReturnsListOfUsers()
  {
    // arrange
    const string name = "' OR '1858%'='1858";

    // act
    var res = await _fixture.Client.GetFromJsonAsync<User[]>($"/Users?name={name}");

    // assert
    res.Should().NotBeEmpty();
    res.Should().NotContainEquivalentOf(new
    {
      Name = name
    });
  }

  [Fact]
  public async Task Get_Users_ReturnsUserById()
  {
    // act
    var res = await _fixture.Client.GetFromJsonAsync<User>("/Users/1");

    // assert
    res.Should().BeOfType<User>();
  }

  [Fact]
  public async Task Delete_Users_RemovesUserById()
  {
    // act
    using var res = await _fixture.Client.DeleteAsync($"/Users/{int.MaxValue}");

    // assert
    res.Should().BeEquivalentTo(new
    {
      StatusCode = HttpStatusCode.OK
    });
  }
}
