
// Controllers/AuthController.cs
using Microsoft.AspNetCore.Mvc;
using Gnuf.Models;
using Gnuf.Models.Auth;
using Microsoft.EntityFrameworkCore;
using Konscious.Security.Cryptography;
using System.Security.Cryptography;
using System.Text;
using gnufv2.Interfaces;

namespace Gnuf.Controllers;

[ApiController]
[Route("api/[controller]")]
public class AuthController : ControllerBase
{
    private readonly GnufContext _context;
    private readonly ITokenService _tokenService;

    public AuthController(GnufContext context, ITokenService tokenService)
    {
        _context = context;
        _tokenService = tokenService;
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
            MemorySize = 65536,      // 64 MB
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

        if (user == null)
            return Unauthorized("Invalid credentials");

        var salt = Convert.FromBase64String(user.Salt ?? "");
        var token = _tokenService.GenerateJwtToken(user);

        if (!await VerifyPasswordAsync(request.Password, salt, user.Password))
            return Unauthorized("Invalid credentials");

        return Ok(new { user.UserId, user.Username, user.Email, user.ImagePath, token });
    }
}
