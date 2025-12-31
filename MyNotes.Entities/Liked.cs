namespace MyNotes.Entities
{
    public class Liked
    {
        public int Id { get; set; }

        public int NoteId { get; set; } // Note ile ilişki için yabancı anahtar

        public int LikedUserId { get; set; } // EverNoteUser ile ilişki için yabancı anahtar

        public virtual EverNoteUser LikedUser { get; set; }

        public virtual Note Note { get; set; }
    }
}
