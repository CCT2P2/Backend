
namespace Gnuf.Models.Community;

public class CreateCommunityRequest
{
    public string Name { get; set; } = string.Empty;
    
    public string Description { get; set; }= string.Empty;
    
    public string? Img { get; set; } 
    
    public List<int> Tags { get; set; } = new();
    
}

public class CreateCommunityResponse 
{
    public int CommunityId { get; set; }
    
}