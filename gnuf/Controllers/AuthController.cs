using Microsoft.AspNetCore.Mvc;

[ApiController]
[Route("api/auth")]
public class AuthController : ControllerBase
{
    [HttpPost("login")]
    public IActionResult Login([FromBody] object request) => Ok("Login endpoint hit");

    [HttpPost("register")]
    public IActionResult Register([FromBody] object request) => Ok("Register endpoint hit");
}

