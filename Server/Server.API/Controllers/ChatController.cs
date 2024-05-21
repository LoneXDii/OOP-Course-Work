using AutoMapper;
using Microsoft.AspNetCore.Mvc;
using Server.API.DTO;
using Microsoft.AspNetCore.Authorization;
using System.Security.Claims;

namespace Server.API.Controllers;

[Route("api/[controller]")]
[ApiController]
[Authorize]
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

    [HttpPost("create")]
    public async Task<IActionResult> CreateChat(ChatCreationDTO requestData)
    {
        _logger.LogInformation($"Processing route: {Request.Path.Value}");
        var chat = new Chat(requestData.Name);
        chat.Users.Add(await _mediator.Send(new GetUserByIdRequest(requestData.UserId)));
        chat = await _mediator.Send(new AddChatRequest(chat));
        var chatDto = _mapper.Map<ChatDTO>(chat);
        _logger.LogInformation($"{Request.Path.Value}: 200 OK");
        return Ok(chatDto);
    }

    [HttpPost("createDialogue")]
    public async Task<IActionResult> CreateDialogue(DialogueCreationDTO requestData)
    {
        _logger.LogInformation($"Processing route: {Request.Path.Value}");

        if (requestData.User1Id == requestData.User2Id)
        {
            _logger.LogError($"{Request.Path.Value}: 400 Bad Request");
            return BadRequest();
        }

        var dialogue = await _mediator.Send(new IsDialogueExistsRequest(requestData.User1Id, requestData.User2Id));
        if (dialogue is not null)
        {
            _logger.LogError($"{Request.Path.Value}: 400 Bad Request");
            return BadRequest($"{dialogue.Id}");
        }

        var chat = new Chat($"{requestData.User1Id}&{requestData.User2Id}");
        chat.Users.Add(await _mediator.Send(new GetUserByIdRequest(requestData.User1Id)));
        chat.Users.Add(await _mediator.Send(new GetUserByIdRequest(requestData.User2Id)));
        chat.IsDialogue = true;
        chat = await _mediator.Send(new AddChatRequest(chat));
        var chatDto = _mapper.Map<ChatDTO>(chat);
        _logger.LogInformation($"{Request.Path.Value}: 200 OK");
        return Ok(chatDto);
    }

    [HttpPut("update")]
    public async Task<IActionResult> UpdateChat(ChatDTO reqChat)
    {
        _logger.LogInformation($"Processing route: {Request.Path.Value}");

        if (!await IsChatMember(HttpContext, reqChat.Id))
        {
            _logger.LogError($"{Request.Path.Value}: 403 Forbidden");
            return Forbid();
        }

        var chat = await _mediator.Send(new GetChatByIdRequest(reqChat.Id));
        if (chat is null)
        {
            _logger.LogError($"{Request.Path.Value}: 404 Not Found");
            return NotFound();
        }
        if (chat.IsDialogue)
        {
            _logger.LogError($"{Request.Path.Value}: 400 Bad Request");
            return BadRequest();
        }
        chat.Name = reqChat.Name;
        chat = await _mediator.Send(new UpdateChatRequest(chat));
        var chatDto = _mapper.Map<ChatDTO>(chat);
        _logger.LogInformation($"{Request.Path.Value}: 200 OK");
        return Ok(chatDto);
    }

    [HttpDelete("delete/id={id:int}")]
    public async Task<IActionResult> DeleteChat(int id)
    {
        _logger.LogInformation($"Processing route: {Request.Path.Value}");

        if (!await IsChatMember(HttpContext, id))
        {
            _logger.LogError($"{Request.Path.Value}: 403 Forbidden");
            return Forbid();
        }

        await _mediator.Send(new DeleteChatRequest(id));
        _logger.LogInformation($"{Request.Path.Value}: 200 OK");
        return Ok();
    }

    [HttpPost("addMessage")]
    public async Task<IActionResult> AddMessage(MessageDTO reqMessage)
    {
        _logger.LogInformation($"Processing route: {Request.Path.Value}");

        if (!await IsChatMember(HttpContext, (int)reqMessage.ChatId!))
        {
            _logger.LogError($"{Request.Path.Value}: 403 Forbidden");
            return Forbid();
        }

        var message = new Message(reqMessage.Text);
        message.User = await _mediator.Send(new GetUserByIdRequest((int)reqMessage.UserId));
        message.ChatId = reqMessage.ChatId;
        message.SendTime = DateTime.Now;
        message = await _mediator.Send(new AddMessageRequest(message));
        var messageDto = _mapper.Map<MessageDTO>(message);
        _logger.LogInformation($"{Request.Path.Value}: 200 OK");
        return Ok(messageDto);
    }

    [HttpPut("updateMessage")]
    public async Task<IActionResult> UpdateMessage(MessageDTO reqMessage)
    {
        _logger.LogInformation($"Processing route: {Request.Path.Value}");

        if (!await IsChatMember(HttpContext, (int)reqMessage.ChatId!) || !IsMessageSender(HttpContext, reqMessage))
        {
            _logger.LogError($"{Request.Path.Value}: 403 Forbidden");
            return Forbid();
        }

        var message = await _mediator.Send(new GetMessageByIdRequest(reqMessage.Id));
        if(message is null)
        {
            _logger.LogError($"{Request.Path.Value}: 404 Not Found");
            return NotFound();
        }
        message.Text = reqMessage.Text;
        message = await _mediator.Send(new UpdateMessageRequest(message));
        var messageDto = _mapper.Map<MessageDTO>(message);
        _logger.LogInformation($"{Request.Path.Value}: 200 OK");
        return Ok(messageDto);
    }

    [HttpDelete("deleteMessage/id={id:int}")]
    public async Task<IActionResult> DeleteMessage(int id)
    {
        _logger.LogInformation($"Processing route: {Request.Path.Value}");

        var message = await _mediator.Send(new GetMessageByIdRequest(id));

        if(message is null)
        {
            _logger.LogError($"{Request.Path.Value}: 404 Not Found");
            return NotFound();
        }

        if (!await IsChatMember(HttpContext, (int)message.ChatId!) 
            || !IsMessageSender(HttpContext, _mapper.Map<MessageDTO>(message)))
        {
            _logger.LogError($"{Request.Path.Value}: 403 Forbidden");
            return Forbid();
        }

        await _mediator.Send(new DeleteMessageRequest(id));
        _logger.LogInformation($"{Request.Path.Value}: 200 OK");
        return Ok();
    }

    [HttpPost("addUser")]
    public async Task<IActionResult> AddUserToChat(UserAndChatDTO requestData)
    {
        _logger.LogInformation($"Processing route: {Request.Path.Value}");

        if (!await IsChatMember(HttpContext, requestData.ChatId))
        {
            _logger.LogError($"{Request.Path.Value}: 403 Forbidden");
            return Forbid();
        }

        var user = await _mediator.Send(new GetUserByIdRequest(requestData.UserId));
        var chat = await _mediator.Send(new GetChatByIdRequest(requestData.ChatId));
        if (user is null || chat is null)
        {
            _logger.LogError($"{Request.Path.Value}: 404 Not Found");
            return NotFound();
        }
        if (chat.IsDialogue)
        {
            _logger.LogError($"{Request.Path.Value}: 400 Bad Request");
            return BadRequest();
        }
        try
        {
            await _mediator.Send(new AddUserToChatRequest(requestData.UserId, requestData.ChatId));
            _logger.LogInformation($"{Request.Path.Value}: 200 OK");
            return Ok();
        }
        catch(Exception ex)
        {
            _logger.LogError($"{Request.Path.Value}: 400 Bad Request");
            return BadRequest(ex.Message);
        }
    }

    [HttpDelete("deleteUser/userId={userId:int}&chatId={chatId:int}")]
    public async Task<IActionResult> DeleteUserFromChat(int userId, int chatId)
    {
        _logger.LogInformation($"Processing route: {Request.Path.Value}");

        if (!await IsChatMember(HttpContext, chatId))
        {
            _logger.LogError($"{Request.Path.Value}: 403 Forbidden");
            return Forbid();
        }

        var chat = await _mediator.Send(new GetChatByIdRequest(chatId));

        if (chat is null)
        {
            _logger.LogError($"{Request.Path.Value}: 404 Not Found");
            return NotFound();
        }

        if (chat.IsDialogue)
        {
            _logger.LogError($"{Request.Path.Value}: 400 Bad Request");
            return BadRequest();
        }
        await _mediator.Send(new DeleteUserFromChatRequest(userId, chatId));
        _logger.LogInformation($"{Request.Path.Value}: 200 OK");
        return Ok();
    }

    [HttpGet("getMessages/chatId={id:int}")]
    public async Task<IActionResult> GetChatMessages(int id)
    {
        _logger.LogInformation($"Processing route: {Request.Path.Value}");

        if (!await IsChatMember(HttpContext, id))
        {
            _logger.LogError($"{Request.Path.Value}: 403 Forbidden");
            return Forbid();
        }

        var messages = await _mediator.Send(new GetChatMessagesRequest(id));
        if(messages is null)
        {
            _logger.LogError($"{Request.Path.Value}: 404 Not Found");
            return NotFound();
        }
        var messagesDto = _mapper.Map<IEnumerable<MessageDTO>>(messages);
        _logger.LogInformation($"{Request.Path.Value}: 200 OK");
        return Ok(messagesDto);
    }

    [HttpGet("getMembers/chatId={id:int}")]
    public async Task<IActionResult> GetChatMembers(int id)
    {
        _logger.LogInformation($"Processing route: {Request.Path.Value}");

        if (!await IsChatMember(HttpContext, id))
        {
            _logger.LogError($"{Request.Path.Value}: 403 Forbidden");
            return Forbid();
        }

        var members = await _mediator.Send(new GetChatMembersRequest(id));
        if(members is null)
        {
            _logger.LogError($"{Request.Path.Value}: 404 Not Found");
            return NotFound();
        }
        var membersDto = _mapper.Map<List<UserDTO>>(members);
        _logger.LogInformation($"{Request.Path.Value}: 200 OK");
        return Ok(membersDto);
    }

    private async Task<bool> IsChatMember(HttpContext context, int chatId)
    {
        var userId = Convert.ToInt32(context.User.FindFirstValue("Id"));
        if (await _mediator.Send(new IsUserInChatRequest(userId, chatId)))
        {
            return true;
        }
        return false;
    }

    private bool IsMessageSender(HttpContext context, MessageDTO message)
    {
        var userId = Convert.ToInt32(context.User.FindFirstValue("Id"));
        if (userId == message.UserId)
        {
            return true;
        }
        return false;
    }
}
