namespace Server.API.DTO;

public class UserDTO
{
    public int Id { get; set; }
    public string AuthorizationToken { get; set; } = "";
    public string Name { get; set; } = "";
    public string Login { get; set; } = "";
    public string Password { get; set; } = "";
}
