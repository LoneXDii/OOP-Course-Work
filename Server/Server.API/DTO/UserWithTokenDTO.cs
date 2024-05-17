namespace Server.API.DTO;

public class UserWithTokenDTO
{
    public string AuthorizationToken { get; set; } = "";
    public int Id { get; set; }
    public string Name { get; set; } = "";
}
