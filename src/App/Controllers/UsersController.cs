using App.Models;
using Microsoft.AspNetCore.Mvc;

namespace App.Controllers;

[ApiController]
[Route("[controller]")]
public class UsersController : ControllerBase
{
  private readonly IUsers _users;

  public UsersController(IUsers users)
  {
    _users = users;
  }

  [HttpPost]
  [ProducesResponseType(typeof(User), StatusCodes.Status201Created)]
  public async Task<IActionResult> Create([FromBody] CreateUserDto createUserDto)
  {
    var user = await _users.Create(createUserDto).ConfigureAwait(false);

    return CreatedAtAction(nameof(FindOne), new { id = user.Id }, user);
  }

  [HttpGet]
  [ProducesResponseType(typeof(List<User>), StatusCodes.Status200OK)]
  public Task<List<User>> FindByName([FromQuery] string name) => _users.FindByName(name);

  [HttpGet]
  [Route("{id}")]
  [ProducesResponseType(typeof(User), StatusCodes.Status200OK)]
  [ProducesResponseType(StatusCodes.Status404NotFound)]
  public async Task<ActionResult<User>> FindOne(int id)
  {
    var user = await _users.FindOne(id).ConfigureAwait(false);

    return user == null ? NotFound() : user;

  }

  [HttpDelete]
  [Route("{id}")]
  [ProducesResponseType(StatusCodes.Status204NoContent)]
  public Task Remove(int id) => _users.Remove(id);
}
