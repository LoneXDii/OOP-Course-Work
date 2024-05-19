using AutoMapper;
using Microsoft.AspNetCore.Mvc;
using Server.Infrastructure.Authentification;
using Server.API.DTO;

namespace Server.API.Controllers;

[Route("api/[controller]")]
[ApiController]
public class AuthController : ControllerBase
{
    private readonly IMediator _mediator;
    private readonly IMapper _mapper;
    private readonly IJwtTokenGenerator _jwtTokenGenerator;
    private readonly ILogger _logger;

    public AuthController(IMediator mediator, IMapper mapper, IJwtTokenGenerator jwtTokenGenerator,
                          ILogger<AuthController> logger)
    {
        _mediator = mediator;
        _mapper = mapper;
        _jwtTokenGenerator = jwtTokenGenerator;
        _logger = logger;
    }

    [HttpPost("register")]
    public async Task<IActionResult> Register(UserWithLoginAndPasswordDTO request)
    {
        _logger.LogInformation($"Processing route: {Request.Path.Value}");
        var user = _mapper.Map<User>(request);
        user = await _mediator.Send(new AddUserRequest(user));
        user.AuthorizationToken = _jwtTokenGenerator.CreateToken(user.Id, user.Login, user.Password);
        try
        {
            var userDto = _mapper.Map<UserWithTokenDTO>(user);
            _logger.LogInformation($"{Request.Path.Value}: 200 OK");
            return Ok(userDto);
        }
        catch (Exception ex)
        {
            _logger.LogError($"{Request.Path.Value}: 400 Bad Request");
            return BadRequest(ex.Message);
        }
    }

    [HttpGet("authorize/login={login}&password={password}")]
    public async Task<IActionResult> Authorize(string login, string password)
    {
        _logger.LogInformation($"Processing route: {Request.Path.Value}");
        var user = await _mediator.Send(new AuthorizeUserRequest(login, password));
        if (user is null)
        {
            _logger.LogError($"{Request.Path.Value}: 404 Not Found");
            return NotFound();
        }
        user.AuthorizationToken = _jwtTokenGenerator.CreateToken(user.Id, user.Login, user.Password);
        var userDto = _mapper.Map<UserWithTokenDTO>(user);
        _logger.LogInformation($"{Request.Path.Value}: 200 OK");
        return Ok(userDto);
    }
}
