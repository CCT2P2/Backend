using System.ComponentModel.DataAnnotations;
using System.ComponentModel.DataAnnotations.Schema;

namespace Gnuf.Models;


[Table("community")]
public class Community
{
    [Key]
    [Column ( " COMMUNITY_ID")]
    public int CommunityID { get; set; }    

    [Column("Name")] 
    public string Name { get; set; } = string.Empty;

    [Column("DESCRIPTION")]
    public string Description { get; set; } = string.Empty;
    
    [Column ("MEMBER_COUNT")]
    public int MemberCount { get; set; } = 0; 
    
    [Column ("IMG_PATH")]
    public string? Img { get; set; } 
    
    [Column ("TAGS", TypeName= "integer []" )]
    public List<int> Tags { get; set; } = new List<int>(); 
    
    [Column ("POST_ID", TypeName= "integer []")]
    public List<int> PostID  { get; set; } = new List<int>();
    
    
}