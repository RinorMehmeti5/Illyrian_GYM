using System.IdentityModel.Tokens.Jwt;
using System.Security.Claims;
using System.Text;
using Illyrian.Domain.Configuration;
using Illyrian.Domain.Entities;
using Microsoft.Extensions.Options;
using Microsoft.IdentityModel.Tokens;

namespace Illyrian.Domain.Services.Auth;

public class JwtTokenService : ITokenService
{
    private readonly JwtSettings _jwtSettings;

    public JwtTokenService(IOptions<JwtSettings> jwtSettings)
    {
        _jwtSettings = jwtSettings.Value;
    }

    public Task<(string Token, DateTime Expiration)> GenerateTokenAsync(Entities.User user, IList<string> roles)
    {
        var authClaims = new List<Claim>
        {
            new(ClaimTypes.Name, user.UserName ?? string.Empty),
            new(ClaimTypes.Email, user.Email ?? string.Empty),
            new(ClaimTypes.NameIdentifier, user.Id),
            new(JwtRegisteredClaimNames.Jti, Guid.NewGuid().ToString()),
        };

        foreach (var role in roles)
        {
            authClaims.Add(new Claim(ClaimTypes.Role, role));
        }

        var authSigningKey = new SymmetricSecurityKey(Encoding.UTF8.GetBytes(_jwtSettings.Secret));
        var expiration = DateTime.Now.AddHours(_jwtSettings.ExpirationInHours);

        var token = new JwtSecurityToken(
            issuer: _jwtSettings.ValidIssuer,
            audience: _jwtSettings.ValidAudience,
            expires: expiration,
            claims: authClaims,
            signingCredentials: new SigningCredentials(authSigningKey, SecurityAlgorithms.HmacSha256)
        );

        var tokenString = new JwtSecurityTokenHandler().WriteToken(token);
        return Task.FromResult((tokenString, expiration));
    }
}
