namespace Server.Infrastructure.Persistence.Repositories.FakeRepositories;

internal class FakeUnitOfWork : IUnitOfWork
{
    private readonly Lazy<IRepository<User>> _users;
    private readonly Lazy<IRepository<Chat>> _chats;
    private readonly Lazy<IRepository<ChatMember>> _chatMembers;
    private readonly Lazy<IRepository<Message>> _messages;

    public FakeUnitOfWork()
    {
        _users = new Lazy<IRepository<User>>(() => new FakeUserRepository());
        _chats = new Lazy<IRepository<Chat>>(() => new FakeChatRepository());
        _chatMembers = new Lazy<IRepository<ChatMember>>(() => new FakeChatMemberRepository());
        _messages = new Lazy<IRepository<Message>>(() => new FakeMessageRepository());
    }

    public IRepository<User> UserRepository => _users.Value;
    public IRepository<Chat> ChatRepository => _chats.Value;
    public IRepository<Message> MessageRepository => _messages.Value;
    public IRepository<ChatMember> ChatMemberRepository => _chatMembers.Value;
    public async Task SaveAllAsync()
    {
        await Task.Delay(1);
    }
    public async Task DeleteDataBaseAsync()
    {
        await Task.Delay(1);
    }
    public async Task CreateDataBaseAsync()
    {
        await Task.Delay(1);
    }
}
