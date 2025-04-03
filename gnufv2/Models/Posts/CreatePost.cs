namespace Gnuf.Models.Posts;


public class CreatePostRequest
{
    public string Title { get; set; } = string.Empty;

    public string MainText { get; set; } = string.Empty;

    public int AuthorId { get; set; }// nok userID

    public int CommunityId { get; set; }

    public int? PostIdRef { get; set; }

    public bool IsComment { get; set; }
}

public class CreatePostResponse
{
    public int PostId { get; set; }
}
