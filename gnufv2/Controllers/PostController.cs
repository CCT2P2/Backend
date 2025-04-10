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
            Title = post.Title,
            MainText = post.MainText,
            auth_id = post.auth_id,
            com_id = post.com_id,
            post_id_ref = post.post_id_ref,
            comment_flag = post.comment_flag,
            timestamp = DateTime.UtcNow,  // Fixed timestamp handling
            likes = 0,
            dislikes = 0,
            comment_Count = 0,
            comments = "" // Ensure initialized
        };

        _context.Post.Add(newPost);
        await _context.SaveChangesAsync();

        return Ok(new { post_id = newPost.PostID });
    }

    // 4.4.2 Get post
    [HttpGet("view/{post_id}")]
    public async Task<ActionResult> GetPost(int post_id)
    {
        var post = await _context.Post
            .Include(p => p.comments)
            .FirstOrDefaultAsync(p => p.PostID == post_id);

        if (post == null)
        {
            return NotFound();
        }

        var response = new
        {
            id = post.PostID,
            title = post.Title,
            main_text = post.MainText,
            auth_id = post.auth_id,
            com_id = post.com_id,
            timestamp = post.timestamp,
            likes = post.likes,
            dislikes = post.dislikes,
            post_id_ref = post.post_id_ref,
            comment_flag = post.comment_flag,
            comment_count = post.comment_Count,
            comments = post.comments
        };

        return Ok(response);
    }

    // 4.4.3 Update post (user)
    [HttpPut("update/user/{post_id}")]
    public async Task<ActionResult> UpdatePostUser(int post_id, [FromBody] PostStructure update)
    {
        var post = await _context.Post.FindAsync(post_id);
        if (post == null)
        {
            return NotFound();
        }

        post.Title = update.Title;
        post.MainText = update.MainText;

        await _context.SaveChangesAsync();
        return Ok();
    }
    //TODO: fix this its not a list but a comma seperated string
    // 4.4.4 Update post (backend)
    [HttpPut("update/backend/{post_id}")]
    public async Task<ActionResult> UpdatePostBackend(int post_id, [FromBody] PostBackendUpdateDto update)
    {
        var post = await _context.Post
            .Include(p => p.comments)
            .FirstOrDefaultAsync(p => p.PostID == post_id);

        if (post == null)
        {
            return NotFound();
        }

        post.comment_Count = update.COMMENT_CNT;
        post.likes = update.LIKES;
        post.dislikes = update.DISLIKES;

        post.comments.Clear();
        foreach (var commentId in update.COMMENTS)
        {
            var comment = await _context.comments.FindAsync(commentId);
            if (comment != null)
            {
                post.comments.Add(comment);
            }
        }

        await _context.SaveChangesAsync();
        return Ok();
    }

    // 4.4.5 Delete post
    [HttpDelete("remove/{post_id}")]
    public async Task<ActionResult> DeletePost(int post_id)
    {
        var post = await _context.Post.FindAsync(post_id);
        if (post == null)
        {
            return NotFound();
        }

        _context.Post.Remove(post);
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
