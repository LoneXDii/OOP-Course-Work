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
            if (chat.IsDialogue)
            {
                var ids = chat.Name.Split('&');
                try
                {
                    var u1Id = Convert.ToInt32(ids[0]);
                    var u2Id = Convert.ToInt32(ids[1]);
                    var member = _serverService.GetChatMembers(chat).FirstOrDefault(u => u.Id != user.Id);
                    chat.Name = member is null ? "DELETED" : member.Name;
                }
                catch { }
            }
            var tempChat = ChatRepository.Chats.FirstOrDefault(c => c.LastMessageDate <= chat.LastMessageDate);
            if (tempChat is null)
            {
                ChatRepository.Chats.Add(chat);
            }
            else
            {
                int index = ChatRepository.Chats.IndexOf(tempChat);
                ChatRepository.Chats.Insert(index, chat);
            }
        }
    }
}
