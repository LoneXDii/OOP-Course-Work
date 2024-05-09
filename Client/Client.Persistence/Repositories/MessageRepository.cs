using Client.Domain.Entitites;
using Client.Persistence.Services;
using System.Collections.ObjectModel;

namespace Client.Persistence.Repositories;

public class MessageRepository
{
    private readonly IServerService _serverService;

    public MessageRepository(IServerService serverService)
    {
        _serverService = serverService;
        _serverService.GetMessageFromHubEvent += GetFromHub;
    }

    public ObservableCollection<Message> Messages { get; private set; } = new();

    public void GetFromServer(Chat chat)
    {
        Messages = new ObservableCollection<Message>(_serverService.GetChatMessages(chat));
    }

    public void SendToServer(Message message)
    {
        _serverService.SendMessage(message);
    }

    private void GetFromHub(Message message)
    {
        Messages.Add(message);
    }
}
