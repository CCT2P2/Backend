using Microsoft.AspNetCore.Mvc;

[ApiController]
[Route("api/search")]
public class SearchController : ControllerBase
{
    [HttpGet("posts")]
    public IActionResult SearchPosts([FromQuery] string q) => Ok($"Search posts: {q}");

    [HttpGet("communities")]
    public IActionResult SearchCommunities([FromQuery] string q) => Ok($"Search communities: {q}");

    [HttpGet("users")]
    public IActionResult SearchUsers([FromQuery] string q) => Ok($"Search users: {q}");
}

