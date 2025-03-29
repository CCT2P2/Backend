using System.ComponentModel.DataAnnotations;

namespace Gnuf.Models.Community;

public class CreateCommunityRequest
{
    public string Name { get; set; } = string.Empty;
    
    public string Description { get; set; }= string.Empty;
    
    public string? Img { get; set; } 
    
    public List<int> Tags { get; set; } = new List<int>();
    
}

public class CommunityResponse 
{
    public int CommunityId { get; set; }
    public string Name { get; set; } = string.Empty;
    public string Description { get; set; } = string.Empty;
    public int MemberCount { get; set; } = 0; 
    public string? Img { get; set; } 
    public List<int> Tags { get; set; } = new List<int>();
}