using AutoMapper;
using Microsoft.AspNetCore.Mvc;
using Server.API.DTO;
using Server.API.SerialzationLib;
using Server.API.DTO;
using System.Collections;

namespace Server.API.Controllers;

[Route("api/[controller]")]
[ApiController]
//[Authorize]
public class ChatController : Controller
{
    private readonly IMediator _mediator;
    private readonly IMapper _mapper;
    private readonly ILogger _logger;

    public ChatController(IMediator mediator, IMapper mapper, ILogger<ChatController> logger)
    {
        _mediator = mediator;
        _mapper = mapper;
        _logger = logger;
    }

    [HttpPost("create/name={name}&creatorId={id:int}")]
    public async Task<IActionResult> CreateChat(string name, int id)
    {
        _logger.LogInformation($"Processing route: {Request.Path.Value}");
        var chat = new Chat(name);
        chat.Users.Add(await _mediator.Send(new GetUserByIdRequest(id)));
        chat = await _mediator.Send(new AddChatRequest(chat));
        var chatDto = _mapper.Map<ChatDTO>(chat);
        return Ok(chatDto);
    }

    [HttpPut("update/id={id:int}&name={name}")]
    public async Task<IActionResult> UpdateChat(int id, string name)
    {
        _logger.LogInformation($"Processing route: {Request.Path.Value}");
        var chat = await _mediator.Send(new GetChatByIdRequest(id));
        if(chat is null)
        {
            return NotFound();
        }
        chat.Name = name;
        chat = await _mediator.Send(new UpdateChatRequest(chat));
        var chatDto = _mapper.Map<ChatDTO>(chat);
        return Ok(chatDto);
    }

    [HttpDelete("delete/id={id:int}")]
    public async Task<IActionResult> DeleteChat(int id)
    {
        _logger.LogInformation($"Processing route: {Request.Path.Value}");
        await _mediator.Send(new DeleteChatRequest(id));
        return Ok();
    }

    [HttpPost("addMessage/text={text}&senderId={senderId:int}&chatId={chatId:int}")]
    public async Task<IActionResult> AddMessage(string text, int senderId, int chatId)
    {
        _logger.LogInformation($"Processing route: {Request.Path.Value}");
        var message = new Message(text);
        message.User = await _mediator.Send(new GetUserByIdRequest(senderId));
        message.ChatId = chatId;
        message = await _mediator.Send(new AddMessageRequest(message));
        var messageDto = _mapper.Map<MessageDTO>(message);
        return Ok(messageDto);
    }

    [HttpPut("updateMessage/id={id:int}&text={text}")]
    public async Task<IActionResult> UpdateMessage(int id, string text)
    {
        _logger.LogInformation($"Processing route: {Request.Path.Value}");
        var message = await _mediator.Send(new GetMessageByIdRequest(id));
        if(message is null)
        {
            return NotFound();
        }
        message.Text = text;
        message = await _mediator.Send(new UpdateMessageRequest(message));
        var messageDto = _mapper.Map<MessageDTO>(message);
        return Ok(messageDto);
    }

    [HttpDelete("deleteMessage/id={id:int}")]
    public async Task<IActionResult> DeleteMessage(int id)
    {
        _logger.LogInformation($"Processing route: {Request.Path.Value}");
        await _mediator.Send(new DeleteMessageRequest(id));
        return Ok();
    }

    [HttpPost("addUser/userId={userId:int}&chatId={chatId:int}")]
    public async Task<IActionResult> AddUserToChat(int userId, int chatId)
    {
        _logger.LogInformation($"Processing route: {Request.Path.Value}");
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
        _logger.LogInformation($"Processing route: {Request.Path.Value}");
        await _mediator.Send(new DeleteUserFromChatRequest(userId, chatId));
        return Ok();
    }

    [HttpGet("getMessages/chatId={id:int}")]
    public async Task<IActionResult> GetChatMessages(int id)
    {
        _logger.LogInformation($"Processing route: {Request.Path.Value}");
        var messages = await _mediator.Send(new GetChatMessagesRequest(id));
        if(messages is null)
        {
            return NotFound();
        }
        var messagesDto = _mapper.Map<IEnumerable<MessageDTO>>(messages);
        return Ok(messagesDto);
    }

    [HttpGet("getMembers/chatId={id:int}")]
    public async Task<IActionResult> GetChatMembers(int id)
    {
        _logger.LogInformation($"Processing route: {Request.Path.Value}");
        var members = await _mediator.Send(new GetChatMembersRequest(id));
        if(members is null)
        {
            return NotFound();
        }
        var membersDto = _mapper.Map<List<UserDTO>>(members);
        return Ok(membersDto);
    }
}
