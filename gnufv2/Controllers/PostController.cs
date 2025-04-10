using Microsoft.AspNetCore.Mvc;
using Microsoft.EntityFrameworkCore;
using Gnuf.Models;
using System.Linq;
using Gnuf.Models.Posts;

namespace Gnuf.Controllers
{
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
                timestamp = DateTime.UtcNow, // Fixed timestamp handling
                likes = 0,
                dislikes = 0,
                comment_Count = 0,
                comments = "" // Ensure initialized as an empty CSV string
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
                // Return comments as CSV string
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

        // 4.4.4 Update post (backend)
        [HttpPut("update/backend/{post_id}")]
        public async Task<ActionResult> UpdatePostBackend(int post_id, [FromBody] PostStructure update)
        {
            var post = await _context.Post
                .FirstOrDefaultAsync(p => p.PostID == post_id);

            if (post == null)
            {
                return NotFound();
            }

            post.comment_Count = update.comment_Count;
            post.likes = update.likes;
            post.dislikes = update.dislikes;

            // Update comments as CSV string
            post.comments = string.Join(",", update.comments); // Join list to CSV string

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
        
        
        
        
        
        //4.4.6 Get Multiple Posts
        [HttpGet("posts")]
        public async Task<ActionResult> GetPosts([FromQuery] PostQueryParameters query)
        {
            var postsQuery = _context.Post.AsQueryable();

            if (query.CommunityId.HasValue)
                postsQuery = postsQuery.Where(p => p.com_id == query.CommunityId.Value);

            if (query.UserId.HasValue)
                postsQuery = postsQuery.Where(p => p.auth_id == query.UserId.Value);

            if (query.TimestampStart.HasValue)
                postsQuery = postsQuery.Where(p =>
                    p.timestamp >= DateTimeOffset.FromUnixTimeSeconds(query.TimestampStart.Value).UtcDateTime);

            if (query.TimestampEnd.HasValue)
                postsQuery = postsQuery.Where(p =>
                    p.timestamp <= DateTimeOffset.FromUnixTimeSeconds(query.TimestampEnd.Value).UtcDateTime);

            // Sorting
            postsQuery = query.SortBy.ToLower() switch
            {
                "likes" => query.SortOrder == "asc"
                    ? postsQuery.OrderBy(p => p.likes)
                    : postsQuery.OrderByDescending(p => p.likes),
                "comments" => query.SortOrder == "asc"
                    ? postsQuery.OrderBy(p => p.comment_Count)
                    : postsQuery.OrderByDescending(p => p.comment_Count),
                _ => query.SortOrder == "asc"
                    ? postsQuery.OrderBy(p => p.timestamp)
                    : postsQuery.OrderByDescending(p => p.timestamp),
            };

            int totalCount = await postsQuery.CountAsync();
            int limit = Math.Clamp(query.Limit, 1, 100);
            int offset = Math.Max(query.Offset, 0);

            var posts = await postsQuery
                .Skip(offset)
                .Take(limit)
                .Select(p => new
                {
                    post_id = p.PostID,
                    title = p.Title,
                    main_text = p.MainText,
                    auth_id = p.auth_id,
                    com_id = p.com_id,
                    timestamp = ((DateTimeOffset)p.timestamp).ToUnixTimeSeconds(),
                    likes = p.likes,
                    dislikes = p.dislikes,
                    post_id_ref = p.post_id_ref,
                    comment_flag = p.comment_flag,
                    comment_count = p.comment_Count
                })
                .ToListAsync();

            return Ok(new
            {
                posts = posts,
                total_count = totalCount,
                next_offset = offset + posts.Count
            });
        }

    }

}