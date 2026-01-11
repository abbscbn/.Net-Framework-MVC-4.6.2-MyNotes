using MyNotes.Common;
using MyNotes.Entities;
using System.Web;

namespace MyNotes.WebApp.Inıt
{
    public class WebCommon : ICommon
    {
        public string getCurrentUsername()
        {
            if (HttpContext.Current.Session["login"] != null)
            {
                EverNoteUser user = HttpContext.Current.Session["login"] as EverNoteUser;

                return user.Username;
            }
            else
            {
                return null;
            }
        }
    }
}