namespace Gnuf.Models.User
{
    public class UserQueryParameters
    {
        public int Id { get; set; }
        public string? CommunityIDs { get; set; }
        public string? PostIDs { get; set; }
        public string? Tags { get; set; }
        public string Action { get; set; }
    }
}
