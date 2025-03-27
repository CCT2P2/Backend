
namespace Gnuf.Models.DTOs.User
{
    public class UpdateUserBackendRequest
    {
        public List<int> Communities { get; set; }
        public List<int> PostIDs { get; set; }
        public List<int> Tags { get; set; }
    }
}
