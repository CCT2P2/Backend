namespace Gnuf.Models.Posts;

public class UpdatePostRequest
{
    public int CommentCount { get; set; }
    public int Likes { get; set; }
    public int Dislikes { get; set; }
    public List<string> Comments { get; set; } = new();
}

public class UpdatePostResponse
{
    public int PostId { get; set; }
    public int CommentCount { get; set; }
    public int Likes { get; set; }
    public int Dislikes { get; set; }
    public List<string>? Comments { get; set; }
}
