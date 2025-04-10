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
        return Ok(new { community.CommunityID });
    }

    
    //TODO: fix
    // 4.3.2 Get community details
    [HttpGet("details/{community_id}")]
    public async Task<ActionResult> GetCommunityDetails(int community_id)
    {
        var community = await _context.Community
            .Include(c => c.Tags)
            .Include(c => c.PostID)
            .FirstOrDefaultAsync(c => c.CommunityID == community_id);

        if (community == null)
        {
            return NotFound();
        }

        var response = new
        {
            id = community.CommunityID,
            name = community.Name,
            description = community.Description,
            Img_path = community.Img_path,
            MemberCount = community.MemberCount,
            Tags = community.Tags.Select(t => t.CommunityID),
            post_ids = community.PostID.Select(p => p.CommunityID)
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
        community.Img_path = update.Img_path;

        await _context.SaveChangesAsync();
        return Ok();
    }

    //TODO: fix this its not a list but a comma seperated string
    // 4.3.4 Update community (backend)
    [HttpPut("update/backend/{community_id}")]
    public async Task<ActionResult> UpdateCommunityBackend(int community_id, [FromBody] CommunityStructure update)
    {
        var community = await _context.Community
            .Include(c => c.Tags)
            .Include(c => c.PostID)
            .FirstOrDefaultAsync(c => c.CommunityID == community_id);

        if (community == null)
        {
            return NotFound();
        }

        community.MemberCount = update.MemberCount;

        community.Tags.Clear();
        foreach (var tagCommunityID in update.TAGS)
        {
            var tag = await _context.Tags.FindAsync(tagCommunityID);
            if (tag != null)
                community.Tags.Add(tag);
        }

        community.PostID.Clear();
        foreach (var postCommunityID in update.PostID)
        {
            var post = await _context.PostID.FindAsync(postCommunityID);
            if (post != null)
                community.PostID.Add(post);
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
