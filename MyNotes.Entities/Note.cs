using System.Collections.Generic;
using System.ComponentModel.DataAnnotations;
using System.ComponentModel.DataAnnotations.Schema;

namespace MyNotes.Entities
{
    [Table("Notes")]
    public class Note : MyEntityBase
    {

        [Required, StringLength(60)]
        public string Title { get; set; }

        [Required, StringLength(2000)]
        public string Text { get; set; }

        public bool IsDraft { get; set; }
        public int LikeCount { get; set; }

        public int CategoryId { get; set; } // Category ile ilişki için yabancı anahtar

        public int OwnerId { get; set; } // EverNoteUser ile ilişki için yabancı anahtar
        public virtual Category Category { get; set; }
        public virtual EverNoteUser Owner { get; set; }
        public virtual List<Comment> Comments { get; set; }
        public virtual List<Liked> Likes { get; set; }

    }
}
