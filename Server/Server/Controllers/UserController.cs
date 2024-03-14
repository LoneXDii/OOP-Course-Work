using Microsoft.AspNetCore.Mvc;
using Server.Data;
using Server.Models;

namespace Server.Controllers;

[ApiController]
[Route("api/[controller]")]
public class UserController : Controller
{
    private readonly IDBService _dbService;

    public UserController(IDBService db)
    {
        _dbService = db;
    }

    [HttpPost("adduser/login={login}&password={password}")]
    public IActionResult CreateUser(string login, string password)
    {
        var user = new User() { Login = login, Password = password, UserName = login };
        user = _dbService.AddUser(user);
        return Accepted(user);
    }

    [HttpGet("getuser/id={id:int}")]
    public IActionResult GetUser(int id)
    {
        var user = _dbService.GetUserById(id);
        if (user == null)
        {
            return BadRequest("No such user");
        }
        return Ok(user);
    }

    [HttpGet("getuser/name={name}")]
    public IActionResult GetUser(string name)
    {
        var user = _dbService.GetUserByName(name);
        if (user == null)
        {
            return BadRequest("No such user");
        }
        return Ok(user);
    }
}
