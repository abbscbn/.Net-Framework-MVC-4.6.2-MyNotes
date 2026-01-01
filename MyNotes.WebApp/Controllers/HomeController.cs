using System.Web.Mvc;

namespace MyNotes.WebApp.Controllers
{
    public class HomeController : Controller
    {
        // GET: Home

        public ActionResult Index()
        {
            BusinessLayer.Test test = new BusinessLayer.Test();

            return View();
        }
    }
}