using Microsoft.AspNetCore.Mvc;

namespace Server.API.Controllers;

[Route("api/[controller]")]
[ApiController]
//[Authorize]
public class ChatController : Controller
{
    private readonly IMediator _mediator;

    public ChatController(IMediator mediator)
    {
        _mediator = mediator;
    }

    [HttpPost("create/name={name}&creatorId={id:int}")]
    public async Task<IActionResult> CreateChat(string name, int id)
    {
        var chat = new Chat(name);
        chat.CreatorId = id;
        chat = await _mediator.Send(new AddChatRequest(chat));
        return Ok(chat);
    }

    [HttpPost("update/id={id:int}&name={name}")]
    public async Task<IActionResult> UpdateChat(int id, string name)
    {
        var chat = await _mediator.Send(new GetChatByIdRequest(id));
        if(chat is null)
        {
            return NotFound();
        }
        chat.Name = name;
        chat = await _mediator.Send(new UpdateChatRequest(chat));
        return Ok(chat);
    }

    [HttpDelete("delete/id={id:int}")]
    public async Task<IActionResult> DeleteChat(int id)
    {
        await _mediator.Send(new DeleteChatRequest(id));
        return Ok();
    }

    [HttpPost("addMessage/text={text}&senderId={senderId:int}&chatId={chatId:int}")]
    public async Task<IActionResult> AddMessage(string text, int senderId, int chatId)
    {
        var message = new Message(text);
        message.SenderId = senderId;
        message.ChatId = chatId;
        message = await _mediator.Send(new AddMessageRequest(message));
        return Ok(message);
    }

    [HttpPost("updateMessage/id={id:int}&text={text}")]
    public async Task<IActionResult> UpdateMessage(int id, string text)
    {
        var message = await _mediator.Send(new GetMessageByIdRequest(id));
        if(message is null)
        {
            return NotFound();
        }
        message.Text = text;
        message = await _mediator.Send(new UpdateMessageRequest(message));
        return Ok(message);
    }

    [HttpDelete("deleteMessage/id={id:int}")]
    public async Task<IActionResult> DeleteMessage(int id)
    {
        await _mediator.Send(new DeleteMessageRequest(id));
        return Ok();
    }

    [HttpPost("addUser/userId={userId:int}&chatId={chatId:int}")]
    public async Task<IActionResult> AddUserToChat(int userId, int chatId)
    {
        var user = await _mediator.Send(new GetUserByIdRequest(userId));
        var chat = await _mediator.Send(new GetChatByIdRequest(chatId));
        if (user is null || chat is null)
        {
            return NotFound();
        }
        await _mediator.Send(new AddUserToChatRequest(userId, chatId));
        return Ok();
    }

    [HttpDelete("deleteUser/userId={userId:int}&chatId={chatId:int}")]
    public async Task<IActionResult> DeleteUserFromChat(int userId, int chatId)
    {
        await _mediator.Send(new DeleteUserFromChatRequest(userId, chatId));
        return Ok();
    }

    [HttpGet("getMessages/chatId={id:int}")]
    public async Task<IActionResult> GetChatMessages(int id)
    {
        var messages = await _mediator.Send(new GetChatMessagesRequest(id));
        if(messages is null)
        {
            return NotFound();
        }
        return Ok(messages);
    }

    [HttpGet("getMembers/chatId={id:int}")]
    public async Task<IActionResult> GetChatMembers(int id)
    {
        var members = await _mediator.Send(new GetChatMembersRequest(id));
        if(members is null)
        {
            return NotFound();
        }
        return Ok(members);
    }
}
