using System.Web.Mvc;

namespace MyNotes.WebApp.Controllers
{
    public class HomeController : Controller
    {
        // GET: Home
        // Yorum alanı
        public ActionResult Index()
        {
            return View();
        }
    }
}