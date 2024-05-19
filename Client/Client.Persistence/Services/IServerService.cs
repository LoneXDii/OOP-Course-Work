using Client.Domain.Entitites;

namespace Client.Persistence.Services;

public interface IServerService
{
    public event Action<Message>? GetMessageHubEvent;
    public event Action<Message>? DeleteMessageHubEvent;
    public event Action<Message>? UpdateMessageHubEvent;
    public event Action<User, Chat>? DeleteChatMemberHubEvent;
    public event Action<User, Chat>? AddChatMemberHubEvent;
    public event Action<Chat>? UpdateChatHubEvent;
    List<User> GetAllUsers();
    User LoginUser(string login, string password);
    User RegisterUser(string username, string login, string password);
    User UpdateUsername(User user);
    void UpdatePassword(User user, string oldPassword, string newPassowrd);
    void DeleteUser(User user);
    List<Chat> GetUserChats(User user);
    List<Message> GetChatMessages(Chat chat);
    List<User> GetChatMembers(Chat chat);
    Message SendMessage(Message message);
    void DeleteMessage(Message message);
    void UpdateMessage(Message message);
    void DeleteChatMember(Chat chat, User user);
    void AddChatMember(Chat chat, User user);
    void UpdateChat(Chat chat);
    Chat CreateChat(string name, User creator);
}
