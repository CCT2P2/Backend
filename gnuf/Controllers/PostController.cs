using Microsoft.AspNetCore.Mvc;

[ApiController]
[Route("api/post")]
public class PostController : ControllerBase
{
    [HttpPost("create")]
    public IActionResult CreatePost([FromBody] object request) => Ok("Post created");

    [HttpGet("view/{post_id}")]
    public IActionResult GetPost(int post_id) => Ok($"Post {post_id} details");

    [HttpPut("update/user/{post_id}")]
    public IActionResult UpdatePost(int post_id, [FromBody] object request) =>
        Ok($"Updated post {post_id} (user)");

    [HttpPut("update/backend/{post_id}")]
    public IActionResult UpdatePostBackend(int post_id, [FromBody] object request) =>
        Ok($"Updated post {post_id} (backend)");

    [HttpDelete("remove/{post_id}")]
    public IActionResult DeletePost(int post_id) => Ok($"Deleted post {post_id}");

    [HttpPut("vote/{post_id}")]
    public IActionResult VotePost(int post_id, [FromBody] object request) =>
        Ok($"Voted on post {post_id}");

    [HttpPut("comments/{post_id}")]
    public IActionResult AddComments(int post_id, [FromBody] object request) =>
        Ok($"Added comments to post {post_id}");
}

