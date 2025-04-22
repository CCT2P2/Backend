using System.Security.Claims;
using Gnuf.Models;
using Gnuf.Models.Posts;
using gnufv2.Extensions;
using Microsoft.AspNetCore.Authorization;
using Microsoft.AspNetCore.Mvc;
using Microsoft.EntityFrameworkCore;

namespace Gnuf.Controllers;

[ApiController]
[Route("api/[controller]")]
[Authorize]
public class PostController : ControllerBase
{
    private readonly GnufContext _context;

    public PostController(GnufContext context)
    {
        _context = context;
    }

    // helper function for getting vote state
    private static string GetVoteState(string postIdString, string likeIdString, string dislikeIdString)
    {
        var likeIds = likeIdString?.Split(',') ?? [];
        var dislikeIds = dislikeIdString?.Split(',') ?? [];

        if (likeIds.Any(id => id == postIdString)) return "like";
        if (dislikeIds.Any(id => id == postIdString)) return "dislike";
        return "none";
    }

    // 4.4.1 Create post
    [HttpPost("create")]
    public async Task<ActionResult> CreatePost([FromBody] PostStructure post)
    {
        // checks if the user id in the token claims it matches the auth_id in the request
        if (!User.MatchesId(post.auth_id.ToString())) return Unauthorized();

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

        // this is only for comments
        if (newPost.comment_flag)
        {
            var parentPost = await _context.Post.FindAsync(newPost.post_id_ref);
            if (parentPost == null)
            {
                return NotFound("Parent post not found");
            }

            parentPost.comments += string.IsNullOrWhiteSpace(parentPost.comments)
                ? $"{newPost.PostID}"
                : $",{newPost.PostID}";

            parentPost.comment_Count++;
        }

        await _context.SaveChangesAsync();

        return Ok(new { post_id = newPost.PostID });
    }

    // 4.4.2 Get post
    [HttpGet("view/{post_id}")]
    public async Task<ActionResult> GetPost(int post_id)
    {
        var userId = User.FindFirst(ClaimTypes.NameIdentifier)?.Value;
        var user = await _context.Users.FindAsync(Convert.ToInt32(userId));

        // using LINQ instead of with methods here, cus doing two joins like that is crazy
        var matchingPost = await (from post in _context.Post
                                  join author in _context.Users on post.auth_id equals author.UserId
                                  join community in _context.Community on post.com_id equals community.CommunityID
                                  where post.PostID == post_id
                                  select new
                                  {
                                      id = post.PostID,
                                      title = post.Title,
                                      main_text = post.MainText,
                                      post.timestamp,
                                      post.likes,
                                      post.dislikes,
                                      post.post_id_ref,
                                      post.comment_flag,
                                      post.Img,
                                      comment_count = post.comment_Count,
                                      // Return comments as CSV string
                                      post.comments,
                                      VoteState = GetVoteState(post_id.ToString(), user.LikeId, user.DislikeId),
                                      author = new
                                      {
                                          post.auth_id,
                                          author.Username,
                                          author.ImagePath,
                                          author.IsAdmin,
                                      },
                                      community = new
                                      {
                                          post.com_id,
                                          community.Name,
                                      }
                                  }).FirstOrDefaultAsync();

        if (matchingPost == null)
        {
            return NotFound();
        }

        return Ok(matchingPost);
    }

    // 4.4.3 Update post (user)
    [HttpPut("update/user/{post_id}")]
    public async Task<ActionResult> UpdatePostUser(int post_id, [FromBody] PostStructure update)
    {
        var post = await _context.Post.FindAsync(post_id);
        if (post == null) return NotFound();

        // checks if user id matches the auth_id in the request. If the user is an admin they can edit anyway
        if (!User.MatchesId(post.auth_id.ToString()) && !User.IsInRole("Admin")) return Unauthorized();

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

        if (post == null) return NotFound();

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
        if (post == null) return NotFound();

        // checks if user id matches the auth_id in the request. If the user is an admin they can delete anyway
        if (!User.MatchesId(post.auth_id.ToString()) && !User.IsInRole("Admin")) return Unauthorized();

        _context.Post.Remove(post);
        await _context.SaveChangesAsync();
        return Ok();
    }

    //4.4.6 Get Multiple Posts
    [HttpGet("posts")]
    public async Task<ActionResult> GetPosts([FromQuery] PostQueryParameters query)
    {
        DateTime? timestampStart = null;
        DateTime? timestampEnd = null;

        try
        {
            if (query.TimestampStart.HasValue)
                timestampStart = DateTimeOffset.FromUnixTimeSeconds(query.TimestampStart.Value).UtcDateTime;

            if (query.TimestampEnd.HasValue)
                timestampEnd = DateTimeOffset.FromUnixTimeSeconds(query.TimestampEnd.Value).UtcDateTime;
        }
        catch (ArgumentOutOfRangeException ex)
        {
            // return a 400 Bad Request instead of crashing the whole request
            return BadRequest("Invalid Unix timestamp provided.");
        }

        var postsQuery = _context.Post.AsQueryable();

        postsQuery = postsQuery.Where(p => p.comment_flag == query.GetComments || !p.comment_flag == query.GetPosts);

        if (query.ParentPostId.HasValue)
        {
            postsQuery = postsQuery.Where(p => p.post_id_ref == query.ParentPostId.Value);
        }

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
                : postsQuery.OrderByDescending(p => p.timestamp)
        };

        var totalCount = await postsQuery.CountAsync();
        var limit = Math.Clamp(query.Limit, 1, 100);
        var offset = Math.Max(query.Offset, 0);

        var posts = await postsQuery
            .Join(
                _context.Users,
                post => post.auth_id,
                author => author.UserId,
                (post, author) => new
                {
                    // Post properties
                    post_id = post.PostID,
                    title = post.Title,
                    main_text = post.MainText,
                    post.auth_id,
                    post.com_id,
                    post.timestamp,
                    post.likes,
                    post.dislikes,
                    post.post_id_ref,
                    post.comment_flag,
                    comment_count = post.comment_Count,
                    author = new
                    {
                        author.Username,
                        author.ImagePath,
                        author.IsAdmin,
                    },
                })
            .Skip(offset)
            .Take(limit)
            .ToListAsync();


        return Ok(new
        {
            posts,
            total_count = totalCount,
            next_offset = offset + posts.Count
        });
    }

    [HttpGet("posts/by-ids")]
    public async Task<ActionResult> GetPostsByIds([FromQuery] string ids)
    {
        if (string.IsNullOrWhiteSpace(ids))
            return BadRequest("No post IDs provided.");

        var postIdList = ids
            .Split(',')
            .Select(idStr => int.TryParse(idStr.Trim(), out var id) ? id : (int?)null)
            .Where(id => id.HasValue)
            .Select(id => id.Value)
            .ToList();

        if (!postIdList.Any())
            return BadRequest("Invalid post IDs.");

        var posts = await _context.Post
            .Where(p => postIdList.Contains(p.PostID))
            .Select(p => new
            {
                post_id = p.PostID,
                title = p.Title,
                main_text = p.MainText,
                p.auth_id,
                p.com_id,
                timestamp = ((DateTimeOffset)p.timestamp).ToUnixTimeSeconds(),
                p.likes,
                p.dislikes,
                p.post_id_ref,
                p.comment_flag,
                comment_count = p.comment_Count
            })
            .OrderByDescending(post => post.timestamp)
            .ToListAsync();

        return Ok(posts);
    }
}
