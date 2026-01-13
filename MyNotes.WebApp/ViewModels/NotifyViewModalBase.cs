using System.Collections.Generic;

namespace MyNotes.WebApp.ViewModels
{
    public class NotifyViewModalBase<T>
    {
        public List<T> Items { get; set; }

        public string Header { get; set; }

        public string Title { get; set; }

        public bool IsRedirectingUrl { get; set; }

        public string RedirectingUrl { get; set; }

        public int RedirectingTimeout { get; set; }


        public NotifyViewModalBase()
        {
            Header = "Yönlendiriliyorsunuz...";
            Title = "Varsayılan Başlık";
            IsRedirectingUrl = true;
            RedirectingUrl = "/Home/Index";
            RedirectingTimeout = 10;
        }

    }
}