namespace Gnuf.Models.DTOs.Post
{
    public class CreatePostRequest
    {
        public required string Title { get; set; }
        public required string MainText { get; set; }
        public required int AuthId { get; set; }
        public required int ComId { get; set; }
        public required int PostIdRef { get; set; }
        public required bool CommentFlag { get; set; }
    }
}
