namespace Gnuf.Models.User
{
    public class UpdateUserProfileRequest
    {
        public string ImgPath { get; set; }
        public string Password { get; set; }
    }
    public class UpdateUserProfileResponse
    {
        public int HTTPCode = 200;
    }
}
