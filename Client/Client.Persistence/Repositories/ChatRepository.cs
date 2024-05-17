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
    }

    public ObservableCollection<Chat> Chats { get; private set; } = new();

    public void GetFromServer(User user)
    {
        Chats = new ObservableCollection<Chat>(_serverService.GetUserChats(user));
    }
}
