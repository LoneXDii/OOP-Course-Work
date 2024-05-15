using Client.Domain.Entitites;

namespace Client.Persistence.Services;

public interface IServerService
{
    public event Action<Message>? GetMessageFromHubEvent;
    public event Action<Message>? DeleteMessageFromHubEvent;
    public event Action<Message>? UpdateMessageFromHubEvent;
    User LoginUser(string login, string password);
    User RegisterUser(string username, string login, string password);
    User UpdateUser(User user, string password);
    void DeleteUser(User user);
    List<Chat> GetUserChats(User user);
    List<Message> GetChatMessages(Chat chat);
    Message SendMessage(Message message);
    void DeleteMessage(Message message);
    void UpdateMessage(Message message);
}
