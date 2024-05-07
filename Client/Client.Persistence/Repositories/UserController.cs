using Client.Domain.Entitites;
using Client.Persistence.Services;

namespace Client.Persistence.Repositories;

public class UserController
{
    private User? _user = null;
    private readonly IServerService _serverService;

    public UserController(IServerService serverService)
    {
        _serverService = serverService;
    }
    public void Login(string login, string password)
    {
        _user = _serverService.LoginUser(login, password);
    }

    public void Register(string username, string login, string password)
    {
        _user = _serverService.RegisterUser(username, login, password);
    }

    public void Update(string username, string password)
    {
        if (_user is null)
        {
            throw new NullReferenceException("No user");
        }
        _user.Name = username;
        _user.Password = password;
        _user = _serverService.UpdateUser(_user);
    }

    public void Delete()
    {
        if (_user is null)
        {
            throw new NullReferenceException("No user");
        }
        _serverService.DeleteUser(_user);
        _user = null;
    }

    public User GetUser()
    {
        if (_user is null)
        {
            throw new NullReferenceException("No user");
        }
        return (User)_user.Clone();
    }
}
