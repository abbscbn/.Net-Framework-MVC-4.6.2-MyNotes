using MyNotes.BusinessLayer;
using MyNotes.BusinessLayer.Result;
using MyNotes.Entities;
using MyNotes.WebApp.Filters;
using System;
using System.Data;
using System.Data.Entity;
using System.IO;
using System.Linq;
using System.Net;
using System.Web;
using System.Web.Mvc;

namespace MyNotes.WebApp.Controllers
{
    [Exc]
    public class NoteController : Controller
    {

        private const int PageSize = 6;

        NoteManager noteManager = new NoteManager();
        CategoryManager categoryManager = new CategoryManager();
        LikeManager likeManager = new LikeManager();
        EvernoteUserManager EvernoteUserManager = new EvernoteUserManager();

        public ActionResult Index()
        {
            EverNoteUser user = null;

            if (Session["login"] != null)
            {
                user = Session["login"] as EverNoteUser;
            }

            else
            {
                return RedirectToAction("Login", "Home");
            }


            var notes = noteManager.ListQueryable().AsNoTracking().Include(n => n.Category).Include(n => n.Owner).Where(x => x.Owner.Id == user.Id).OrderByDescending(x => x.ModifedOn);

            return View(notes.ToList());

        }


        public ActionResult MyNotes(int page = 1)
        {

            EverNoteUser currentUser = null;


            if (Session["login"] != null)
            {
                currentUser = Session["login"] as EverNoteUser;
            }

            else
            {
                return HttpNotFound();
            }

            ViewBag.currentUser = currentUser;

            var query = noteManager
               .ListQueryable()
               .AsNoTracking()
               .Where(x => x.OwnerId == currentUser.Id && x.IsDraft == false);

            int totalCount = query.Count();

            var notes = query
                .OrderByDescending(x => x.ModifedOn)
                .Skip((page - 1) * PageSize)
                .Take(PageSize)
                .ToList();

            ViewBag.CurrentPage = page;

            ViewBag.TotalPages = (int)Math.Ceiling((double)totalCount / PageSize);

            return View(notes);

        }


        public ActionResult OtherUserNotes(int id, int page = 1)
        {


            EverNoteUser foundedUser = EvernoteUserManager.Find(x => x.Id == id);

            if (foundedUser == null)
            {
                return HttpNotFound();
            }

            ViewBag.foundedUser = foundedUser.Username;

            var query = noteManager
              .ListQueryable()
              .AsNoTracking()
              .Where(x => x.OwnerId == id && x.IsDraft == false);

            int totalCount = query.Count();

            var notes = query
                .OrderByDescending(x => x.ModifedOn)
                .Skip((page - 1) * PageSize)
                .Take(PageSize)
                .ToList();

            ViewBag.CurrentPage = page;

            ViewBag.TotalPages = (int)Math.Ceiling((double)totalCount / PageSize);

            return View(notes);
        }

        [Auth]
        public ActionResult MyLikedNotes(int page = 1)
        {
            EverNoteUser currentUser = null;



            if (Session["login"] != null)
            {
                currentUser = Session["login"] as EverNoteUser;

            }

            else
            {
                return HttpNotFound();
            }

            ViewBag.currentUser = currentUser;

            var query = likeManager.ListQueryable().AsNoTracking().Include("Note").Where(x => x.LikedUserId == currentUser.Id).Select(x => x.Note).Include("Owner").Include("Category");

            int totalCount = query.Count();

            var notes = query
                .OrderByDescending(x => x.ModifedOn)
                .Skip((page - 1) * PageSize)
                .Take(PageSize)
                .ToList();

            ViewBag.CurrentPage = page;

            ViewBag.TotalPages = (int)Math.Ceiling((double)totalCount / PageSize);

            return View(notes);


        }


        public ActionResult Details(int? id)
        {
            if (id == null)
            {
                return new HttpStatusCodeResult(HttpStatusCode.BadRequest);
            }
            Note note = noteManager.Find(x => x.Id == id.Value);

            if (note == null)
            {
                return HttpNotFound();
            }
            return View(note);
        }


        [Auth]
        public ActionResult Create()
        {
            ViewBag.CategoryId = new SelectList(categoryManager.List(), "Id", "Title");

            return View();
        }

