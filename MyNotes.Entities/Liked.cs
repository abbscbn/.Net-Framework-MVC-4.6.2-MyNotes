using System.ComponentModel.DataAnnotations;
using System.ComponentModel.DataAnnotations.Schema;

namespace MyNotes.Entities
{
    [Table("Likes")]
    public class Liked
    {

        [Key, DatabaseGenerated(DatabaseGeneratedOption.Identity)]
        public int Id { get; set; }

        public int NoteId { get; set; } // Note ile ilişki için yabancı anahtar

        public int LikedUserId { get; set; } // EverNoteUser ile ilişki için yabancı anahtar

        public virtual Note Note { get; set; }

        public virtual EverNoteUser LikedUser { get; set; }


    }
}
