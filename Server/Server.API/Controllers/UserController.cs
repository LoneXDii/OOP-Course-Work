using Microsoft.AspNetCore.Mvc;
using Microsoft.AspNetCore.Authorization;
using Server.API.DTO;
using AutoMapper;
using System.Security.Claims;

namespace Server.API.Controllers;

[Route("api/[controller]")]
[ApiController]
[Authorize]
public class UserController : Controller
{
    private readonly IMediator _mediator;
    private readonly IMapper _mapper;
    private readonly ILogger _logger;

    public UserController(IMediator mediator, IMapper mapper, ILogger<UserController> logger)
    {
        _mediator = mediator;
        _mapper = mapper;
        _logger = logger;
    }

    [HttpPut("updateName")]
    public async Task<IActionResult> UpdateUser(UserDTO request)
    {
        _logger.LogInformation($"Processing route: {Request.Path.Value}");

        var userId = Convert.ToInt32(HttpContext.User.FindFirstValue("Id"));
        if (userId != request.Id)
        {
            _logger.LogError($"{Request.Path.Value}: 403 Forbidden");
            return Forbid();
        }

        var user = await _mediator.Send(new GetUserByIdRequest(request.Id));
        if (user is null)
        {
            _logger.LogError($"{Request.Path.Value}: 404 Not Found");
            return NotFound();
        }
        user.Name = request.Name;
        await _mediator.Send(new UpdateUserRequest(user));
        var userDto = _mapper.Map<UserDTO>(user);
        _logger.LogInformation($"{Request.Path.Value}: 200 OK");
        return Ok(userDto);
    }

    [HttpPut("updatePassword")]
    public async Task<IActionResult> UpdateUserPassword(ChangePasswordDTO request)
    {
        _logger.LogInformation($"Processing route: {Request.Path.Value}");

        var userId = Convert.ToInt32(HttpContext.User.FindFirstValue("Id"));
        if (userId != request.Id)
        {
            _logger.LogError($"{Request.Path.Value}: 403 Forbidden");
            return Forbid();
        }

        var user = await _mediator.Send(new GetUserByIdRequest(request.Id));
        if (user is null)
        {
            _logger.LogError($"{Request.Path.Value}: 404 Not Found");
            return NotFound();
        }
        if (user.Password != request.OldPassword)
        {
            _logger.LogError($"{Request.Path.Value}: 400 Bad Request");
            return BadRequest();
        }
        user.Password = request.NewPassword;
        await _mediator.Send(new UpdateUserRequest(user));
        _logger.LogInformation($"{Request.Path.Value}: 200 OK");
        return Ok();
    }

    [HttpDelete("delete/id={id:int}")]
    public async Task<IActionResult> DeleteUser(int id)
    {
        _logger.LogInformation($"Processing route: {Request.Path.Value}");

        var userId = Convert.ToInt32(HttpContext.User.FindFirstValue("Id"));
        if (userId != id)
        {
            _logger.LogError($"{Request.Path.Value}: 403 Forbidden");
            return Forbid();
        }

        await _mediator.Send(new DeleteUserRequest(id));
        _logger.LogInformation($"{Request.Path.Value}: 200 OK");
        return Ok();
    }

    [HttpGet("chats/userId={id:int}")]
    public async Task<IActionResult> GetUserChats(int id)
    {
        _logger.LogInformation($"Processing route: {Request.Path.Value}");

        var userId = Convert.ToInt32(HttpContext.User.FindFirstValue("Id"));
        if (userId != id)
        {
            _logger.LogError($"{Request.Path.Value}: 403 Forbidden");
            return Forbid();
        }

        var chats = await _mediator.Send(new GetUserChatsRequest(id));
        var chatsDto = _mapper.Map<IEnumerable<ChatDTO>>(chats);
        _logger.LogInformation($"{Request.Path.Value}: 200 OK");
        return Ok(chatsDto);
    }

    //[HttpGet("chatMessages/userId={userId:int}&chatId={chatId:int}")]
    //public async Task<IActionResult> GetUserChatMessages(int userId, int chatId)
    //{
    //    _logger.LogInformation($"Processing route: {Request.Path.Value}");
    //    var messages = await _mediator.Send(new GetUserChatMessagesRequest(userId, chatId));
    //    var messagesDto = _mapper.Map<IEnumerable<MessageDTO>>(messages);
    //    return Ok(messagesDto);
    //}

    [HttpGet("allUsers")]
    public async Task<IActionResult> GetAllUsersRequest()
    {
        _logger.LogInformation($"Processing route: {Request.Path.Value}");
        var users = await _mediator.Send(new GetAllUsersRequest());
        var usersDto = _mapper.Map<IEnumerable<UserDTO>>(users);
        _logger.LogInformation($"{Request.Path.Value}: 200 OK");
        return Ok(usersDto);
    }
}
