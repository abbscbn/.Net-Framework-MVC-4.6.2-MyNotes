using MyNotes.BusinessLayer;
using MyNotes.Entities;
using MyNotes.WebApp.Filters;
using System.Web.Mvc;

namespace MyNotes.WebApp.Controllers
{
    [Exc]
    public class CommentController : Controller
    {

        CommentManager CommentManager = new CommentManager();
        NoteManager NoteManager = new NoteManager();

        public ActionResult Index()
        {
            return View();
        }

        public ActionResult List(int id)
        {
            var comments = CommentManager.List(x => x.NoteId == id);

            ViewBag.NoteId = id;

            return PartialView("_PartialComments", comments);
        }

        [Auth]
        public ActionResult Edit(int id, string text)
        {

            if (string.IsNullOrEmpty(text.Trim()))
            {

                return Json(new { Success = false, message = "Yorum Boş Bırakılamaz" });
            }

            var currentUser = Session["login"] as EverNoteUser;

            var comment = CommentManager.Find(x => x.Id == id);

            if (comment == null)
            {
                return HttpNotFound();
            }
            if (comment.Owner.Id != currentUser.Id)
            {
                return new HttpUnauthorizedResult();
            }


            comment.Text = text;
            CommentManager.Update(comment);



            return Json(new { success = true });
        }

        [Auth]
        public ActionResult Create(int NoteId, string Text)
        {

            if (string.IsNullOrEmpty(Text.Trim()))
            {

                return Json(new { Success = false, message = "Yorum Boş Bırakılamaz" });
            }

            var currentUser = Session["login"] as EverNoteUser;

            if (currentUser == null)
            {
                return new HttpUnauthorizedResult();
            }


            var Note = NoteManager.Find(x => x.Id == NoteId);

            if (Note == null)
            {
                return HttpNotFound();
            }

            Comment comment = new Comment()
            {
                Text = Text,
                Owner = currentUser,
                Note = Note
            };


            if (CommentManager.Insert(comment) > 0)
            {
                return Json(new { success = true, noteId = comment.Note.Id });
            }

            return Json(new { success = false });


        }

        [Auth]
        public ActionResult Delete(int id)
        {
            var currentUser = Session["login"] as EverNoteUser;
            var comment = CommentManager.Find(x => x.Id == id);

            int note = comment.Note.Id;

            if (comment == null)
            {
                return HttpNotFound();
            }
            if (comment.Owner.Id != currentUser.Id)
            {
                return new HttpUnauthorizedResult();
            }
            if (CommentManager.Delete(comment) > 0)
            {
                return Json(new { success = true, noteId = note });
            }
            else
            {
                return Json(new { success = false });
            }
        }
    }
}