using Microsoft.AspNetCore.Mvc;

[ApiController]
[Route("api/user")]
public class UserController : ControllerBase
{
    [HttpGet("profile/{user_id}")]
    public IActionResult GetProfile(int user_id) => Ok($"Profile for user {user_id}");

    [HttpDelete("remove/{user_id}")]
    public IActionResult DeleteUser(int user_id) => Ok($"Deleted user {user_id}");

    [HttpPut("update/user/{user_id}")]
    public IActionResult UpdateUser(int user_id, [FromBody] object request) =>
        Ok($"Updated user {user_id} (user)");

    [HttpPut("update/backend/{user_id}")]
    public IActionResult UpdateUserBackend(int user_id, [FromBody] object request) =>
        Ok($"Updated user {user_id} (backend)");
}

