using System.IdentityModel.Tokens.Jwt;
using System.Security.Claims;

namespace API.Services;

public interface ITokenService
{
    public JwtSecurityToken CreateJwtToken(ClaimsIdentity claimsIdentity);
    public ClaimsIdentity GetClaimsIdentity(string email, string role);
}