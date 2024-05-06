namespace Client.Domain.Abstractions;

public interface IUserController
{
    void Login(string login, string password);
    void Register(string username, string login, string password);
    void Update(string username, string password);
    void Delete();
}
