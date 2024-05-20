using Client.Domain.Entitites;
using Client.Persistence.Services;
using System.Collections.ObjectModel;

namespace Client.Persistence.Repositories;

public class ChatRepository
{
    private readonly IServerService _serverService;

    public ChatRepository(IServerService serverService)
    {
        _serverService = serverService;
        _serverService.UpdateChatHubEvent += UpdateFromHub;
    }

    public ObservableCollection<Chat> Chats { get; private set; } = new();

    public void GetFromServer(User user)
    {
        Chats = new ObservableCollection<Chat>(_serverService.GetUserChats(user));
    }

    public void Update(Chat chat)
    {
        _serverService.UpdateChat(chat);
    }

    public void Create(string name, User user)
    {
        Chats.Add(_serverService.CreateChat(name, user));
    }

    public Chat CreateDialogue(User user1, User user2)
    {
        return _serverService.CreateDialogue(user1, user2);
    }

    private void UpdateFromHub(Chat chat)
    {
        var tempChat = Chats.FirstOrDefault(m => m.Id == chat.Id);
        if (tempChat is null)
        {
            return;
        }
        var index = Chats.IndexOf(tempChat);
        Chats.RemoveAt(index);
        Chats.Insert(index, chat);
    }
}
