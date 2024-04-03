namespace Server.Infrastructure.Authentification;

public class JwtSettings
{
    public const string SectionName = "JwtSettings";
    public string Secret { get; init; } = "TempSecretKey123123123123123123123";
    public int ExpiryMinutes { get; init; }
    public string Issuer { get; init; } = null!;
}
