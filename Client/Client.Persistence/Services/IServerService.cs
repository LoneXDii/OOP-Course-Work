using Client.Domain.Entitites;

namespace Client.Persistence.Services;

internal interface IServerService
{
    User Login(string login, string password);
}
