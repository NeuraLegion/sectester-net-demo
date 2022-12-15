namespace App.Models;

public interface IUsers
{
  Task<User> Create(CreateUserDto payload);
  Task<User?> FindOne(int id);
  Task Remove(int id);
  Task<List<User>> FindByName(string name);
}
