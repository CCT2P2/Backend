namespace Gnuf.Models.Community;

public class UpdateCommunityBackendRequest
{
    public int MemberCount { get; set; }
    public string Tags { get; set; } = string.Empty;
    public string PostID { get; set; } = string.Empty;
}

public class UpdateCommunityBackendResponse
{
    public int CommunityId { get; set; }
    public int MemberCount { get; set; }
    public string Tags { get; set; } = string.Empty;
    public string PostID { get; set; } = string.Empty;
}
