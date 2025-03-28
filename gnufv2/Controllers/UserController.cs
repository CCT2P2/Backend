
// Controllers/UserController.cs
using Microsoft.AspNetCore.Mvc;
using Microsoft.EntityFrameworkCore;
using Gnuf.Models;

namespace Gnuf.Controllers;

[ApiController]
[Route("api/user")]
public class UserController : ControllerBase
{
    private readonly GnufContext _context;

    public UserController(GnufContext context)
    {
        _context = context;
    }

    // 4.2.1 Get User Profile
    [HttpGet("profile/{user_id}")]
    public async Task<ActionResult> GetUserProfile(int user_id)
    {
        var user = await _context.Users
            .Include(u => u.Posts)
            .Include(u => u.Communities)
            .Include(u => u.tags)
            .FirstOrDefaultAsync(u => u.Id == user_id);

        if (user == null)
        {
            return NotFound();
        }

        var result = new
        {
            id = user.Id,
            email = user.email,
            username = user.username,
            img_path = user.img_path,
            post_ids = user.Posts.Select(p => p.Id),
            community_ids = user.Communities.Select(c => c.Id),
            tags = user.tags.Select(t => t.Id),
            admin = user.admin
        };

        return Ok(result);
    }

    // 4.2.2 Delete User Account
    [HttpDelete("remove/{user_id}")]
    public async Task<ActionResult> DeleteUser(int user_id)
    {
        var user = await _context.Users.FindAsync(user_id);
        if (user == null)
        {
            return NotFound();
        }

        _context.Users.Remove(user);
        await _context.SaveChangesAsync();
        return Ok();
    }

    // 4.2.3 Update User Profile (user)
    [HttpPut("update/user/{user_id}")]
    public async Task<ActionResult> UpdateUserProfile(int user_id, [FromBody] UpdateUserDto update)
    {
        var user = await _context.Users.FindAsync(user_id);
        if (user == null)
        {
            return NotFound();
        }

        user.img_path = update.img_path;
        user.password = update.password;

        await _context.SaveChangesAsync();
        return Ok();
    }

    // 4.2.4 Update User Profile (backend)
    [HttpPut("update/backend/{user_id}")]
    public async Task<ActionResult> UpdateUserBackend(int user_id, [FromBody] BackendUserUpdateDto update)
    {
        var user = await _context.Users
            .Include(u => u.Posts)
            .Include(u => u.Communities)
            .Include(u => u.tags)
            .FirstOrDefaultAsync(u => u.Id == user_id);

        if (user == null)
        {
            return NotFound();
        }

        user.Posts.Clear();
        foreach (var postId in update.POST_IDs)
        {
            var post = await _context.Posts.FindAsync(postId);
            if (post != null)
                user.Posts.Add(post);
        }

        user.Communities.Clear();
        foreach (var communityId in update.communities)
        {
            var com = await _context.Community.FindAsync(communityId);
            if (com != null)
                user.Communities.Add(com);
        }

        user.tags.Clear();
        foreach (var tagId in update.TAGS)
        {
            var tag = await _context.Tags.FindAsync(tagId);
            if (tag != null)
                user.tags.Add(tag);
        }

        await _context.SaveChangesAsync();
        return Ok();
    }
}

// DTOs
public class UpdateUserDto
{
    public string img_path { get; set; }
    public string password { get; set; }
}

public class BackendUserUpdateDto
{
    public List<int> communities { get; set; } = new();
    public List<int> POST_IDs { get; set; } = new();
    public List<int> TAGS { get; set; } = new();
}
