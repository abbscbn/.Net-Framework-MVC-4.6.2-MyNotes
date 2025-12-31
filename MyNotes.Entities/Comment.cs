namespace MyNotes.Entities
{
    public class Comment : MyEntityBase
    {
        public string Text { get; set; }

        public virtual EverNoteUser Owner { get; set; }

        public int NoteId { get; set; } // Note ile ilişki için yabancı anahtar

        public virtual Note Note { get; set; }

    }
}