        [Auth]
        [HttpPost]
        [ValidateAntiForgeryToken]
        public ActionResult Create(Note note, HttpPostedFileBase NoteImageFilename)
        {

            ModelState.Remove("CreatedOn");
            ModelState.Remove("ModifedOn");
            ModelState.Remove("ModifiedUsername");

            if (ModelState.IsValid)
            {
                EverNoteUser user = Session["login"] as EverNoteUser;

                if (user == null)
                {
                    return RedirectToAction("Login", "Home");
                }

                if (NoteImageFilename != null &&
                    (NoteImageFilename.ContentType == "image/jpeg" ||
                    NoteImageFilename.ContentType == "image/jpg" ||
                    NoteImageFilename.ContentType == "image/png"))
                {
                    string extension = Path.GetExtension(NoteImageFilename.FileName);
                    string filename = $"user{user.Id}_{DateTime.Now:yyyyMMddHHmmss}{extension}";
                    NoteImageFilename.SaveAs(Server.MapPath($"~/Images/{filename}"));
                    note.NoteImageFilename = filename;
                }

                note.OwnerId = user.Id;

                BusinessLayerResult<Note> businessLayerResult = noteManager.Insert(note);

                if (businessLayerResult.Errors.Count > 0)
                {
                    businessLayerResult.Errors.ForEach(x => ModelState.AddModelError("", x.Message));
                    ViewBag.CategoryId = new SelectList(categoryManager.List(), "Id", "Title", note.CategoryId);
                    return View(note);
                }


                return RedirectToAction("Index");
            }

            ViewBag.CategoryId = new SelectList(categoryManager.List(), "Id", "Title", note.CategoryId);

            return View(note);
        }

        [Auth]
        public ActionResult Edit(int? id)
        {
            EverNoteUser currentUser = null;

            if (id == null)
            {
                return new HttpStatusCodeResult(HttpStatusCode.BadRequest);
            }
            Note note = noteManager.Find(x => x.Id == id.Value);

            if (note == null)
            {
                return HttpNotFound();
            }

            currentUser = Session["login"] as EverNoteUser;

            if (note.OwnerId != currentUser.Id)
            {
                return HttpNotFound();
            }

            ViewBag.CategoryId = new SelectList(categoryManager.List(), "Id", "Title", note.CategoryId);

            return View(note);
        }

        [Auth]
        [HttpPost]
        [ValidateAntiForgeryToken]
        public ActionResult Edit(Note note, HttpPostedFileBase NoteImageFilename)
        {

            EverNoteUser currentUser = Session["login"] as EverNoteUser;

            if (currentUser == null || currentUser.Id != note.OwnerId)
            {
                return HttpNotFound();
            }

            ModelState.Remove("CreatedOn");
            ModelState.Remove("ModifedOn");
            ModelState.Remove("ModifiedUsername");

            if (ModelState.IsValid)
            {

                if (NoteImageFilename != null &&
                (NoteImageFilename.ContentType == "image/jpeg" ||
                NoteImageFilename.ContentType == "image/jpg" ||
                NoteImageFilename.ContentType == "image/png"))
                {
                    string filename = $"user{currentUser.Id}_{note.Id}.{NoteImageFilename.ContentType.Split('/')[1]}";
                    NoteImageFilename.SaveAs(Server.MapPath($"~/Images/{filename}"));
                    note.NoteImageFilename = filename;
                }


                BusinessLayerResult<Note> businessLayerResult = noteManager.Update(note);

                if (businessLayerResult.Errors.Count > 0)
                {
                    businessLayerResult.Errors.ForEach(x => ModelState.AddModelError("", x.Message));
                    ViewBag.CategoryId = new SelectList(categoryManager.List(), "Id", "Title", note.CategoryId);
                    return View(note);
                }

                return RedirectToAction("MyNotes");
            }
            ViewBag.CategoryId = new SelectList(categoryManager.List(), "Id", "Title", note.CategoryId);

            return View(note);
        }

        [Auth]
        public ActionResult Delete(int? id)
        {
            if (id == null)
            {
                return new HttpStatusCodeResult(HttpStatusCode.BadRequest);
            }
            Note note = noteManager.Find(x => x.Id == id.Value);

            if (note == null)
            {
                return HttpNotFound();
            }

            return View(note);
        }

        [Auth]
        [HttpPost, ActionName("Delete")]
        [ValidateAntiForgeryToken]
        public ActionResult DeleteConfirmed(int id)
        {
            EverNoteUser currentUser = Session["login"] as EverNoteUser;


            Note note = noteManager.Find(x => x.Id == id);

            if (note == null)
            {
                return HttpNotFound();
            }

            if (currentUser == null || currentUser.Id != note.OwnerId)
            {
                return HttpNotFound();
            }


            noteManager.Delete(note);

            return RedirectToAction("MyNotes");
        }


        public ActionResult GetNoteDetail(int id)
        {
            var note = noteManager.Find(x => x.Id == id);

            if (note == null)
                return Json(null, JsonRequestBehavior.AllowGet);

            return Json(new
            {
                title = note.Title,
                owner = note.Owner.Username,
                date = note.ModifedOn.ToString("dd.MM.yyyy HH:mm"),
                text = note.Text,
                profileImage = note.Owner.ProfileImageFilename ?? "user.webp"
            }, JsonRequestBehavior.AllowGet);
        }





    }
}
