using Client.Domain.Entitites;
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

        _serverService.AddChatMemberHubEvent += AddAsMemberFromHub;
    }

    public ChatRepository ChatRepository { get; private set; }
    public ChatMembersRepository ChatMembersRepository { get; private set; }
    public MessageRepository MessageRepository { get; private set; }
    public UserController User { get; private set; }

    public List<User> AllUsers()
    {
        return _serverService.GetAllUsers();
    }

    private void AddAsMemberFromHub(User user, Chat chat)
    {
        if (User.GetUser().Id == user.Id)
        {
            ChatRepository.Chats.Add(chat);
        }
    }
}
