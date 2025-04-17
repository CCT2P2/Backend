using Gnuf.Models;

namespace gnufv2.Interfaces;

public interface ITokenService
{
    string GenerateJwtToken(UserStructure user);
}