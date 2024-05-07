using Client.Domain.Entitites;

namespace Client.Persistence.Services;

public interface IServerService
{
    User LoginUser(string login, string password);
    User RegisterUser(string username, string login, string password);
    User UpdateUser(User user);
    void DeleteUser(User user);
    List<Chat> GetUserChats(User user);
    List<Message> GetChatMessages(Chat chat);
}
