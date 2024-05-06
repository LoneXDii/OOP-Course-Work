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

    [HttpPut("update")]
    public async Task<IActionResult> UpdateUser(UserDTO requestUser)
    {
        var user = await _mediator.Send(new GetUserByIdRequest(requestUser.Id));
        if (user is null)
        {
            return NotFound();
        }
        user.Login = requestUser.Login;
        user.Password = requestUser.Password;
        user.Name = requestUser.Name;
        await _mediator.Send(new UpdateUserRequest(user));
        var userDto = _mapper.Map<UserDTO>(user);
        return Ok(userDto);
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
        return Ok(chatsDto);
    }

    [HttpGet("chatMessages/userId={userId:int}&chatId={chatId:int}")]
    public async Task<IActionResult> GetUserChatMessages(int userId, int chatId)
    {
        var messages = await _mediator.Send(new GetUserChatMessagesRequest(userId, chatId));
        var messagesDto = _mapper.Map<IEnumerable<MessageDTO>>(messages);
        return Ok(messagesDto);
    }
}
