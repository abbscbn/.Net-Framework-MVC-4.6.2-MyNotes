using System.ComponentModel.DataAnnotations;
using System.ComponentModel.DataAnnotations.Schema;

namespace MyNotes.Entities
{
    [Table("Comments")]
    public class Comment : MyEntityBase
    {
        [Required, StringLength(250)]
        public string Text { get; set; }

        public int OwnerId { get; set; } // EverNoteUser ile ilişki için yabancı anahtar

        public int NoteId { get; set; } // Note ile ilişki için yabancı anahtar

        public virtual EverNoteUser Owner { get; set; }

        public virtual Note Note { get; set; }

    }
}
