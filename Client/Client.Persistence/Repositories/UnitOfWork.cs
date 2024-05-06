using Client.Domain.Abstractions;
using Client.Domain.Entitites;
using Client.Persistence.Services;

namespace Client.Persistence.Repositories;

internal class UnitOfWork : IUnitOfWork
{
    private readonly IServerService _serverService;
    private readonly Lazy<IRepository<User>> _users;
    private readonly Lazy<IRepository<Chat>> _chats;
    private readonly Lazy<IRepository<Message>> _messages;

    public UnitOfWork(IServerService serverService)
    {
        _serverService = serverService;
        _users = new Lazy<IRepository<User>>(() => new Repository<User>(serverService));
        _chats = new Lazy<IRepository<Chat>>(() => new Repository<Chat>(serverService));
        _messages = new Lazy<IRepository<Message>>(() => new Repository<Message>(serverService));
    }

    public IRepository<User> UserRepository => _users.Value;
    public IRepository<Chat> ChatRepository => _chats.Value;
    public IRepository<Message> MessageRepository => _messages.Value;
}
