using Client.Persistence.Services;

namespace Client.Persistence.Repositories;

internal class UnitOfWork : IUnitOfWork
{
    private readonly IServerService _serverService;

    public UnitOfWork(IServerService serverService)
    {
        _serverService = serverService;
        User = new UserController(serverService);
        ChatRepository = new ChatRepository(serverService);
        ChatMembersRepository = new ChatMembersRepository(serverService);
        MessageRepository = new MessageRepository(serverService);
    }

    public ChatRepository ChatRepository { get; private set; }
    public ChatMembersRepository ChatMembersRepository { get; private set; }
    public MessageRepository MessageRepository { get; private set; }
    public UserController User { get; private set; }
}
