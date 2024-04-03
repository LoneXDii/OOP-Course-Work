namespace Server.Infrastructure.Authentification;

public interface IJwtTokenGenerator
{
    string CreateToken(int id, string login, string password);
}
