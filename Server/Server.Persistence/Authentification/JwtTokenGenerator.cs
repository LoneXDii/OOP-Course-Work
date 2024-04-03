using Microsoft.Extensions.Options;
using Microsoft.IdentityModel.Tokens;
using System.IdentityModel.Tokens.Jwt;
using System.Security.Claims;
using System.Text;

namespace Server.Infrastructure.Authentification;

internal class JwtTokenGenerator : IJwtTokenGenerator
{
    private readonly JwtSettings _jwtSettings;

    public JwtTokenGenerator(IOptions<JwtSettings> jwtSettings)
    {
        _jwtSettings = jwtSettings.Value;
    }

    public string CreateToken(int id, string login, string password)
    {
        var credentials = new SigningCredentials(new SymmetricSecurityKey(Encoding.UTF8.GetBytes(_jwtSettings.Secret)),
                                                 SecurityAlgorithms.HmacSha256Signature);

        var claims = new[] {
            new Claim(JwtRegisteredClaimNames.Name, login),
            new Claim(JwtRegisteredClaimNames.Nonce, password),
            new Claim(JwtRegisteredClaimNames.Jti, Guid.NewGuid().ToString()),
            new Claim(JwtRegisteredClaimNames.Sub, id.ToString())
        };

        var token = new JwtSecurityToken(issuer: _jwtSettings.Issuer, expires: DateTime.Now.AddMinutes(_jwtSettings.ExpiryMinutes),
                                         claims: claims, signingCredentials: credentials);

        return new JwtSecurityTokenHandler().WriteToken(token);
    }
}
