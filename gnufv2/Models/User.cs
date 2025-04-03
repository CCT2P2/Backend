
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

    [Column("EMAIL")]
    public string Email { get; set; } = string.Empty;

    [Column("USER_NAME")]
    public string Username { get; set; } = string.Empty;

    [Column("PASSWORD")]
    public string Password { get; set; } = string.Empty;

}
