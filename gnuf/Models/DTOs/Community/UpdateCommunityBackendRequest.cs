
namespace Gnuf.Models.DTOs.Community
{
    public class UpdateCommunityBackendRequest
    {
        public required int MemberCount { get; set; }
        public required List<int> Tags { get; set; }
        public required List<int> PostIDs { get; set; }
    }
}
