using System.Collections.Generic;

namespace MyNotes.Entities
{
    public class EverNoteUser : MyEntityBase
    {
        public string Name { get; set; }
        public string Surname { get; set; }
        public string Email { get; set; }
        public string Password { get; set; }
        public string Username { get; set; }
        public bool IsActive { get; set; }
        public bool IsAdmin { get; set; }
        public bool ActiveGuid { get; set; }

        public virtual List<Note> Notes { get; set; }

        public virtual List<Comment> Comments { get; set; }

        public virtual List<Liked> Likes { get; set; }

    }
}
