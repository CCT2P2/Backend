// Controllers/AuthController.cs

using System.Security.Claims;
using System.Security.Cryptography;
using System.Text;
using Gnuf.Models;
using Gnuf.Models.Auth;
using gnufv2.Interfaces;
using Konscious.Security.Cryptography;
using Microsoft.AspNetCore.Mvc;
using Microsoft.EntityFrameworkCore;

namespace Gnuf.Controllers;

[ApiController]
[Route("api/[controller]")]
public class AuthController : ControllerBase
{
    private readonly IConfiguration _configuration;
    private readonly GnufContext _context;
    private readonly ITokenService _tokenService;

    public AuthController(GnufContext context, ITokenService tokenService, IConfiguration configuration)
    {
        _context = context;
        _tokenService = tokenService;
        _configuration = configuration;
    }

    // Helper: Generate random salt
    private byte[] GenerateSalt(int length = 16)
    {
        using var rng = RandomNumberGenerator.Create();
        var salt = new byte[length];
        rng.GetBytes(salt);
        return salt;
    }

    // Helper: Hash password using Argon2id
    private async Task<string> HashPasswordAsync(string password, byte[] salt)
    {
        var argon2 = new Argon2id(Encoding.UTF8.GetBytes(password))
        {
            Salt = salt,
            DegreeOfParallelism = 4, // threads
            MemorySize = 65536, // 64 MB
            Iterations = 4
        };

        var hash = await argon2.GetBytesAsync(32); // 256-bit hash
        return Convert.ToBase64String(hash);
    }

    // Helper: Verify password
    private async Task<bool> VerifyPasswordAsync(string password, byte[] salt, string storedHash)
    {
        var computedHash = await HashPasswordAsync(password, salt);
        return storedHash == computedHash;
    }

    // Helper: Sets a refresh token in cookies
    private void SetRefreshToken(string refreshToken)
    {
        var options = new CookieOptions
        {
            HttpOnly = true,
            Expires = DateTime.UtcNow.AddDays(int.Parse(_configuration["RefreshToken:ExpireDays"] ?? "7")),
            Secure = true,
            SameSite = SameSiteMode.Strict,
            Path = "/api/auth"
        };

        Response.Cookies.Append("refreshToken", refreshToken, options);
    }

    [HttpPost("register")]
    public async Task<IActionResult> Register([FromBody] RegisterRequest request)
    {
        if (_context.Users.Any(u => u.Username == request.Username))
            return Conflict("Username already exists");

        var salt = GenerateSalt();
        var passwordHash = await HashPasswordAsync(request.Password, salt);

        var user = new UserStructure
        {
            Email = request.Email,
            Username = request.Username,
            Password = passwordHash,
            Salt = Convert.ToBase64String(salt) // Store salt in DB (as base64 string)
        };

        _context.Users.Add(user);
        await _context.SaveChangesAsync();

        return Ok(new { user.UserId });
    }

    [HttpPost("login")]
    public async Task<IActionResult> Login([FromBody] LoginRequest request)
    {
        var user = await _context.Users
            .FirstOrDefaultAsync(u => u.Username == request.Username);

        if (user == null) return Unauthorized("Invalid credentials");

        var salt = Convert.FromBase64String(user.Salt ?? "");

        if (!await VerifyPasswordAsync(request.Password, salt, user.Password))
            return Unauthorized("Invalid credentials");

        var accessToken = _tokenService.GenerateJwtAccessToken(user);
        var refreshToken = _tokenService.GenerateJwtRefreshToken(user);

        SetRefreshToken(refreshToken);

        return Ok(new { user.UserId, user.Username, user.Email, user.ImagePath, accessToken });
    }

    [HttpPost("refresh")]
    public async Task<IActionResult> Refresh()
    {
        var refreshToken = Request.Cookies["refreshToken"];
        var accessToken = Request.Headers["Authorization"].ToString().Replace("Bearer ", "");

        // check if there even is a token
        if (string.IsNullOrWhiteSpace(accessToken) || string.IsNullOrWhiteSpace(refreshToken))
            return BadRequest("Both access token and refresh token are required");

        // check if access tokens is authentic. if so store claims in principal
        ClaimsPrincipal accessTokenPrincipal;
        ClaimsPrincipal refreshTokenPrincipal;
        try
        {
            accessTokenPrincipal = _tokenService.ValidateJwtToken(accessToken, false);
            refreshTokenPrincipal = _tokenService.ValidateJwtToken(accessToken, true);
        }
        catch
        {
            return Unauthorized("Invalid token(s)");
        }

        // gets user id claim from both, and checks if they are identical.
        // then see if it can get parsed as an int, and store the user id if it succeeds
        var accessTokenUserId = accessTokenPrincipal.FindFirst(ClaimTypes.NameIdentifier)?.Value;
        var refreshTokenUserId = refreshTokenPrincipal.FindFirst(ClaimTypes.NameIdentifier)?.Value;

        if (accessTokenUserId != refreshTokenUserId || !int.TryParse(accessTokenUserId, out var userId))
            return Unauthorized("Token mismatch or invalid user id");


        var user = await _context.Users.FirstOrDefaultAsync(user =>
            user.UserId == userId);

        if (user == null) return Unauthorized("User not found");

        var newAccessToken = _tokenService.GenerateJwtAccessToken(user);
        var newRefreshToken = _tokenService.GenerateJwtRefreshToken(user);

        SetRefreshToken(newRefreshToken);

        return Ok(new { accessToken = newAccessToken });
    }
}