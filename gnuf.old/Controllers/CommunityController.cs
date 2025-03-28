using Microsoft.AspNetCore.Mvc;
using Gnuf.Models.DTOs.Community;

[ApiController]
[Route("api/community")]
public class CommunityController : ControllerBase
{
    [HttpPost("create")]
    public IActionResult CreateCommunity([FromBody] CreateCommunityRequest request)
    {
        return Ok("Community created");
    }

    [HttpGet("details/{community_id}")]
    public IActionResult GetCommunity(int community_id) => Ok($"Details for community {community_id}");

    [HttpPut("update/details/{community_id}")]
    public IActionResult UpdateCommunity(int community_id, [FromBody] UpdateCommunityDetailsRequest request)
    {
        return Ok($"Updated community {community_id} (user)");
    }

    [HttpPut("update/backend/{community_id}")]
    public IActionResult UpdateCommunityBackend(int community_id, [FromBody] UpdateCommunityBackendRequest request)
    {
        return Ok($"Updated community {community_id} (backend)");
    }

    [HttpDelete("remove/{community_id}")]
    public IActionResult DeleteCommunity(int community_id) => Ok($"Deleted community {community_id}");
}
