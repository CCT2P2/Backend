
// Controllers/PostInteractionsController.cs
using Microsoft.AspNetCore.Mvc;
using Microsoft.EntityFrameworkCore;
using Gnuf.Models;

namespace Gnuf.Controllers;

[ApiController]
[Route("api/post")]
public class PostInteractionsController : ControllerBase
{
    private readonly GnufContext _context;

    public PostInteractionsController(GnufContext context)
    {
        _context = context;
    }

    // 4.5.1 Like / Dislike
    [HttpPut("vote/{post_id}")]
    public async Task<ActionResult> VoteOnPost(int post_id, [FromBody] VoteDto vote)
    {
        var post = await _context.Posts.FindAsync(post_id);
        if (post == null)
        {
            return NotFound();
        }

        post.likes = vote.likes;
        post.dislikes = vote.dislikes;

        await _context.SaveChangesAsync();
        return Ok();
    }

    // 4.5.2 Add comments
    [HttpPut("comments/{post_id}")]
    public async Task<ActionResult> AddCommentsToPost(int post_id, [FromBody] CommentRefDto dto)
    {
        var post = await _context.Posts
            .Include(p => p.Comments)
            .FirstOrDefaultAsync(p => p.Id == post_id);

        if (post == null)
        {
            return NotFound();
        }

        foreach (var commentId in dto.Comments)
        {
            var comment = await _context.Comments.FindAsync(commentId);
            if (comment != null && !post.Comments.Any(c => c.Id == commentId))
            {
                post.Comments.Add(comment);
                post.comment_count++;
            }
        }

        await _context.SaveChangesAsync();
        return Ok();
    }
}

// DTOs
public class VoteDto
{
    public int likes { get; set; }
    public int dislikes { get; set; }
}

public class CommentRefDto
{
    public List<int> Comments { get; set; } = new();
}
