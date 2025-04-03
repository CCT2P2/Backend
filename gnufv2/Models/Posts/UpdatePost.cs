namespace Gnuf.Models.Posts;

<<<<<<< HEAD
public class UpdatePostRequest
{
    public int CommentCount { get; set; }
    public int Likes { get; set; }
    public int Dislikes { get; set; }
    public List<string> Comments { get; set; } = new();
=======
public class UpdatePostRequest 
{
    public int CommentCount { get; set; }
    public int Likes { get; set; } 
    public int Dislikes { get; set; } 
    public List<string> Comments { get; set; } = new ();
>>>>>>> origin/main
}

public class UpdatePostResponse
{
    public int PostId { get; set; }
    public int CommentCount { get; set; }
<<<<<<< HEAD
    public int Likes { get; set; }
    public int Dislikes { get; set; }
=======
    public int Likes { get; set; } 
    public int Dislikes { get; set; } 
>>>>>>> origin/main
    public List<string>? Comments { get; set; }
}
