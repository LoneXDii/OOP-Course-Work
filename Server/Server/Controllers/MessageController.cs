using Microsoft.AspNetCore.Mvc;
using Server.Data;
using Server.Models;

namespace Server.Controllers;

[ApiController]
[Route("api/[controller]")]
public class MessageController : Controller
{
    private readonly IDBService _dbService;

    public MessageController(IDBService db)
    {
        _dbService = db;
    }

    [HttpPost("addmessage/text={text}&chat={chatId:int}&sender={senderId:int}")]
    public IActionResult AddMessage(string text, int chatId, int senderId)
    {
        var sender = _dbService.GetUserById(senderId);
        var chat = _dbService.GetChatById(chatId);

        if (sender is null) return BadRequest("No such user");
        if (chat is null) return BadRequest("No such chat");

        var message = new Message() { Content = text, ChatId = chatId, SenderId = senderId, CreatedAt = DateTime.Now };
        message = _dbService.AddMessage(message);
        return Accepted(message);
    }
}
