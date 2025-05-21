using Microsoft.AspNetCore.Mvc;
using Microsoft.EntityFrameworkCore;
using Gnuf.Models;
using gnufv2.Extensions;
using Microsoft.AspNetCore.Authorization;

namespace Gnuf.Controllers
{
    [ApiController]
    [Route("api/post")]
    [Authorize]
    public class PostInteractionsController : ControllerBase
    {
        private readonly GnufContext _context;

        public PostInteractionsController(GnufContext context)
        {
            _context = context;
        }

        // 4.5.1 Like / Dislike

        [HttpPut("vote/{post_id}")]
        public async Task<ActionResult> VoteOnPost(int post_id, [FromBody] Gnuf.Models.Interactions.VoteRequest vote)
        {
            var user_id = vote.UserID;
            
            if (!User.MatchesId(user_id.ToString()) && !User.IsInRole("Admin")) return Unauthorized();

            
            var voteType = vote.VoteType.ToLower();

            var interaction = await _context.Users.FirstOrDefaultAsync(x => x.UserId == user_id);

            if (interaction == null)
            {
                return NotFound("User not found.");
            }

            var post = await _context.Post.FirstOrDefaultAsync(x => x.PostID == post_id);
            if (post == null)
            {
                return NotFound("Post not found.");
            }

            interaction.LikeId ??= "";
            interaction.DislikeId ??= "";

            bool alreadyLiked = interaction.LikeId.Contains($"{post_id},");
            bool alreadyDisliked = interaction.DislikeId.Contains($"{post_id},");

            if (voteType == "none")
            {
                if (alreadyLiked)
                {
                    post.likes--;
                    interaction.LikeId = interaction.LikeId.Replace($"{post_id},", "");
                }
                else if (alreadyDisliked)
                {
                    post.dislikes--;
                    interaction.DislikeId = interaction.DislikeId.Replace($"{post_id},", "");
                }
            }
            else if (voteType == "like")
            {
                if (!alreadyLiked)
                {
                    post.likes++;

                    if (alreadyDisliked)
                    {
                        post.dislikes--;
                        interaction.DislikeId = interaction.DislikeId.Replace($"{post_id},", "");
                    }

                    interaction.LikeId += $"{post_id},";
                }
            }
            else if (voteType == "dislike")
            {
                if (!alreadyDisliked)
                {
                    post.dislikes++;

                    if (alreadyLiked)
                    {
                        post.likes--;
                        interaction.LikeId = interaction.LikeId.Replace($"{post_id},", "");
                    }

                    interaction.DislikeId += $"{post_id},";
                }
            }
            else
            {
                return BadRequest("Invalid vote type."); //not sure how the rest handles this, but not my problem
            }

            post.likes = Math.Max(0, post.likes);
            post.dislikes = Math.Max(0, post.dislikes);

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
