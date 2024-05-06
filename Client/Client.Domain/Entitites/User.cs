namespace Client.Domain.Entitites;

public class User : Entity
{
    public string AuthorizationToken { get; set; } = "";
    public string Name { get; set; } = "";
    public string Login { get; set; } = "";
    public string Password { get; set; } = "";
}
