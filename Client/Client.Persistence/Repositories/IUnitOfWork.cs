using Client.Domain.Entitites;

namespace Client.Persistence.Repositories;

public interface IUnitOfWork
{ 
    ChatRepository ChatRepository { get; }
    MessageRepository MessageRepository { get; }
    UserController User { get; }
    Chat? CurrentChat { get; set; }
}
