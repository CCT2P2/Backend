// Controllers/ControllersCommunityController.cs
using Microsoft.AspNetCore.Mvc;
using Gnuf.Models;
using Gnuf.Models.Community;
using Microsoft.EntityFrameworkCore;

namespace Gnuf.Controllers;

[ApiController]
[Route("api/[controller]")]
public class CommunityController : ControllerBase
{
    private readonly GnufContext _context;

    public CommunityController(GnufContext context)
    {
        _context = context;
    }

    // 4.3.1 Create community
    [HttpPost("create")]
    public async Task<ActionResult> CreateCommunity([FromBody] CommunityStructure community)
    {
        if (_context.Community.Any(c => c.Name == community.Name))
        {
            return BadRequest("Community already exists");
        }

        _context.Community.Add(community);
        await _context.SaveChangesAsync();
        return Ok(new { community.Id });
    }

    // 4.3.2 Get community details
    [HttpGet("details/{community_id}")]
    public async Task<ActionResult> GetCommunityDetails(int community_id)
    {
        var community = await _context.Community
            .Include(c => c.tags)
            .Include(c => c.Posts)
            .FirstOrDefaultAsync(c => c.Id == community_id);

        if (community == null)
        {
            return NotFound();
        }

        var response = new
        {
            id = community.Id,
            name = community.Name,
            description = community.Description,
            img_path = community.img_path,
            member_count = community.member_count,
            tags = community.tags.Select(t => t.Id),
            post_ids = community.Posts.Select(p => p.Id)
        };

        return Ok(response);
    }

    // 4.3.3 Update community (user)
    [HttpPut("update/details/{community_id}")]
    public async Task<ActionResult> UpdateCommunityUser(int community_id, [FromBody] CommunityStructure update)
    {
        var community = await _context.Community.FindAsync(community_id);
        if (community == null)
        {
            return NotFound();
        }

        community.Description = update.Description;
        community.img_path = update.img_path;

        await _context.SaveChangesAsync();
        return Ok();
    }

    // 4.3.4 Update community (backend)
    [HttpPut("update/backend/{community_id}")]
    public async Task<ActionResult> UpdateCommunityBackend(int community_id, [FromBody] BackendUpdateDto update)
    {
        var community = await _context.Community
            .Include(c => c.tags)
            .Include(c => c.Posts)
            .FirstOrDefaultAsync(c => c.Id == community_id);

        if (community == null)
        {
            return NotFound();
        }

        community.member_count = update.member_count;

        community.tags.Clear();
        foreach (var tagId in update.TAGS)
        {
            var tag = await _context.Tags.FindAsync(tagId);
            if (tag != null)
                community.tags.Add(tag);
        }

        community.Posts.Clear();
        foreach (var postId in update.POST_IDs)
        {
            var post = await _context.Posts.FindAsync(postId);
            if (post != null)
                community.Posts.Add(post);
        }

        await _context.SaveChangesAsync();
        return Ok();
    }

    // 4.3.5 Delete community
    [HttpDelete("remove/{community_id}")]
    public async Task<ActionResult> DeleteCommunity(int community_id)
    {
        var community = await _context.Community.FindAsync(community_id);
        if (community == null)
        {
            return NotFound();
        }

        _context.Community.Remove(community);
        await _context.SaveChangesAsync();
        return Ok();
    }
}
