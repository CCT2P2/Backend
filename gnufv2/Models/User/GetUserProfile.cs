namespace Gnuf.Models.User
{
    public class GetUserProfileResponse
    {
        public int? Id { get; set; }
        public string Email { get; set; }
        public string Username { get; set; }
        public string? ImgPath { get; set; }
        public int? PostIds { get; set; }
        public int? CommunityIds { get; set; }
        public int? Tags { get; set; }
        public int? Admin { get; set; }
    }
}
