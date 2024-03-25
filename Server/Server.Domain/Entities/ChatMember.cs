using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace Server.Domain.Entities;

public class ChatMember : Entity
{
    public ChatMember(int userId, int chatId)
    {
        UserId = userId;
        ChatId = chatId;
    }

    public int UserId { get; set; }
    public int ChatId { get; set; }
}
