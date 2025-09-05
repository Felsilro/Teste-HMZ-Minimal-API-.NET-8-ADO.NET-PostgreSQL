using Microsoft.IdentityModel.Tokens;
using System.IdentityModel.Tokens.Jwt;
using System.Security.Claims;
using System.Text;

namespace HMZ.Application.Services;

public sealed class JwtTokenService
{
    private readonly string _issuer;
    private readonly string _audience;
    private readonly SymmetricSecurityKey _key;

    public JwtTokenService()
    {
        _issuer = Environment.GetEnvironmentVariable("JWT_ISSUER") ?? "hmz-issuer";
        _audience = Environment.GetEnvironmentVariable("JWT_AUDIENCE") ?? "hmz-audience";
        var secret = Environment.GetEnvironmentVariable("JWT_SECRET") ?? "super-secret-change-me-please-32chars";
        _key = new SymmetricSecurityKey(Encoding.UTF8.GetBytes(secret));
    }

    public (string token, DateTime expiresAt) Create(string subject, string email)
    {
        var handler = new JwtSecurityTokenHandler();
        var expires = DateTime.UtcNow.AddHours(4);

        var token = handler.CreateJwtSecurityToken(new SecurityTokenDescriptor
        {
            Issuer = _issuer,
            Audience = _audience,
            Expires = expires,
            Subject = new ClaimsIdentity(new[]
            {
                new Claim(JwtRegisteredClaimNames.Sub, subject),
                new Claim(JwtRegisteredClaimNames.Email, email),
            }),
            SigningCredentials = new SigningCredentials(_key, SecurityAlgorithms.HmacSha256)
        });

        return (handler.WriteToken(token), expires);
    }
}
