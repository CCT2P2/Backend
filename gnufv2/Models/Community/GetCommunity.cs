
namespace Gnuf.Models.Community;



public class GETCommunityResponse 
{
    public int CommunityID { get; set; }

    public string Name { get; set; } = string.Empty;

    public string Description { get; set; } = string.Empty;

    public int MemberCount { get; set; } = 0;

    public string? Img { get; set; }

    public string Tags { get; set; } = string.Empty;
    
    public string PostID { get; set; } = string.Empty;

    
}