using Client.Domain.Entitites;

namespace Client.Persistence.Repositories;

public interface IUnitOfWork
{ 
    ChatRepository ChatRepository { get; }
    MessageRepository MessageRepository { get; }
    ChatMembersRepository ChatMembersRepository { get; }
    UserController User { get; }

    List<User> AllUsers();
}
