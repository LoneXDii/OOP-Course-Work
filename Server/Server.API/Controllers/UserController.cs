using Microsoft.AspNetCore.Authorization;
using Microsoft.AspNetCore.Http;
using Microsoft.AspNetCore.Mvc;
using Server.API.SerialzationLib;

namespace Server.API.Controllers;

[Route("api/[controller]")]
[ApiController]
//[Authorize]
public class UserController : Controller
{
    private readonly MySerializer<List<Chat>> _chatsSerializer;
    private readonly MySerializer<List<Message>> _messagesSerializer;

    private readonly IMediator _mediator;

    public UserController(IMediator mediator)
    {
        _mediator = mediator;

        _chatsSerializer = MySerializer<List<Chat>>.GetInstance();
        _messagesSerializer = MySerializer<List<Message>>.GetInstance();
    }

    [HttpPost("update/id={id:int}&name={name}&login={login}&password={password}")]
    public async Task<IActionResult> UpdateUser(int id, string name, string login, string password)
    {
        var user = await _mediator.Send(new GetUserByIdRequest(id));
        if (user is null)
        {
            return NotFound();
        }
        user.Login = login;
        user.Password = password;
        user.Name = name;
        await _mediator.Send(new UpdateUserRequest(user));
        return Ok();
    }

    [HttpDelete("delete/id={id:int}")]
    public async Task<IActionResult> DeleteUser(int id)
    {
        await _mediator.Send(new DeleteUserRequest(id));
        return Ok();
    }

    [HttpGet("chats/userId={id:int}")]
    public async Task<IActionResult> GetUserChats(int id)
    {
        var chats = await _mediator.Send(new GetUserChatsRequest(id));
        //return Ok(chats);
        return Ok(_chatsSerializer.SerializeJson(chats.ToList()));
    }

    [HttpGet("chatMessages/userId={userId:int}&chatId={chatId:int}")]
    public async Task<IActionResult> GetUserChatMessages(int userId, int chatId)
    {
        var messages = await _mediator.Send(new GetUserChatMessagesRequest(userId, chatId));
        //return Ok(messages);
        return Ok(_messagesSerializer.SerializeJson(messages.ToList()));
    }
}
