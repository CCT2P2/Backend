using Microsoft.AspNetCore.Mvc;
using Microsoft.EntityFrameworkCore;
using Gnuf.Models;

namespace Gnuf.Controllers
{
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

        // 4.5.2 Add comments (CSV-based)
        [HttpPut("comments/{post_id}")]
        public async Task<ActionResult> AddCommentsToPost(int post_id, [FromBody] PostStructure update)
        {
            var post = await _context.Post.FindAsync(post_id);
            if (post == null)
            {
                return NotFound();
            }

            if (string.IsNullOrWhiteSpace(update.comments))
            {
                return BadRequest("No comments provided.");
            }

            var newCommentIds = update.comments
                .Split(',', StringSplitOptions.RemoveEmptyEntries)
                .Select(id => id.Trim())
                .Where(id => !string.IsNullOrWhiteSpace(id))
                .ToList();

            var existingComments = string.IsNullOrWhiteSpace(post.comments)
                ? new List<string>()
                : post.comments.Split(',', StringSplitOptions.RemoveEmptyEntries)
                               .Select(id => id.Trim())
                               .ToList();

            foreach (var commentId in newCommentIds)
            {
                if (!existingComments.Contains(commentId))
                {
                    existingComments.Add(commentId);
                    post.comment_Count += 1;
                }
            }

            post.comments = string.Join(",", existingComments);

            await _context.SaveChangesAsync();
            return Ok();
        }
    }
}
