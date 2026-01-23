using MyNotes.BusinessLayer;
using MyNotes.BusinessLayer.Result;
using MyNotes.Entities;
using MyNotes.WebApp.Filters;
using System.Net;
using System.Web.Mvc;

namespace MyNotes.WebApp.Controllers
{
    [Exc]
    public class UserController : Controller
    {

        EvernoteUserManager evernoteUserManager = new EvernoteUserManager();

        [AuthAdmin]
        [Auth]
        public ActionResult Index()
        {
            return View(evernoteUserManager.List());
        }
        [AuthAdmin]
        [Auth]
        public ActionResult Details(int? id)
        {
            if (id == null)
            {
                return new HttpStatusCodeResult(HttpStatusCode.BadRequest);
            }

            EverNoteUser everNoteUser = evernoteUserManager.Find(x => x.Id == id.Value);

            if (everNoteUser == null)
            {
                return HttpNotFound();
            }

            return View(everNoteUser);
        }

        [AuthAdmin]
        [Auth]
        public ActionResult Create()
        {
            return View();
        }

        [AuthAdmin]
        [Auth]
        [HttpPost]
        [ValidateAntiForgeryToken]
        public ActionResult Create(EverNoteUser everNoteUser)
        {
            ModelState.Remove("CreatedOn");
            ModelState.Remove("ModifedOn");
            ModelState.Remove("ModifiedUsername");

            // Burası incelenecek

            if (ModelState.IsValid)
            {
                BusinessLayerResult<EverNoteUser> businessLayerResult = evernoteUserManager.Insert(everNoteUser);

                if (businessLayerResult.Errors.Count > 0)
                {
                    businessLayerResult.Errors.ForEach(x => ModelState.AddModelError("", x.Message));
                    return View(everNoteUser);
                }

                return RedirectToAction("Index");
            }

            return View(everNoteUser);
        }

        [AuthAdmin]
        [Auth]
        public ActionResult Edit(int? id)
        {
            if (id == null)
            {
                return new HttpStatusCodeResult(HttpStatusCode.BadRequest);
            }
            EverNoteUser everNoteUser = evernoteUserManager.Find(x => x.Id == id.Value);

            if (everNoteUser == null)
            {
                return HttpNotFound();
            }

            return View(everNoteUser);
        }

        [AuthAdmin]
        [Auth]
        [HttpPost]
        [ValidateAntiForgeryToken]
        public ActionResult Edit(EverNoteUser everNoteUser)
        {
            ModelState.Remove("CreatedOn");
            ModelState.Remove("ModifedOn");
            ModelState.Remove("ModifiedUsername");
            ModelState.Remove("Password");

            if (ModelState.IsValid)
            {
                BusinessLayerResult<EverNoteUser> businessLayerResult = evernoteUserManager.Update(everNoteUser);

                if (businessLayerResult.Errors.Count > 0)
                {
                    businessLayerResult.Errors.ForEach(x => ModelState.AddModelError("", x.Message));
                    return View(everNoteUser);
                }

                return RedirectToAction("Index");
            }
            return View(everNoteUser);
        }

        [AuthAdmin]
        [Auth]
        public ActionResult Delete(int? id)
        {
            if (id == null)
            {
                return new HttpStatusCodeResult(HttpStatusCode.BadRequest);
            }

            EverNoteUser everNoteUser = evernoteUserManager.Find(x => x.Id == id.Value);

            if (everNoteUser == null)
            {
                return HttpNotFound();
            }


            return View(everNoteUser);
        }

        [AuthAdmin]
        [Auth]
        [HttpPost, ActionName("Delete")]
        [ValidateAntiForgeryToken]
        public ActionResult DeleteConfirmed(int id)
        {
            EverNoteUser everNoteUser = evernoteUserManager.Find(x => x.Id == id);

            BusinessLayerResult<EverNoteUser> businessLayerResult = evernoteUserManager.Delete(everNoteUser);

            if (businessLayerResult.Errors.Count > 0)
            {

                businessLayerResult.Errors.ForEach(x => ModelState.AddModelError("", x.Message));

                return View(everNoteUser);
            }
            return RedirectToAction("Index");
        }


    }
}
