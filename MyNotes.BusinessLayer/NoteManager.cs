using MyNotes.BusinessLayer.Abstract;
using MyNotes.BusinessLayer.Result;
using MyNotes.DataAccessLayer.EntityFramework;
using MyNotes.Entities;
using MyNotes.Entities.Messages;

namespace MyNotes.BusinessLayer
{
    public class NoteManager : ManagerBase<Note>
    {
        BusinessLayerResult<Note> res = new BusinessLayerResult<Note>();
        Repository<Category> repo_category = new Repository<Category>();
        Repository<EverNoteUser> repo_user = new Repository<EverNoteUser>();

        public new BusinessLayerResult<Note> Update(Note obj)
        {
            Note note = Find(x => x.Id == obj.Id);

            if (note != null)
            {
                note.Title = obj.Title.Trim();
                note.Text = obj.Text;
                note.IsDraft = obj.IsDraft;
                note.CategoryId = obj.CategoryId;

                if (base.Update(note) == 0)
                {
                    res.AddError(ErrorMessageCode.NoteCouldNotBeUpdated, "Not güncellenemedi");
                }
                else
                {
                    res.Result = note;
                }
            }
            else
            {
                res.AddError(ErrorMessageCode.NoteNotFound, "Not bulunamadı");
            }
            return res;
        }

        public new BusinessLayerResult<Note> Insert(Note note)
        {
            Note checkNote = Find(x => x.Title == note.Title.Trim() && x.OwnerId == note.OwnerId);

            if (checkNote != null)
            {
                res.AddError(ErrorMessageCode.NoteAlreadyExists, "Bu başlıkta zaten bir notunuz var. Lütfen farklı bir başlık deneyiniz.");

                return res;
            }


            Category category = repo_category.Find(x => x.Id == note.CategoryId);
            EverNoteUser everNoteUser = repo_user.Find(x => x.Id == note.OwnerId);

            note.Title = note.Title.Trim();
            note.Category = category;
            note.Owner = everNoteUser;

            int dbResult = base.Insert(note);

            if (dbResult > 0)
            {
                res.Result = note;
            }
            else
            {
                res.AddError(ErrorMessageCode.NoteCouldNotBeInserted, "Not eklenemedi");
            }

            return res;
        }

    }
}
