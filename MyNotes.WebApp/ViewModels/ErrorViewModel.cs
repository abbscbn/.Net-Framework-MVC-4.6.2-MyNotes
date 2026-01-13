using MyNotes.Entities.Messages;

namespace MyNotes.WebApp.ViewModels
{
    public class ErrorViewModel : NotifyViewModalBase<ErrorMessageObj>
    {
        public ErrorViewModel()
        {
            Title = "Hata Oluştu";
        }
    }
}