using System.ComponentModel.DataAnnotations.Schema;

namespace Notes.Models;

[Table("notes")]
public class NoteModel
{
    [Column("id")] public int Id { get; }

    [Column("title", TypeName = "varchar(20)")]
    public string Title { get; set; }

    [Column("description", TypeName = "varchar(50)")]
    public string Description { get; set; }

    [ForeignKey("user_id")]
    [Column("user_id")]
    public int UserId { get; set; }

    public virtual UserModel User { get; set; }
}