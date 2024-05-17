using Client.Domain.Entitites;
using Client.Persistence.Services;
using System.Collections.ObjectModel;

namespace Client.Persistence.Repositories;

public class ChatMembersRepository
{
    private readonly IServerService _serverService;
    private Chat _currentChat = new();

    public ChatMembersRepository(IServerService serverService)
    {
        _serverService = serverService;
    }

    public ObservableCollection<User> Members { get; private set; } = new();
    public Chat CurrentChat { get { return _currentChat; } }

    public void GetFromServer(Chat chat)
    {
        _currentChat = chat;
        Members = new ObservableCollection<User>(_serverService.GetChatMembers(chat));
    }
}
