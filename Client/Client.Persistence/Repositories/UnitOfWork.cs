using Client.Domain.Abstractions;
using Client.Domain.Entitites;
using Client.Persistence.Services;

namespace Client.Persistence.Repositories;

internal class UnitOfWork : IUnitOfWork
{
    private readonly IServerService _serverService;
    private User? _user;
    private readonly Lazy<IRepository<Chat>> _chats;
    private readonly Lazy<IRepository<Message>> _messages;

    public UnitOfWork(IServerService serverService)
    {
        _serverService = serverService;
        _user = null;
        _chats = new Lazy<IRepository<Chat>>(() => new ChatRepository(serverService));
        _messages = new Lazy<IRepository<Message>>(() => new MessageRepository(serverService));
    }

    public IRepository<Chat> ChatRepository => _chats.Value;
    public IRepository<Message> MessageRepository => _messages.Value;

    public void Login(string login, string password)
    {
        _user = _serverService.Login(login, password);
    }
}
