using System.Net;
using App.Models;
using Microsoft.AspNetCore.Mvc;

namespace App.Controllers;

[ApiController]
[Route("[controller]")]
public class UsersController : ControllerBase
{
  private readonly Users _users;

  public UsersController(Users users)
  {
    _users = users;
  }

  [HttpPost]
  [ProducesResponseType(typeof(User), (int)HttpStatusCode.Created)]
  public Task<User> Create([FromBody] CreateUserDto createUserDto) => _users.Create(createUserDto);

  [HttpGet]
  [ProducesResponseType(typeof(IEnumerable<User>), (int)HttpStatusCode.OK)]
  public Task<List<User>> FindByName([FromQuery] string name) => _users.FindByName(name);

  [HttpGet]
  [Route("{id}")]
  [ProducesResponseType(typeof(User), (int)HttpStatusCode.OK)]
  public Task<User?> FindOne(int id) => _users.FindOne(id);

  [HttpDelete]
  [Route("{id}")]
  [ProducesResponseType((int)HttpStatusCode.NoContent)]
  public Task Remove(int id) => _users.Remove(id);
}
