namespace Gnuf.Models.Feedback
{
    public class FeedbackRequest
    {
        public string Worked { get; set; }
        public string Didnt { get; set; }
        public string Feedback { get; set; }
        public int Rating { get; set; }
        public int Timestamp { get; set; }
    }
}
