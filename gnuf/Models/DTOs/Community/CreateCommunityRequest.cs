namespace Gnuf.Models.DTOs.Community
{
    public class CreateCommunityRequest
    {
        public required string Name { get; set; }
        public required string Description { get; set; }
        public required string ImgPath { get; set; }
        public required List<int> Tags { get; set; }
    }
}
