using App.Infrastructure;
using Microsoft.EntityFrameworkCore;

namespace App.Models;

public class Users
{
  private readonly UserContext _userContext;

  public Users(UserContext userContext)
  {
    _userContext = userContext;
  }

  public virtual async Task<User> Create(CreateUserDto payload)
  {
    var user = new User
    {
      Name = payload.Name
    };

    _userContext.Users.Add(user);

    await _userContext.SaveChangesAsync().ConfigureAwait(false);

    return user;
  }

  public virtual Task<User?> FindOne(int id) => _userContext.Users.FindAsync(id).AsTask();

  public virtual async Task Remove(int id)
  {
    var user = _userContext.Users.SingleOrDefault(x => x.Id == id);

    if (user is not null)
    {
      _userContext.Users.Remove(user);
      await _userContext.SaveChangesAsync().ConfigureAwait(false);
    }
  }

  /// <summary>
  /// This method performs a simple SQL query that would typically search through the users table and retrieve
  /// the user who has a concrete ID. However, an attacker can easily use the lack of validation from
  /// user inputs to read sensitive data from the database, modify database data, or
  /// execute administration operations by inputting values that the developer did not consider a valid (e.g. `1 OR 2028=2028` or `1; DROP user--`)
  ///
  /// Using the built-in `Where` method that escapes the input passed to it automatically before it is inserted into the query,
  /// you can fix the actual issue:
  /// ```csharp
  /// public Task<List<User>> FindByName(string name)
  /// {
  ///   return _userContext.Users.Where(u => u.Name.Contains(name)).ToListAsync();
  /// }
  /// </summary>
  public virtual Task<List<User>> FindByName(string name)
  {
    var query = $@"select * from ""Users"" where ""Name"" ilike '{name}%'";

    return _userContext.Users.FromSqlRaw(query).ToListAsync();
  }
}
