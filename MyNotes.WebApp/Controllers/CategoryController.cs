using System.Web.Mvc;

namespace MyNotes.WebApp.Controllers
{
    public class CategoryController : Controller
    {
        // GET: Category
        public ActionResult Index()
        {
            return View();
        }

        // Not: Aşağıdaki Select metodu, HomeController içindeki ByCategory metodunun
        //public ActionResult Select(int? id)
        //{
        //    if (id == null)
        //    {
        //        return new HttpStatusCodeResult(HttpStatusCode.BadRequest);
        //    }

        //    CategoryManager cm = new CategoryManager();
        //    var category = cm.GetCategoryById(id.Value);

        //    if (category == null)
        //    {
        //        return HttpNotFound();
        //    }

        //    TempData["categoryNotes"] = category.Notes;

        //    return RedirectToAction("Index", "Home");
        //}
    }
}