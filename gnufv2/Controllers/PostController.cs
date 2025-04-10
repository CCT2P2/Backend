// Controllers/PostController.cs
using Microsoft.AspNetCore.Mvc;
using Microsoft.EntityFrameworkCore;
using Gnuf.Models;

namespace Gnuf.Controllers;

[ApiController]
[Route("api/[controller]")]
public class PostController : ControllerBase
{
    private readonly GnufContext _context;

    public PostController(GnufContext context)
    {
        _context = context;
    }

    // 4.4.1 Create post
    [HttpPost("create")]
    public async Task<ActionResult> CreatePost([FromBody] PostStructure post)
    {
        var newPost = new PostStructure
        {
            Title = post.title,
            Maintext = post.main_text,
            Auth_id = post.auth_id,
            com_id = post.com_id,
            post_id_ref = post.post_id_ref,
            comment_flag = post.comment_flag,
            timestamp = DateTime.UtcNow,
            likes = 0,
            dislikes = 0,
            comment_count = 0
        };

        _context.Posts.Add(newPost);
        await _context.SaveChangesAsync();

        return Ok(new { post_id = newPost.Id });
    }

    // 4.4.2 Get post
    [HttpGet("view/{post_id}")]
    public async Task<ActionResult> GetPost(int post_id)
    {
        var post = await _context.Posts
            .Include(p => p.Comments)
            .FirstOrDefaultAsync(p => p.Id == post_id);

        if (post == null)
        {
            return NotFound();
        }

        var response = new
        {
            id = post.Id,
            title = post.title,
            main_text = post.main_text,
            auth_id = post.auth_id,
            com_id = post.com_id,
            timestamp = post.timestamp,
            likes = post.likes,
            dislikes = post.dislikes,
            post_id_ref = post.post_id_ref,
            comment_flag = post.comment_flag,
            comment_count = post.comment_count,
            comments = post.Comments.Select(c => c.Id)
        };

        return Ok(response);
    }

    // 4.4.3 Update post (user)
    [HttpPut("update/user/{post_id}")]
    public async Task<ActionResult> UpdatePostUser(int post_id, [FromBody] PostStructure update)
    {
        var post = await _context.Posts.FindAsync(post_id);
        if (post == null)
        {
            return NotFound();
        }

        post.title = update.title;
        post.main_text = update.main_text;

        await _context.SaveChangesAsync();
        return Ok();
    }

    // 4.4.4 Update post (backend)
    [HttpPut("update/backend/{post_id}")]
    public async Task<ActionResult> UpdatePostBackend(int post_id, [FromBody] PostBackendUpdateDto update)
    {
        var post = await _context.Posts
            .Include(p => p.Comments)
            .FirstOrDefaultAsync(p => p.Id == post_id);

        if (post == null)
        {
            return NotFound();
        }

        post.comment_count = update.COMMENT_CNT;
        post.likes = update.LIKES;
        post.dislikes = update.DISLIKES;

        post.Comments.Clear();
        foreach (var commentId in update.COMMENTS)
        {
            var comment = await _context.Comments.FindAsync(commentId);
            if (comment != null)
            {
                post.Comments.Add(comment);
            }
        }

        await _context.SaveChangesAsync();
        return Ok();
    }

    // 4.4.5 Delete post
    [HttpDelete("remove/{post_id}")]
    public async Task<ActionResult> DeletePost(int post_id)
    {
        var post = await _context.Posts.FindAsync(post_id);
        if (post == null)
        {
            return NotFound();
        }

        _context.Posts.Remove(post);
        await _context.SaveChangesAsync();
        return Ok();
    }
}

// DTO for backend post updates
public class PostBackendUpdateDto
{
    public int COMMENT_CNT { get; set; }
    public int LIKES { get; set; }
    public int DISLIKES { get; set; }
    public List<int> COMMENTS { get; set; } = new();
}
