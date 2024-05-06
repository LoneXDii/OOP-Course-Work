using Client.Domain.Entitites;

namespace Client.Domain.Abstractions;

public interface IUnitOfWork
{ 
    IRepository<Chat> ChatRepository { get; }
    IRepository<Message> MessageRepository { get; }
    IUserController User { get; }
}
