using System.Collections.Generic;
using System.ComponentModel.DataAnnotations;
using System.ComponentModel.DataAnnotations.Schema;

namespace MyNotes.Entities
{
    [Table("EverNoteUsers")]
    public class EverNoteUser : MyEntityBase
    {
        [StringLength(25)]
        public string Name { get; set; }

        [Required, StringLength(25)]
        public string Surname { get; set; }

        [Required, StringLength(70)]
        public string Email { get; set; }

        [Required, StringLength(50)]
        public string Password { get; set; }
        public string Username { get; set; }
        public bool IsActive { get; set; }
        public bool IsAdmin { get; set; }

        [Required]
        public bool ActiveGuid { get; set; }

        public virtual List<Note> Notes { get; set; }

        public virtual List<Comment> Comments { get; set; }

        public virtual List<Liked> Likes { get; set; }

    }
}
