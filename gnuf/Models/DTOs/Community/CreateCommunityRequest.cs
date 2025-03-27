namespace Gnuf.Models.DTOs.Community
{
    public class CreateCommunityRequest
    {
        public string Name { get; set; }
        public string Description { get; set; }
        public string ImgPath { get; set; }
        public List<int> Tags { get; set; }
    }
}
