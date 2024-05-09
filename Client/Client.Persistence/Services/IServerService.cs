using Client.Domain.Entitites;
using static Client.Persistence.Services.ServerService;

namespace Client.Persistence.Services;

public interface IServerService
{
    public event Action<Message>? GetMessageFromHubEvent;
    User LoginUser(string login, string password);
    User RegisterUser(string username, string login, string password);
    User UpdateUser(User user);
    void DeleteUser(User user);
    List<Chat> GetUserChats(User user);
    List<Message> GetChatMessages(Chat chat);
    Message SendMessage(Message message);
}
