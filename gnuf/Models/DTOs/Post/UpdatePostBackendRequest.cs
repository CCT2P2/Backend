
namespace Gnuf.Models.DTOs.Post
{
    public class UpdatePostBackendRequest
    {
        public required int CommentCnt { get; set; }
        public required int Likes { get; set; }
        public required int Dislikes { get; set; }
        public required List<int> Comments { get; set; }
    }
}
