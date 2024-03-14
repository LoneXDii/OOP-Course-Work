using Server.Models;
    
namespace Server.Data;

public class LocalDataStorage : IDBService
{
    public List<User> Users { get; set; } = new List<User>();
    public List<Chat> Chats { get; set; } = new List<Chat>();
    public List<Message> Messages { get; set; } = new List<Message>();
    public List<(User, Chat)> ChatMembers { get; set; } = new List<(User, Chat)>();

    public User AddUser(User user)
    {
        user.Id = Users.Count;
        Users.Add(user);
        return user;
    }

    public Chat AddChat(Chat chat)
    {
        chat.Id = Chats.Count;
        Chats.Add(chat);
        return chat;
    }

    public Message AddMessage(Message message)
    {
        message.Id = Messages.Count;
        Messages.Add(message);
        return message;
    }

    public void AddChatMember(Chat chat, User member)
    {
        ChatMembers.Add((member, chat));
    }

    public User? GetUserById(int id)
    {
        return Users.FirstOrDefault(x => x.Id == id);
    }

    public User? GetUserByName(string name)
    {
        return Users.FirstOrDefault(x => x.UserName == name);
    }

    public Chat? GetChatById(int id)
    {
        return Chats.FirstOrDefault(x => x.Id == id);
    }

    public IEnumerable<Chat> GetUserChats(int userId)
    {
        return (from member in ChatMembers
                where member.Item1.Id == userId
                select member.Item2);
    }

    public IEnumerable<Message> GetChatMessages(int chatId)
    {
        return (from message in Messages
                where message.ChatId == chatId
                select message);
    }

    public IEnumerable<User> GetChatMembers(int chatId)
    {
        return (from member in ChatMembers
                where member.Item2.Id == chatId
                select member.Item1);
    }
}
