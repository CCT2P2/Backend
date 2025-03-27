namespace Gnuf.Models.DTOs.Post
{
    public class CreatePostRequest
    {
        public string Title { get; set; }
        public string MainText { get; set; }
        public int AuthId { get; set; }
        public int ComId { get; set; }
        public int PostIdRef { get; set; }
        public bool CommentFlag { get; set; }
    }
}
