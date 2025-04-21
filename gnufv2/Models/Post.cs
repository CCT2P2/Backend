
using System.ComponentModel.DataAnnotations;
using System.ComponentModel.DataAnnotations.Schema;

namespace Gnuf.Models;

[Table("POSTS")]

public class PostStructure
{
    [Key]
    [Column("POST_ID")]
    public int PostID { get; set; }

    [Column("TITLE")]
    [StringLength(300)]
    public string Title { get; set; } = string.Empty;

    [Column("MAIN")]
    [StringLength(100000)]
    public string MainText { get; set; } = string.Empty;

    [ForeignKey("User")]
    [Column("AUTHOR_ID")]
    public int auth_id { get; set; }

    [ForeignKey("Community")]
    [Column("COMMUNITY_ID")]
    public int com_id { get; set; }

    [Column("TIMESTAMP")]
    public DateTime timestamp { get; set; }

    [Column("LIKES")]
    public int likes { get; set; }

    [Column("DISLIKES")]
    public int dislikes { get; set; }

    [Column("POST_ID_REF")]
    public int? post_id_ref { get; set; }

    [Column("COMMENT_FLAG")]
    public bool comment_flag { get; set; }

    [Column("COMMENT_CNT")]
    public int comment_Count { get; set; }

    [Column("COMMENTS")]
    public string? comments { get; set; } = string.Empty;

    [Column("IMG_PATH")]
    public string? Img { get; set; }

    [Column("TAGS")]
    public string? Tags { get; set; }

}
