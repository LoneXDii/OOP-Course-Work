namespace Server.Domain.Entities;

public class User : Entity
{
    public User(string name, string login, string password)
    {
        Name = name;
        Login = login;
        Password = password;
    }

    public string AuthorizationToken { get; set; } = "";
    public string Name { get; set; }
    public string Login { get; set; }
    public string Password { get; set; }
    public List<Chat> Chats { get; set; } = new();
}
