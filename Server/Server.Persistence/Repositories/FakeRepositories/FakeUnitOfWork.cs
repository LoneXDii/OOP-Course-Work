using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace Server.Persistence.Repositories.FakeRepositories;

internal class FakeUnitOfWork : IUnitOfWork
{
    private readonly Lazy<IRepository<User>> _users;
    private readonly Lazy<IRepository<Chat>> _chats;
    private readonly Lazy<IRepository<ChatMember>> _chatMembers;
    private readonly Lazy<IRepository<Message>> _messages;

    public FakeUnitOfWork()
    {
        _users = new(() => new FakeUserRepository());
        _chats = new(() => new FakeChatRepository());
        _chatMembers = new (() => new FakeChatMemberrepository());
        _messages = new (() => new FakeMessageRepository());
    }

    public IRepository<User> UserRepository => _users.Value;
    public IRepository<Chat> ChatRepository => _chats.Value;
    public IRepository<Message> MessageRepository => _messages.Value;
    public IRepository<ChatMember> ChatMemberRepository => _chatMembers.Value;
    public Task SaveAllAsync()
    {
        throw new NotImplementedException();
    }
    public Task DeleteDataBaseAsync()
    {
        throw new NotImplementedException();
    }
    public Task CreateDataBaseAsync()
    {
        throw new NotImplementedException();
    }
}
