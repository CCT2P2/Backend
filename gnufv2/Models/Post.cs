
// Models/User.cs
using System.ComponentModel.DataAnnotations;
using System.ComponentModel.DataAnnotations.Schema;

namespace Gnuf.Models;

[Table("Post")]
public class Post
{
    [Key]
    [Column("POST_ID")]
    public int PostId { get; set; }
    
    [Column("TITLE")]
    public string Title { get; set; } = string.Empty;

    [Column("MAIN_TEXT")]
    public string MainText { get; set; } = string.Empty;
    
    [Column("AUTH_ID")]
    public int AuthorId { get; set; } // er det ikke UserID? 
    
    [Column("COM_ID")]
    public int CommunityId { get; set; } 
    
    [Column("TIMESTAMP")]
    public long Timestamp { get; set; } 
    
    [Column("LIKES")]
    public int Likes { get; set; } = 0;
    
    [Column("DISLIKES")]
    public int Dislikes { get; set; } = 0;
    
    [Column("POST_ID_REF")]
    public int? PostIdRef { get; set; }
    
    [Column("COMMENT_FLAG")]
    public bool IsComment { get; set; }
    
    [Column("COMMENT_CNT")]
    public int CommentCount { get; set; } = 0;
    
    [Column("COMMENTS")]
    public List<string>? Comments { get; set; } = new(); 
}
    
    
    
    
    

