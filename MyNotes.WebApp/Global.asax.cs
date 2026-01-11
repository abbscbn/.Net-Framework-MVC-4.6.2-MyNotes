using MyNotes.Common;
using MyNotes.WebApp.Inýt;
using System.Web.Mvc;
using System.Web.Routing;

namespace MyNotes.WebApp
{
    public class MvcApplication : System.Web.HttpApplication
    {
        protected void Application_Start()
        {
            AreaRegistration.RegisterAllAreas();
            RouteConfig.RegisterRoutes(RouteTable.Routes);
            App.Common = new WebCommon();
        }
    }
}
