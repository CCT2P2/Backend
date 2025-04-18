using System.ComponentModel.DataAnnotations;
using System.ComponentModel.DataAnnotations.Schema;

namespace Gnuf.Models
{

    [Table("USER")]
    public class InteractionStructure
    {
        [Key]
        [Column("USER_ID")]
        public int UserId { get; set; }

        [Column("LIKE_ID")]
        public string? LikeId { get; set; }

        [Column("DISLIKE_ID")]
        public string? DislikeId { get; set; }
    }

}
