using Client.Domain.Entitites;
using Client.Persistence.Services;

namespace Client.Persistence.Repositories;

public class MessageRepository
{
    private readonly IServerService _serverService;
    protected List<Message> _entities = new();

    public MessageRepository(IServerService serverService)
    {
        _serverService = serverService;
    }

    public void GetFromServer(Chat chat)
    {
        _entities = _serverService.GetChatMessages(chat);
    }

    public IReadOnlyList<Message> GetMessages()
    {
        return _entities;
    }
}
