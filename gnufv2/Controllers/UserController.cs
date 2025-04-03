
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
            .Include(u => u.PostIds)
            .Include(u => u.CommunityIds)
            .Include(u => u.Tags)
            .FirstOrDefaultAsync(u => u.UserId == user_id);

        if (user == null)
        {
            return NotFound();
        }

        var result = new
        {
            id = user.UserId,
            email = user.Email,
            username = user.Username,
            img_path = user.ImagePath,
            //post_ids = user.PostIds.Select(p => Post.PostId),
            //community_ids = user.CommunityIds.Select(c => c.Id),
            //Tags = user.Tags.Select(t => t.Id),
            admin = user.IsAdmin
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

        user.ImagePath = update.ImgPath;
        user.Password = update.Password;

        await _context.SaveChangesAsync();
        return Ok();
    }


    // 4.2.4 Update User Profile (backend)
    [HttpPut("update/backend/{user_id}")]
    public async Task<ActionResult> UpdateUserBackend(int user_id, [FromBody] BackendUserUpdateDto update)
    {
        var user = await _context.Users
            .Include(u => u.PostIds)
            .Include(u => u.CommunityIds)
            .Include(u => u.Tags)
            .FirstOrDefaultAsync(u => u.UserId == user_id);

        if (user == null)
        {
            return NotFound();
        }

        user.PostIds.Clear();
        foreach (var postId in update.PostIds)
        {
            var post = await _context.PostIds.FindAsync(postId);
            if (post != null)
            {
                user.PostIds.Add(post);
            }
        }

        user.CommunityIds.Clear();
        foreach (var communityId in update.CommunityIds)
        {
            var community = await _context.Community.FindAsync(communityId);
            if (community != null)
            {
                user.CommunityIds.Add(community);
            }
        }

        user.Tags.Clear();
        foreach (var tagId in update.TagIds)
        {
            var tag = await _context.Tags.FindAsync(tagId);
            if (tag != null)
            {
                user.Tags.Add(tag);
            }
        }

        await _context.SaveChangesAsync();
        return Ok();
    }

    // DTOs
    public class UpdateUserDto
    {
        public string ImgPath { get; set; }
        public string Password { get; set; }
    }

    public class BackendUserUpdateDto
    {
        public List<int> CommunityIds { get; set; } = new();
        public List<int> PostIds { get; set; } = new();
        public List<int> TagIds { get; set; } = new();
    }
