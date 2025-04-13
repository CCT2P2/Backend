namespace Gnuf.Models.Posts;

public class GetPostResponse
{
    
    public int id { get; set; }
    
    public string title { get; set; } = string.Empty;
    
    public string main_text { get; set; } = string.Empty;
        
    public int auth_id { get; set; }
    
    public int com_id { get; set; }
    
    public int timestamp { get; set; }
    
    public int likes { get; set; }
    
    public int dislikes { get; set; }
    
    public int? post_id_ref { get; set; }

    public bool comment_flag { get; set; }
    
    public int comment_Count { get; set; }
    
    public string? comments { get; set; } = string.Empty;
    
    public string? Img { get; set; } = string.Empty;
    
    public string? Tags { get; set; } = string.Empty;
    
    
    
}