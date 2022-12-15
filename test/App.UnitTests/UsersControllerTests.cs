namespace App.UnitTests;

public class UsersControllerTests : IDisposable
{
  private const string UserName = "username";
  private const int UserId = 1;

  private readonly UsersController _sut;

  private readonly User _user = new()
  {
    Id = UserId,
    Name = UserName,
    IsActive = true
  };

  private readonly IUsers _users = Substitute.For<IUsers>();

  public UsersControllerTests()
  {
    _sut = new UsersController(_users);
  }

  public void Dispose()
  {
    _users.ClearSubstitute();

    GC.SuppressFinalize(this);
  }

  [Fact]
  public async Task Create_ReturnsUser()
  {
    // arrange
    var input = new CreateUserDto { Name = UserName };

    _users.Create(input).Returns(_user);

    // act
    var result = await _sut.Create(input);

    // assert
    result.Should().BeEquivalentTo(new
    {
      Id = UserId,
      Name = UserName
    });
  }

  [Fact]
  public async Task FindByName_ReturnsUsers()
  {
    // arrange
    _users.FindByName(UserName).Returns(new List<User> { _user });

    // act
    var result = await _sut.FindByName(UserName);

    // assert
    result.Should().BeEquivalentTo(new List<User> { _user });
  }

  [Fact]
  public async Task FindOne_ReturnsUser()
  {
    // arrange
    _users.FindOne(UserId).Returns(_user);

    // act
    var result = await _sut.FindOne(UserId);

    // assert
    result.Should().BeEquivalentTo(new
    {
      Id = UserId,
      Name = UserName
    });
  }

  [Fact]
  public async Task Remove_RemovesUser()
  {
    // act
    await _sut.Remove(UserId);

    // assert
    await _users.Received(1).Remove(UserId);
  }
}
