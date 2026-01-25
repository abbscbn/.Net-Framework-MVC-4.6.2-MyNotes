using MyNotes.BusinessLayer;
using MyNotes.Entities;
using MyNotes.WebApp.Filters;
using System.Collections.Generic;
using System.Linq;
using System.Web.Mvc;

namespace MyNotes.WebApp.Controllers
{
    [Exc]
    public class LikeController : Controller
    {
        LikeManager likeManager = new LikeManager();
        NoteManager NoteManager = new NoteManager();



        public ActionResult GetLiked(int[] ids)
        {
            var currentUser = Session["login"] as EverNoteUser;
            List<int> likedNoteIds = null;
            bool success = false;

            if (currentUser == null)
            {
                return Json(new
                {
                    success = false,
                    redirect = "Home/Login"
                }, JsonRequestBehavior.AllowGet);
            }

            if (ids != null && ids.Any())
            {
                likedNoteIds = likeManager
               .List(x => x.LikedUserId == currentUser.Id && ids.Contains(x.Note.Id))
               .Select(x => x.Note.Id)
               .ToList();
                success = true;

            }

            return Json(new
            {
                success = success,
                result = likedNoteIds
            }, JsonRequestBehavior.AllowGet);
        }


        [HttpPost]
        public ActionResult Toggle(int noteId)
        {
            var user = Session["login"] as EverNoteUser;

            if (user == null)
                return Json(new { success = false, message = "Giriş Yapmalısınız" });

            var note = NoteManager.Find(x => x.Id == noteId);

            if (note == null)
                return Json(new { success = false });

            var like = likeManager.Find(x => x.Note.Id == noteId && x.LikedUser.Id == user.Id);

            if (like != null)
            {

                // like kaldır
                likeManager.Delete(like);
                NoteManager.DecreaseLikeCount(noteId);

            }
            else
            {
                // like ekle
                likeManager.Insert(new Liked
                {
                    Note = note,
                    LikedUser = user
                });
                NoteManager.IncreaseLikeCount(noteId);


            }

            int likeCount = likeManager.Count(x => x.NoteId == noteId);


            return Json(new
            {
                success = true,
                liked = (like == null),
                likeCount = likeCount
            });
        }

    }
}