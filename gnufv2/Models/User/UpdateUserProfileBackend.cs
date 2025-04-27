namespace Gnuf.Models.User
{
    public class UpdateUserProfileBackendRequest
    {
        public string? CommunityIds { get; set; }
        public string? PostIds { get; set; }
        public string? Tags { get; set; }
    }
}
