using AutoMapper;
using Microsoft.AspNetCore.Authorization;
using Microsoft.AspNetCore.Mvc;
using Server.API.DTO;

namespace Server.API.Controllers;

[Route("api/[controller]")]
[ApiController]
public class DebugController : Controller
{
    private readonly IMediator _mediator;
    private readonly IMapper _mapper;

    public DebugController(IMediator mediator, IMapper mapper)
    {
        _mediator = mediator;
        _mapper = mapper;
    }

    [HttpGet("allusers")]
    public async Task<IActionResult> GetUsers()
    {
        var users = await _mediator.Send(new GetAllUsersRequest());
        var usersDto = _mapper.Map<IEnumerable<UserDTO>>(users);
        return Ok(usersDto);
    }

    [HttpGet("allchats")]
    public async Task<IActionResult> GetChats()
    {
        var chats = await _mediator.Send(new GetAllChatsRequest());
        var chatsDTO = _mapper.Map<IEnumerable<ChatDTO>>(chats);
        return Ok(chatsDTO);
    }

    [HttpGet("allmessages")]
    public async Task<IActionResult> GetMessages()
    {
        var messages = await _mediator.Send(new GetAllMessagesRequest());
        var messagesDTO = _mapper.Map<IEnumerable<MessageDTO>>(messages);
        return Ok(messagesDTO);
    }
}
