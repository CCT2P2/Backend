
namespace Gnuf.Models.Community;

public class UpdateCommunityRequest 
{
    public int MemberCount { get; set; } 
    public List<int>? Tags { get; set; }
    public List<int> PostIds { get; set; } = new();
}

public class UpdateCommunityResponse
{
    public int CommunityId { get; set; }
    public int MemberCount { get; set; } 
    public List<int> Tags { get; set; } = new();
    public List<int> PostIds { get; set; } = new();
}