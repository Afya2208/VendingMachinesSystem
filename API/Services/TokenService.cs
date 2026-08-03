using System.IdentityModel.Tokens.Jwt;
using System.Security.Claims;
using System.Text;
using Microsoft.IdentityModel.Tokens;

namespace API.Services;

public class TokenService(IConfiguration configuration) : ITokenService
{
    public JwtSecurityToken CreateJwtToken(ClaimsIdentity claimsIdentity)
    {
        var now = DateTime.UtcNow;
        int hoursLifetime = int.Parse(configuration["Jwt:LifetimeHours"]);
        var expires = now.AddHours(hoursLifetime);
        return new JwtSecurityToken(
            issuer: configuration["Jwt:Issuer"],
            audience: configuration["Jwt:Audience"],
            notBefore: now,
            expires: expires,
            claims: claimsIdentity.Claims,
            signingCredentials: new SigningCredentials(
                new SymmetricSecurityKey(Encoding.UTF8.GetBytes(configuration["Jwt:Key"])),
                SecurityAlgorithms.HmacSha256)
        );
    }
    
    public ClaimsIdentity GetClaimsIdentity(string email, string role)
    {
        var claims = new List<Claim>
        {
            new Claim(ClaimTypes.Role, role),
            new Claim(ClaimTypes.Email, email),
        };
        return new ClaimsIdentity(claims, "Token", ClaimTypes.Email, ClaimTypes.Role);
    }
}