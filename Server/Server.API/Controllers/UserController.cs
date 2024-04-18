using Microsoft.AspNetCore.Mvc;
using Server.API.SerialzationLib;
using Server.API.DTO;
using AutoMapper;

namespace Server.API.Controllers;

[Route("api/[controller]")]
[ApiController]
//[Authorize]
public class UserController : Controller
{
    private readonly MySerializer<List<ChatDTO>> _chatsSerializer;
    private readonly MySerializer<List<MessageDTO>> _messagesSerializer;

    private readonly IMediator _mediator;
    private readonly IMapper _mapper;

    public UserController(IMediator mediator, IMapper mapper)
    {
        _mediator = mediator;
        _mapper = mapper;

        _chatsSerializer = MySerializer<List<ChatDTO>>.GetInstance();
        _messagesSerializer = MySerializer<List<MessageDTO>>.GetInstance();
    }

    [HttpPut("update/id={id:int}&name={name}&login={login}&password={password}")]
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
        var chatsDto = _mapper.Map<IEnumerable<ChatDTO>>(chats);
        //return Ok(chatsDto);
        return Ok(_chatsSerializer.SerializeJson(chatsDto.ToList()));
    }

    [HttpGet("chatMessages/userId={userId:int}&chatId={chatId:int}")]
    public async Task<IActionResult> GetUserChatMessages(int userId, int chatId)
    {
        var messages = await _mediator.Send(new GetUserChatMessagesRequest(userId, chatId));
        var messagesDto = _mapper.Map<IEnumerable<MessageDTO>>(messages);
        //return Ok(messagesDto);
        return Ok(_messagesSerializer.SerializeJson(messagesDto.ToList()));
    }
}
