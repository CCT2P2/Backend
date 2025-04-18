using System.Security.Claims;
using Gnuf.Models;

namespace gnufv2.Interfaces;

public interface ITokenService
{
    string GenerateJwtAccessToken(UserStructure user);
    ClaimsPrincipal ValidateJwtToken(string token, bool isRefreshToken);
    string GenerateJwtRefreshToken(UserStructure user);
}