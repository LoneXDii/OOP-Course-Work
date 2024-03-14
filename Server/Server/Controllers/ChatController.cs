using Microsoft.AspNetCore.Mvc;
using Server.Data;
using Server.Models;

namespace Server.Controllers;

[ApiController]
[Route("api/[controller]")]
public class ChatController : Controller
{
    private readonly IDBService _dbService;
    public ChatController(IDBService db)
    {
        _dbService = db;
    }

    [HttpPost("addchat/name={name}&creator={creatorId:int}")]
    public IActionResult AddChat(string name, int creatorId)
    {
        var creator = _dbService.GetUserById(creatorId);

        if (creator is null) return BadRequest("No such user");

        var chat = new Chat() { Name = name };
        chat = _dbService.AddChat(chat);
        _dbService.AddChatMember(chat, creator);
        return Accepted(chat);
    }

    [HttpPost("addmember/chat={chatId}&member={memberId}")]
    public IActionResult AddMember(int chatId, int memberId)
    {
        var member = _dbService.GetUserById(memberId);
        var chat = _dbService.GetChatById(chatId);

        if (member is null) return BadRequest("No such user");
        if (chat is null) return BadRequest("No such chat");

        _dbService.AddChatMember(chat, member);
        return Accepted("OK");
    }

    [HttpGet("getchatmembers/chat={chatId}")]
    public IActionResult GetChatMembers(int chatId) {
        var members = _dbService.GetChatMembers(chatId);
        return Accepted(members);
    }

    [HttpGet("getchatmessages/chat={chatId}")]
    public IActionResult GetChatMessages(int chatId)
    {
        var messages = _dbService.GetChatMessages(chatId);
        return Accepted(messages);
    }
}
