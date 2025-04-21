namespace Gnuf.Models.Posts
{
    public class PostQueryParameters
    {
        public int? CommunityId { get; set; }
        public int? UserId { get; set; }
        public long? TimestampStart { get; set; }
        public long? TimestampEnd { get; set; }
        public int Limit { get; set; } = 20;
        public int Offset { get; set; } = 0;
        public string SortBy { get; set; } = "timestamp";
        public string SortOrder { get; set; } = "desc";
        public bool GetComments { get; set; } = false;
        public int? ParentPostId { get; set; }
        public bool GetPosts { get; set; } = true;

        public string? Img { get; set; } = string.Empty;

        public string? Tags { get; set; } = string.Empty;
    }

    public class GetPostsResponse
    {
        public List<PostStructure> posts { get; set; } = new();
        public int total_count { get; set; }
        public int next_offset { get; set; }
    }
}