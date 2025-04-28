
// Models/Feedback.cs
using System.ComponentModel.DataAnnotations;
using System.ComponentModel.DataAnnotations.Schema;

namespace Gnuf.Models;

[Table("FEEDBACK")]
public class FeedbackStructure
{
    [Key]
    [Column("ID")]
    [DatabaseGenerated(DatabaseGeneratedOption.Identity)]
    public int Id { get; set; }

    [Required]
    [Column("WORKED")]
    public string Worked { get; set; } = string.Empty;

    [Required]
    [Column("DIDNT")]
    public string Didnt { get; set; } = string.Empty;

    [Column("OTHER")]
    public string? Other { get; set; }

    [Required]
    [Column("RATING")]
    public int Rating { get; set; }

    [Required]
    [Column("TIMESTAMP")]
    public int Timestamp { get; set; }
}
