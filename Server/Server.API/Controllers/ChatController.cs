﻿using AutoMapper;
using Microsoft.AspNetCore.Mvc;
using Server.API.DTO;
using Microsoft.AspNetCore.Authorization;
using System.Security.Claims;
using System.ComponentModel;
using System.Diagnostics;
using Server.Domain.Entities;

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

    [HttpPut("update")]
    public async Task<IActionResult> UpdateChat(ChatDTO reqChat)
    {
        _logger.LogInformation($"Processing route: {Request.Path.Value}");

        if (!await IsChatMember(HttpContext, reqChat.Id))
        {
            return Forbid();
        }

        var chat = await _mediator.Send(new GetChatByIdRequest(reqChat.Id));
        if (chat is null)
        {
            return NotFound();
        }
        chat.Name = reqChat.Name;
        chat = await _mediator.Send(new UpdateChatRequest(chat));
        var chatDto = _mapper.Map<ChatDTO>(chat);
        return Ok(chatDto);
    }

    [HttpDelete("delete/id={id:int}")]
    public async Task<IActionResult> DeleteChat(int id)
    {
        _logger.LogInformation($"Processing route: {Request.Path.Value}");

        if (!await IsChatMember(HttpContext, id))
        {
            return Forbid();
        }

        await _mediator.Send(new DeleteChatRequest(id));
        return Ok();
    }

    [HttpPost("addMessage")]
    public async Task<IActionResult> AddMessage(MessageDTO reqMessage)
    {
        _logger.LogInformation($"Processing route: {Request.Path.Value}");

        if (!await IsChatMember(HttpContext, (int)reqMessage.ChatId!))
        {
            return Forbid();
        }

        var message = new Message(reqMessage.Text);
        message.User = await _mediator.Send(new GetUserByIdRequest((int)reqMessage.UserId));
        message.ChatId = reqMessage.ChatId;
        message.SendTime = DateTime.Now;
        message = await _mediator.Send(new AddMessageRequest(message));
        var messageDto = _mapper.Map<MessageDTO>(message);
        return Ok(messageDto);
    }

    [HttpPut("updateMessage")]
    public async Task<IActionResult> UpdateMessage(MessageDTO reqMessage)
    {
        _logger.LogInformation($"Processing route: {Request.Path.Value}");

        if (!await IsChatMember(HttpContext, (int)reqMessage.ChatId!) || !IsMessageSender(HttpContext, reqMessage))
        {
            return Forbid();
        }

        var message = await _mediator.Send(new GetMessageByIdRequest(reqMessage.Id));
        if(message is null)
        {
            return NotFound();
        }
        message.Text = reqMessage.Text;
        message = await _mediator.Send(new UpdateMessageRequest(message));
        var messageDto = _mapper.Map<MessageDTO>(message);
        return Ok(messageDto);
    }

    [HttpDelete("deleteMessage/id={id:int}")]
    public async Task<IActionResult> DeleteMessage(int id)
    {
        _logger.LogInformation($"Processing route: {Request.Path.Value}");

        var message = await _mediator.Send(new GetMessageByIdRequest(id));

        if(message is null)
        {
            return NotFound();
        }

        if (!await IsChatMember(HttpContext, (int)message.ChatId!) 
            || !IsMessageSender(HttpContext, _mapper.Map<MessageDTO>(message)))
        {
            return Forbid();
        }

        await _mediator.Send(new DeleteMessageRequest(id));
        return Ok();
    }

    [HttpPost("addUser")]
    public async Task<IActionResult> AddUserToChat(UserAndChatDTO requestData)
    {
        _logger.LogInformation($"Processing route: {Request.Path.Value}");

        if (!await IsChatMember(HttpContext, requestData.ChatId))
        {
            return Forbid();
        }

        var user = await _mediator.Send(new GetUserByIdRequest(requestData.UserId));
        var chat = await _mediator.Send(new GetChatByIdRequest(requestData.ChatId));
        if (user is null || chat is null)
        {
            return NotFound();
        }
        if (chat.IsDialogue)
        {
            return BadRequest();
        }
        await _mediator.Send(new AddUserToChatRequest(requestData.UserId, requestData.ChatId));
        return Ok();
    }

    [HttpDelete("deleteUser/userId={userId:int}&chatId={chatId:int}")]
    public async Task<IActionResult> DeleteUserFromChat(int userId, int chatId)
    {
        _logger.LogInformation($"Processing route: {Request.Path.Value}");

        if (!await IsChatMember(HttpContext, chatId))
        {
            return Forbid();
        }

        await _mediator.Send(new DeleteUserFromChatRequest(userId, chatId));
        return Ok();
    }

    [HttpGet("getMessages/chatId={id:int}")]
    public async Task<IActionResult> GetChatMessages(int id)
    {
        _logger.LogInformation($"Processing route: {Request.Path.Value}");

        if (!await IsChatMember(HttpContext, id))
        {
            return Forbid();
        }

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

        if (!await IsChatMember(HttpContext, id))
        {
            return Forbid();
        }

        var members = await _mediator.Send(new GetChatMembersRequest(id));
        if(members is null)
        {
            return NotFound();
        }
        var membersDto = _mapper.Map<List<UserDTO>>(members);
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
