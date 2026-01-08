using MyNotes.BusinessLayer;
using MyNotes.WebApp.ViewModels;
using System.Linq;
using System.Net;
using System.Web.Mvc;

namespace MyNotes.WebApp.Controllers
{
    public class HomeController : Controller
    {
        // GET: Home

        public ActionResult Index()
        {
            // Bir başka controller'dan gelen notları göstermek için TempData kullanımı
            //if (TempData["categoryNotes"] != null)
            //{
            //    return View(TempData["categoryNotes"] as List<Note>);
            //}

            NoteManager nm = new NoteManager();


            // return View(nm.getAllNotesQueryable().OrderByDescending(x => x.ModifedOn).ToList());

            return View(nm.getAllNotes().OrderByDescending(x => x.ModifedOn).ToList());
        }


        public ActionResult ByCategory(int? id)
        {
            if (id == null)
            {
                return new HttpStatusCodeResult(HttpStatusCode.BadRequest);
            }

            CategoryManager cm = new CategoryManager();
            var category = cm.GetCategoryById(id.Value);

            if (category == null)
            {
                return HttpNotFound();
            }

            return View("Index", category.Notes.OrderByDescending(x => x.ModifedOn).ToList());
        }


        public ActionResult MostLiked()
        {
            NoteManager nm = new NoteManager();

            return View("Index", nm.getAllNotes().OrderByDescending(x => x.LikeCount).ToList());
        }

        public ActionResult About()
        {

            return View();
        }

        public ActionResult Login()
        {
            // Hata kontrolü

            return View();
        }

        [HttpPost]
        public ActionResult Login(LoginViewModel model)
        {
            return View();
        }

        public ActionResult Register()
        {

            return View();
        }

        [HttpPost]
        public ActionResult Register(RegisterViewModel model)
        {
            // Hata kontrolü
            // Session'a kullanıcı bilgilerini atama
            return View();
        }
    }
}