
namespace Gnuf.Models.DTOs.Community
{
    public class UpdateCommunityBackendRequest
    {
        public int MemberCount { get; set; }
        public List<int> Tags { get; set; }
        public List<int> PostIDs { get; set; }
    }
}
