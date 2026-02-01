using MyNotes.BusinessLayer;
using MyNotes.BusinessLayer.Result;
using MyNotes.Entities;
using MyNotes.WebApp.Filters;
using MyNotes.WebApp.Models;
using System.Net;
using System.Web.Mvc;

namespace MyNotes.WebApp.Controllers
{
    [Exc]
    public class CategoryController : Controller
    {

        CategoryManager categoryManager = new CategoryManager();

        [Auth]
        public ActionResult Index()
        {
            return View(CacheHelper.GetCategoriesFromCache());
        }


        [AuthAdmin]
        [Auth]
        public ActionResult Details(int? id)
        {
            if (id == null)
            {
                return new HttpStatusCodeResult(HttpStatusCode.BadRequest);
            }
            Category category = categoryManager.Find(x => x.Id == id.Value);

            if (category == null)
            {
                return HttpNotFound();
            }
            return View(category);
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
        public ActionResult Create(Category category)
        {

            ModelState.Remove("CreatedOn");
            ModelState.Remove("ModifedOn");
            ModelState.Remove("ModifiedUsername");


            if (ModelState.IsValid)
            {
                categoryManager.Insert(category);
                CacheHelper.RemoveCategoriesFromCache();
                return RedirectToAction("Index");
            }

            return View(category);
        }

        [AuthAdmin]
        [Auth]
        public ActionResult Edit(int? id)
        {
            if (id == null)
            {
                return new HttpStatusCodeResult(HttpStatusCode.BadRequest);
            }
            Category category = categoryManager.Find(x => x.Id == id.Value);

            if (category == null)
            {
                return HttpNotFound();
            }
            return View(category);
        }

        [AuthAdmin]
        [Auth]
        [HttpPost]
        [ValidateAntiForgeryToken]
        public ActionResult Edit(Category category)
        {
            ModelState.Remove("CreatedOn");
            ModelState.Remove("ModifedOn");
            ModelState.Remove("ModifiedUsername");

            if (ModelState.IsValid)
            {
                BusinessLayerResult<Category> res = categoryManager.Update(category);
                CacheHelper.RemoveCategoriesFromCache();

                if (res.Errors.Count > 0)
                {

                    res.Errors.ForEach(x => ModelState.AddModelError("", x.Message));
                    return View(category);

                }

                return RedirectToAction("Index");
            }

            return View(category);
        }

        [AuthAdmin]
        [Auth]
        public ActionResult Delete(int? id)
        {
            if (id == null)
            {
                return new HttpStatusCodeResult(HttpStatusCode.BadRequest);
            }
            Category category = categoryManager.Find(x => x.Id == id.Value);

            if (category == null)
            {
                return HttpNotFound();
            }
            return View(category);
        }

        [AuthAdmin]
        [Auth]
        [HttpPost, ActionName("Delete")]
        [ValidateAntiForgeryToken]
        public ActionResult DeleteConfirmed(int id)
        {
            Category category = categoryManager.Find(x => x.Id == id);
            categoryManager.Delete(category);
            CacheHelper.RemoveCategoriesFromCache();

            return RedirectToAction("Index");
        }


    }
}
