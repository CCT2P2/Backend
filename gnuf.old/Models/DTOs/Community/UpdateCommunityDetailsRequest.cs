namespace Gnuf.Models.DTOs.Community
{
    public class UpdateCommunityDetailsRequest
    {
        public required string Description { get; set; }
        public required string ImgPath { get; set; }
    }
}
