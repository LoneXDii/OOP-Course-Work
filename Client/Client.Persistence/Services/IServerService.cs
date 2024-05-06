using Client.Domain.Entitites;

namespace Client.Persistence.Services;

internal interface IServerService
{
    User LoginUser(string login, string password);
    User RegisterUser(string username, string login, string password);
    User UpdateUser(User user);
    void DeleteUser(User user);
}
