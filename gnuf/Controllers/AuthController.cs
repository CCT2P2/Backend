using Microsoft.AspNetCore.Mvc;
using Gnuf.Models.DTOs.Auth;
using System.Text.Json.Serialization;

[ApiController]
[Route("api/auth")]
public class AuthController : ControllerBase
{
    [HttpPost("login")]
    public IActionResult Login([FromBody] LoginRequest request)
    {
        return Ok($"Login: {request.Username}");
    }

    [HttpPost("register")]
    public IActionResult Register([FromBody] RegisterRequest request)
    {
        return Ok($"Register: {request.Username}");
    }
}
