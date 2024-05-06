using Client.Domain.Entitites;

namespace Client.Domain.Abstractions;

public interface IUnitOfWork
{
    IRepository<User> UserRepository { get; }
    IRepository<Chat> ChatRepository { get; }
    IRepository<Message> MessageRepository { get; }
}
