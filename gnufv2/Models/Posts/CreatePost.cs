<<<<<<< HEAD
=======

>>>>>>> origin/main
namespace Gnuf.Models.Posts;

public class CreatePostRequest
{
    public string Title { get; set; } = string.Empty;
<<<<<<< HEAD

    public string MainText { get; set; } = string.Empty;

    public int AuthorId { get; set; }// nok userID

    public int CommunityId { get; set; }

    public int? PostIdRef { get; set; }

=======
    
    public string MainText { get; set; } = string.Empty;
    
    public int AuthorId { get; set; }// nok userID
    
    public int CommunityId { get; set; } 
    
    public int? PostIdRef { get; set; }
    
>>>>>>> origin/main
    public bool IsComment { get; set; }
}

public class CreatePostResponse
{
    public int PostId { get; set; }
<<<<<<< HEAD
}
=======
}
>>>>>>> origin/main
