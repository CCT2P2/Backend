namespace Gnuf.Models.Posts;

public class UpdatePostBackendRequest
{
    public int CommentCount { get; set; }
    public int Likes { get; set; }
    public int Dislikes { get; set; }
    public List<string> Comments { get; set; } = new();
}

