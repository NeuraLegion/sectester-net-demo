namespace App.UnitTests;

public class UsersTests : IAsyncLifetime
{
  private readonly UserContext _context;

  private readonly Users _sut;
  private readonly User _user = new()
  {
    Id = 1,
    Name = "Bob",
    IsActive = true
  };

  public UsersTests()
  {
    var options = new DbContextOptionsBuilder<UserContext>()
      .UseInMemoryDatabase(Guid.NewGuid().ToString())
      .Options;
    _context = Create.MockedDbContextFor<UserContext>(options);
    _sut = new Users(_context);
  }

  public async Task InitializeAsync()
  {
    _context.Set<User>().AddRange(new List<User> { _user });

    await _context.SaveChangesAsync();
  }

  public async Task DisposeAsync()
  {
    _context.ClearSubstitute();
    await _context.DisposeAsync();
    GC.SuppressFinalize(this);
  }

  [Fact]
  public async Task Create_ReturnsUser()
  {
    // arrange
    const string name = "Charlie";
    var input = new CreateUserDto { Name = name };

    // act
    var result = await _sut.Create(input);

    // assert
    result.Should().BeEquivalentTo(new
    {
      Name = name
    });
  }

  [Fact]
  public async Task FindByName_ReturnsUsers()
  {
    // arrange
    var expected = new List<User> { _user };
    var query = $@"select * from ""Users"" where ""Name"" ilike '{_user.Name}%'";
    _context.Set<User>().AddFromSqlRawResult(expected);

    // act
    var result = await _sut.FindByName(_user.Name);

    // assert
    _context.Users.Received().FromSqlRaw(query);
    result.Should().BeEquivalentTo(expected);
  }

  [Fact]
  public async Task FindOne_ReturnsUser()
  {
    // act
    var result = await _sut.FindOne(_user.Id);

    // assert
    result.Should().BeEquivalentTo(_user);
  }

  [Fact]
  public async Task Remove_RemovesUser()
  {
    // act
    await _sut.Remove(_user.Id);

    // assert
    _context.Users.Received().Remove(_user);
  }
}
