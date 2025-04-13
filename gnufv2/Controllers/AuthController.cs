
// Controllers/AuthController.cs
using Microsoft.AspNetCore.Mvc;
using Gnuf.Models;
using Gnuf.Models.Auth;
using Microsoft.EntityFrameworkCore;

namespace Gnuf.Controllers;

[ApiController]
[Route("api/[controller]")]
public class AuthController : ControllerBase
{
    private readonly GnufContext _context;

    public AuthController(GnufContext context)
    {
        _context = context;
    }

    [HttpPost("register")]
    public async Task<IActionResult> Register([FromBody] RegisterRequest request)
    {

        Console.WriteLine("test");
        // Check if user exists
        if (_context.Users.Any(u => u.Username == request.Username))
            return Conflict("Username already exists");


        var user = new UserStructure
        {
            Email = request.Email,
            Username = request.Username,
            Password = BCrypt.Net.BCrypt.HashPassword(request.Password, workFactor: 12)
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

        if (user == null || !BCrypt.Net.BCrypt.Verify(request.Password, user.Password))
            return Unauthorized("Invalid credentials");

        return Ok(new { user.UserId, user.Username });
    }
}
