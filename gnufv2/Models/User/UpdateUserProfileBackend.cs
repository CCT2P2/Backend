namespace Gnuf.Models.User
{
    public class UpdateUserProfileBackendRequest
    {
        public string? CommunityIDs { get; set; }
        public string? PostIDs { get; set; }
        public string? Tags { get; set; }
    }
}
