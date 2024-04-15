using Microsoft.AspNetCore.Mvc;
using Server.Infrastructure.Authentification;

namespace Server.API.Controllers;

[Route("api/[controller]")]
[ApiController]
public class AuthController : ControllerBase
{
    private readonly IMediator _mediator;
    private readonly IJwtTokenGenerator _jwtTokenGenerator;

    public AuthController(IMediator mediator, IJwtTokenGenerator jwtTokenGenerator)
    {
        _mediator = mediator;
        _jwtTokenGenerator = jwtTokenGenerator;
    }

    [HttpPost("register/name={name}&login={login}&password={password}")]
    public async Task<IActionResult> Register(string name, string login, string password)
    {
        var user = new User(name, login, password);
        user = await _mediator.Send(new AddUserRequest(user));
        user.AuthorizationToken = _jwtTokenGenerator.CreateToken(user.Id, user.Login, user.Password);
        return Ok(user);
    }

    [HttpGet("authorize/login={login}&password={password}")]
    public async Task<IActionResult> Authorize(string login, string password)
    {
        var user = await _mediator.Send(new AuthorizeUserRequest(login, password));
        if (user is null)
        {
            return NotFound();
        }
        user.AuthorizationToken = _jwtTokenGenerator.CreateToken(user.Id, user.Login, user.Password);
        return Ok(user);
    }
}
