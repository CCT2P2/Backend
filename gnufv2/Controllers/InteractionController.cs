
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
    public async Task<ActionResult> VoteOnPost(int post_id, [FromBody] PostStructure vote)
    {
        var post = await _context.Post.FindAsync(post_id);
        if (post == null)
        {
            return NotFound();
        }

        post.likes = vote.likes;
        post.dislikes = vote.dislikes;

        await _context.SaveChangesAsync();
        return Ok();
    }
    //TODO: fix this i dont know the problem
    // 4.5.2 Add comments
    [HttpPut("comments/{post_id}")]
    public async Task<ActionResult> AddCommentsToPost(int post_id, [FromBody] PostStructure update)
    {
        var post = await _context.Post
            .Include(p => p.comments)
            .FirstOrDefaultAsync(p => p.PostID == post_id);

        if (post == null)
        {
            return NotFound();
        }

        foreach (var commentId in update.comments)
        {
            var comment = await _context.comments.FindAsync(commentId);
            if (comment != null && !post.comments.Any(c => c.postID == commentId))
            {
                post.comments.Add(comment);
                post.comment_count+= 1;
            }
        }

        await _context.SaveChangesAsync();
        return Ok();
    }
}
