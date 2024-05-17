using Client.Domain.Entitites;

namespace Client.Persistence.Services;

public interface IServerService
{
    public event Action<Message>? GetMessageHubEvent;
    public event Action<Message>? DeleteMessageHubEvent;
    public event Action<Message>? UpdateMessageHubEvent;
    public event Action<User, Chat>? DeleteChatMemberHubEvent;
    User LoginUser(string login, string password);
    User RegisterUser(string username, string login, string password);
    User UpdateUser(User user, string password);
    void DeleteUser(User user);
    List<Chat> GetUserChats(User user);
    List<Message> GetChatMessages(Chat chat);
    List<User> GetChatMembers(Chat chat);
    Message SendMessage(Message message);
    void DeleteMessage(Message message);
    void UpdateMessage(Message message);
    void DeleteChatMember(Chat chat, User user);
    void UpdateChatName(Chat chat);
}
