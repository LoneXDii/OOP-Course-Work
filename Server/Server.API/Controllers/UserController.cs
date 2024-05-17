using Microsoft.AspNetCore.Mvc;
using Microsoft.AspNetCore.Authorization;
using Server.API.DTO;
using AutoMapper;

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

    [HttpPut("update")]
    public async Task<IActionResult> UpdateUser(KeyValuePair<string, UserDTO> request)
    {
        _logger.LogInformation($"Processing route: {Request.Path.Value}");
        var user = await _mediator.Send(new GetUserByIdRequest(request.Value.Id));
        if (user is null)
        {
            return NotFound();
        }
        user.Password = request.Key;
        user.Name = request.Value.Name;
        await _mediator.Send(new UpdateUserRequest(user));
        var userDto = _mapper.Map<UserDTO>(user);
        return Ok(userDto);
    }

    [HttpDelete("delete/id={id:int}")]
    public async Task<IActionResult> DeleteUser(int id)
    {
        _logger.LogInformation($"Processing route: {Request.Path.Value}");
        await _mediator.Send(new DeleteUserRequest(id));
        return Ok();
    }

    [HttpGet("chats/userId={id:int}")]
    public async Task<IActionResult> GetUserChats(int id)
    {
        _logger.LogInformation($"Processing route: {Request.Path.Value}");
        var chats = await _mediator.Send(new GetUserChatsRequest(id));
        var chatsDto = _mapper.Map<IEnumerable<ChatDTO>>(chats);
        return Ok(chatsDto);
    }

    [HttpGet("chatMessages/userId={userId:int}&chatId={chatId:int}")]
    public async Task<IActionResult> GetUserChatMessages(int userId, int chatId)
    {
        _logger.LogInformation($"Processing route: {Request.Path.Value}");
        var messages = await _mediator.Send(new GetUserChatMessagesRequest(userId, chatId));
        var messagesDto = _mapper.Map<IEnumerable<MessageDTO>>(messages);
        return Ok(messagesDto);
    }

    [HttpGet("allUsers")]
    public async Task<IActionResult> GetAllUsersRequest()
    {
        _logger.LogInformation($"Processing route: {Request.Path.Value}");
        var users = await _mediator.Send(new GetAllUsersRequest());
        var usersDto = _mapper.Map<IEnumerable<UserDTO>>(users);
        return Ok(usersDto);
    }
}
