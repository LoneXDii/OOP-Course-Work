using Microsoft.IdentityModel.Tokens;
using System.Text;

namespace Server.Infrastructure.Authentification;

public class JwtSettings
{
    public const string SectionName = "JwtSettings";
    private const string Secret = "TempSecretKey123123123123123123123";
    public const int ExpiryMinutes = 30;
    public const string Issuer = "MyAuthServer";
    public const string Audience = "MyAuthClient";
    public static SymmetricSecurityKey GetSymmetricSecurityKey() => new SymmetricSecurityKey(Encoding.UTF8.GetBytes(Secret));
}
