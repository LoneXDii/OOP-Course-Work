using Client.Domain.Entitites;
using Client.Persistence.Services;
using System.Linq.Expressions;

namespace Client.Persistence.Repositories;

public class ChatRepository
{
    private readonly IServerService _serverService;
    protected List<Chat> _entities = new();

    public ChatRepository(IServerService serverService)
    {
        _serverService = serverService;

    }

    public void GetFromServer(User user)
    {
        _entities = _serverService.GetUserChats(user);
    }

    public IReadOnlyList<Chat> GetChats()
    {
        return _entities;
    }
}
