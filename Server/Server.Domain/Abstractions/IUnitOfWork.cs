using Server.Domain.Entities;

namespace Server.Domain.Abstractions;

public interface IUnitOfWork
{
    IRepository<User> UserRepository { get; }
    IRepository<Chat> ChatRepository { get; }
    IRepository<Message> MessageRepository { get; }
    IRepository<ChatMember> ChatMemberRepository { get; }

    public Task SaveAllAsync();
    public Task DeleteDataBaseAsync();
    public Task CreateDataBaseAsync();
}
