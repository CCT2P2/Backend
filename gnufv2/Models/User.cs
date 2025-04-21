
// Models/User.cs
using System.ComponentModel.DataAnnotations;
using System.ComponentModel.DataAnnotations.Schema;

namespace Gnuf.Models;

[Table("USER")]
public class UserStructure
{
    [Key]
    [Column("USER_ID")]
    public int UserId { get; set; }

    [Required]
    [Column("EMAIL")]
    [EmailAddress]
    public string Email { get; set; } = string.Empty;

    [Required]
    [Column("USERNAME")]
    [MaxLength(100)]
    public string Username { get; set; } = string.Empty;

    [Required]
    [Column("PASSWORD")]
    [MaxLength(1000)]
    public string Password { get; set; } = string.Empty;

    [Required]
    [Column("SALT")]
    public string Salt { get; set; } = string.Empty;

    [Column("IMG_PATH")]
    public string? ImagePath { get; set; }

    [Column("POST_IDs")]
    public string? PostIds { get; set; }

    [Column("COMMUNITY_IDs")]
    public string? CommunityIds { get; set; }

    [Required]
    [Column("ADMIN")]
    public int IsAdmin { get; set; } = 0;

    [Column("TAGS")]
    public string? Tags { get; set; }

    [Column("DISPLAY_NAME")]
    public string? DisplayName { get; set; }

    [Column("DESCRIPTION")]
    public string? Description { get; set; }

    [Column("LIKE_ID")]
    public string? LikeId { get; set; }

    [Column("DISLIKE_ID")]
    public string? DislikeId { get; set; }

    [Column("COMMENT_ID")]
    public string? CommentId { get; set; }
}
