using AutoMapper;
using Microsoft.AspNetCore.Mvc;
using Server.API.SerialzationLib;
using Server.Infrastructure.Authentification;
using Server.API.DTO;

namespace Server.API.Controllers;

[Route("api/[controller]")]
[ApiController]
public class AuthController : ControllerBase
{
    private readonly MySerializer<UserDTO> _userSerializer;

    private readonly IMediator _mediator;
    private readonly IMapper _mapper;
    private readonly IJwtTokenGenerator _jwtTokenGenerator;

    public AuthController(IMediator mediator, IMapper mapper, IJwtTokenGenerator jwtTokenGenerator)
    {
        _mediator = mediator;
        _mapper = mapper;
        _jwtTokenGenerator = jwtTokenGenerator;

        _userSerializer = MySerializer<UserDTO>.GetInstance();
    }

    [HttpPost("register")]
    public async Task<IActionResult> Register(UserDTO RequestUser)
    {
        var user = _mapper.Map<User>(RequestUser);
        user = await _mediator.Send(new AddUserRequest(user));
        user.AuthorizationToken = _jwtTokenGenerator.CreateToken(user.Id, user.Login, user.Password);
        var userDto = _mapper.Map<UserDTO>(user);
        return Ok(userDto);
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
        var userDto = _mapper.Map<UserDTO>(user);
        return Ok(userDto);
    }
}
