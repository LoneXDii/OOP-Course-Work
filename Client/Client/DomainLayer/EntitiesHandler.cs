using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace Client.DomainLayer;

internal class EntitiesHandler
{
    public List<Chat> Chats { get; set; } = new List<Chat>();
    public List<User> ChatMembers { get; set; } = new List<User>();
    public List<User> Users { get; } = new List<User>();
    public List<Message> ChatMessages { get; set; } = new List<Message>();
    public User? CurrentUser { get; set; }
    public Chat? CurrentChat { get; set; }

    public void AddMessage(Message message)
    {
        ChatMessages.Add(message);
    }

    public void AddChat(Chat chat)
    {
        Chats.Add(chat);
    }

    public void AddChatMember(User chatMember)
    {
        ChatMembers.Add(chatMember);
    }
}
