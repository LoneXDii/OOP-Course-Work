using Server.Models;

namespace Server.Data;

public interface IDBService
{
    public User AddUser(User user);
    public Chat AddChat(Chat chat);
    public Message AddMessage(Message message);
    public void AddChatMember(Chat chat, User member);
    public User? GetUserById(int id);
    public User? GetUserByName(string name);
    public Chat? GetChatById(int id);
    public IEnumerable<Chat> GetUserChats(int userId);
    public IEnumerable<Message> GetChatMessages(int chatId);
    public IEnumerable<User> GetChatMembers(int chatId);
}
