using MyNotes.DataAccessLayer.EntityFramework;
using MyNotes.Entities;
using System.Collections.Generic;
using System.Linq;

namespace MyNotes.BusinessLayer
{
    public class NoteManager
    {
        Repository<Note> repo_note = new Repository<Note>();

        public void updateModificationDate(List<Note> notes)
        {
            int day = 1;

            foreach (var note in notes)
            {
                note.ModifedOn = note.ModifedOn.AddDays(day);
                repo_note.Update(note);
                day++;
            }
        }

        public List<Note> getAllNotes()
        {
            return repo_note.List();
        }

        public IQueryable<Note> getAllNotesQueryable()
        {
            return repo_note.ListQueryable();
        }

    }
}
