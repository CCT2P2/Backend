using System.ComponentModel.DataAnnotations;

namespace Gnuf.Models.Community;
public class commQueryParameters
{
    [Required]
    public int Id { get; set; }
    
    public int? MemberCount { get; set; }
    public string? Tags { get; set; }
    public string? PostID { get; set; }
}
