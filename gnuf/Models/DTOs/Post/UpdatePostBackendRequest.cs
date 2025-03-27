
namespace Gnuf.Models.DTOs.Post
{
    public class UpdatePostBackendRequest
    {
        public int CommentCnt { get; set; }
        public int Likes { get; set; }
        public int Dislikes { get; set; }
        public List<int> Comments { get; set; }
    }
}
