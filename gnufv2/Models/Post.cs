
// Models/User.cs
using System.ComponentModel.DataAnnotations;
using System.ComponentModel.DataAnnotations.Schema;

namespace Gnuf.Models;

[Table("Post")]

public class PostStructure
{
    [Key]
    [Column("POST_ID")]
    public int PostID { get; set; }

    [Column("TITLE")]
    public string Title { get; set; } = string.Empty;

    [Column("DESCRIPTION")]
    public string Description { get; set; } = string.Empty;

    [Column("MEMBER_COUNT")]
    public int MemberCount { get; set; } = 0;

    [Column("IMG_PATH")]
    public string? Img { get; set; }





}
