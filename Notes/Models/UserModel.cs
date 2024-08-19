using System.Collections.Generic;
using System.ComponentModel.DataAnnotations.Schema;

namespace Notes.Models;

[Table("users")]
public class UserModel
{
    [Column("id")]
    public int Id { get; }
    
    [Column("username", TypeName = "varchar(50)")]
    public string Username { get; set; }
    
    [Column("password", TypeName = "varchar(200)")]
    public string Password { get; set; }
    public ICollection<NoteModel> Notes { get; set; } = new List<NoteModel>();
}