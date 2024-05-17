using Microsoft.Extensions.Options;
using Microsoft.IdentityModel.Tokens;
using System.IdentityModel.Tokens.Jwt;
using System.Security.Claims;
using System.Text;

namespace Server.Infrastructure.Authentification;

internal class JwtTokenGenerator : IJwtTokenGenerator
{
    public string CreateToken(int id, string login, string password)
    {
        var claims = new[] {
            new Claim(JwtRegisteredClaimNames.Name, login),
            new Claim("Id", id.ToString())
        };

        var token = new JwtSecurityToken(
                  issuer: JwtSettings.Issuer,
                  audience: JwtSettings.Audience,
                  expires: DateTime.UtcNow.Add(TimeSpan.FromMinutes(JwtSettings.ExpiryMinutes)),
                  claims: claims,
                  signingCredentials: new SigningCredentials(JwtSettings.GetSymmetricSecurityKey(), SecurityAlgorithms.HmacSha256));

        return new JwtSecurityTokenHandler().WriteToken(token);
    }
}
