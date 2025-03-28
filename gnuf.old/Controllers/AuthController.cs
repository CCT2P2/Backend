using Microsoft.AspNetCore.Mvc;
using Gnuf.Models.DTOs.Auth;
using database;

/*[ApiController]
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
}*/


[ApiController]
[Route("api/auth")]
public class AuthController : ControllerBase
{
    private readonly Database _database;

    public AuthController(Database database)
    {
        _database = database;
    }

    [HttpPost("register")]
    public IActionResult Register([FromBody] RegisterRequest request)
    {
        if (string.IsNullOrWhiteSpace(request.Username) ||
            string.IsNullOrWhiteSpace(request.Password) ||
            string.IsNullOrWhiteSpace(request.Email))
        {
            return BadRequest("All fields are required.");
        }

        int? success = _database.RegisterUser(request.Username, request.Password, request.Email); // update to your actual method

        if (!success.HasValue || success.Value == 0)
        {
            return StatusCode(500, "Failed to register user.");
        }

        return StatusCode(201, $"User {request.Username} registered.");
    }
}
