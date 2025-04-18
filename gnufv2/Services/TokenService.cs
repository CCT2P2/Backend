using System.IdentityModel.Tokens.Jwt;
using System.Security.Claims;
using System.Text;
using Gnuf.Models;
using gnufv2.Interfaces;
using Microsoft.IdentityModel.Tokens;

namespace gnufv2.Services;

public class TokenService : ITokenService
{
    private readonly IConfiguration _configuration;

    public TokenService(IConfiguration configuration)
    {
        _configuration = configuration;
    }

    /// <summary>
    ///     Generates a new jwt token based on the configurations
    /// </summary>
    /// <param name="user">The user to create the token for. Is used to set claims about the user</param>
    /// <returns>The generated jwt token (no way)</returns>
    /// <exception cref="InvalidOperationException">If this happens you messed up storing the key in user-secrets</exception>
    public string GenerateJwtAccessToken(UserStructure user)
    {
        // get settings from configuration (appsettings.json and user-secrets)
        var jwtKey = _configuration["Jwt:Key"];
        var jwtIssuer = _configuration["Jwt:Issuer"];
        var jwtAudience = _configuration["Jwt:Audience"];
        var jwtExpireMinutes = int.Parse(_configuration["Jwt:ExpireMinutes"] ?? "60");

        // claims are pieces of info about the user
        var claims = new List<Claim>
        {
            new(JwtRegisteredClaimNames.Sub, user.UserId.ToString()),
            new(JwtRegisteredClaimNames.Name, user.Username),
            new(JwtRegisteredClaimNames.Jti, Guid.NewGuid().ToString()),
            new(ClaimTypes.Role, user.IsAdmin == 1 ? "Admin" : "User"),
            new(JwtRegisteredClaimNames.Email, user.Email),
            new("imagePath", user.ImagePath ?? "")
        };

        // token signing stuff. the server checks this signature later to know that the token is authentic
        var key = new SymmetricSecurityKey(
            Encoding.UTF8.GetBytes(jwtKey ?? throw new InvalidOperationException("JWT Key is not configured")));
        var creds = new SigningCredentials(key, SecurityAlgorithms.HmacSha256);

        // puts all the things together into the jwt token
        var token = new JwtSecurityToken(
            jwtIssuer,
            jwtAudience,
            claims,
            expires: DateTime.Now.AddMinutes(jwtExpireMinutes),
            signingCredentials: creds
        );

        return new JwtSecurityTokenHandler().WriteToken(token);
    }

    /// <summary>
    ///     Validates if an expired token is actually authentic based on the same configurations used by GenerateJwtToken.
    ///     Kills the user if it isnt (throws an exception)
    /// </summary>
    /// <param name="token">The token to validate</param>
    /// <param name="validateLifetime">
    ///     Whether to validate lifetime or not. For access tokens we dont want to cus they get
    ///     refreshed anyway
    /// </param>
    /// <returns>A ClaimsPrincipal containing the claims from the token. Used to make a new token with same claims</returns>
    /// <exception cref="InvalidOperationException">If this happens you messed up storing the key in user-secrets</exception>
    /// <exception cref="SecurityTokenException">
    ///     Invalid token, this is intended to happen if someone submits a fake key,
    ///     otherwise something went wrong with the config
    /// </exception>
    public ClaimsPrincipal ValidateJwtToken(string token, bool validateLifetime)
    {
        var jwtKey = _configuration["Jwt:Key"];
        var jwtIssuer = _configuration["Jwt:Issuer"];
        var jwtAudience = _configuration["Jwt:Audience"];

        // create parameters based on the same configs as the tokens are created with (except for lifetime as thats different from each token)
        var tokenValidationParameters = new TokenValidationParameters
        {
            ValidateIssuer = true,
            ValidateAudience = true,
            ValidateLifetime = validateLifetime, // dont care for access tokens cus we are gonna refresh it anyway
            ValidateIssuerSigningKey = true,
            ValidIssuer = jwtIssuer,
            ValidAudience = jwtAudience,
            IssuerSigningKey = new SymmetricSecurityKey(
                Encoding.UTF8.GetBytes(jwtKey ?? throw new InvalidOperationException("JWT Key is not configured")))
        };

        var tokenHandler = new JwtSecurityTokenHandler();

        // validates that the token matches the validation parameters. automatically throws an exception if it isnt valid
        // principal is the ClaimsPrincipal containing the claims from the token. We can use this to generate the new token
        var principal =
            tokenHandler.ValidateToken(token, tokenValidationParameters,
                out var securityToken); // tfw C# cant return multiple values, more reason why Go is superior

        if (securityToken is not JwtSecurityToken jwtSecurityToken
            || !jwtSecurityToken.Header.Alg.Equals(SecurityAlgorithms.HmacSha256,
                StringComparison.InvariantCultureIgnoreCase))
            throw new SecurityTokenException("Invalid token");

        return principal;
    }

    /// <summary>
    ///     Generates a jwt refresh token
    /// </summary>
    /// <param name="user">The user to create the token for. Used for user id</param>
    /// <returns></returns>
    /// <exception cref="InvalidOperationException">If this happens you messed up storing the key in user-secrets</exception>
    public string GenerateJwtRefreshToken(UserStructure user)
    {
        var jwtKey = _configuration["Jwt:Key"];
        var jwtIssuer = _configuration["Jwt:Issuer"];
        var jwtAudience = _configuration["Jwt:Audience"];
        var jwtExpireDays = int.Parse(_configuration["RefreshToken:ExpireDays"] ?? "7");

        var claims = new List<Claim>
        {
            new(JwtRegisteredClaimNames.Sub, user.UserId.ToString()),
            new(JwtRegisteredClaimNames.Jti, Guid.NewGuid().ToString())
        };

        var key = new SymmetricSecurityKey(
            Encoding.UTF8.GetBytes(jwtKey ?? throw new InvalidOperationException("JWT Key is not configured")));
        var creds = new SigningCredentials(key, SecurityAlgorithms.HmacSha256);

        var token = new JwtSecurityToken(
            jwtIssuer,
            jwtAudience,
            claims,
            expires: DateTime.Now.AddDays(jwtExpireDays),
            signingCredentials: creds
        );

        return new JwtSecurityTokenHandler().WriteToken(token);
    }
}