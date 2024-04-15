using Microsoft.AspNetCore.Authorization;
using Microsoft.AspNetCore.Mvc;

namespace Server.API.Controllers;

[Route("api/[controller]")]
[ApiController]
public class DebugController : Controller
{
    private readonly IMediator _mediator;

    public DebugController(IMediator mediator)
    {
        _mediator = mediator;
    }

    [HttpGet("allusers")]
    public async Task<IActionResult> GetUsers()
    {
        var users = await _mediator.Send(new GetAllUsersRequest());
        return Ok(users);
    }

    [HttpGet("allchats")]
    public async Task<IActionResult> GetChats()
    {
        var chats = await _mediator.Send(new GetAllChatsRequest());
        return Ok(chats);
    }

    [HttpGet("allmessages")]
    public async Task<IActionResult> GetMessages()
    {
        var messages = await _mediator.Send(new GetAllMessagesRequest());
        return Ok(messages);
    }
}
