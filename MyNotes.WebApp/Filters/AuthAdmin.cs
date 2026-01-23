using MyNotes.Entities;
using System.Web.Mvc;

namespace MyNotes.WebApp.Filters
{
    public class AuthAdmin : FilterAttribute, IAuthorizationFilter
    {
        public void OnAuthorization(AuthorizationContext filterContext)
        {
            var session = filterContext.HttpContext.Session["login"];
            var user = filterContext.HttpContext.Session["login"] as EverNoteUser;


            if (session != null && user.IsAdmin == false)
            {
                filterContext.Result = new RedirectResult("/Home/AccessDenied");
            }
        }
    }
}