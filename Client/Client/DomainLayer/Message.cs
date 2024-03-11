using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace Client.DomainLayer;

internal class Message
{
    public int Id { get; set; }
    public int SenderId { get; set; }
    public int ChatId { get; set; }
    public DateTime SendTime { get; set; }
    public string Text { get; set; }

    public Message(string text, int senderId, int chatId)
    {
        Text = text;
        SendTime = DateTime.Now;
        SenderId = senderId;
        ChatId = chatId;
    }
}
