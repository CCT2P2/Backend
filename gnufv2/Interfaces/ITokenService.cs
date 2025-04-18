using System.Security.Claims;
using Gnuf.Models;

namespace gnufv2.Interfaces;

public interface ITokenService
{
    string GenerateJwtToken(UserStructure user);
    string GenerateRefreshToken();
    ClaimsPrincipal ValidateExpiredToken(string token);
}