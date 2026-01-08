using System.ComponentModel;
using System.ComponentModel.DataAnnotations;

namespace MyNotes.WebApp.ViewModels
{
    public class RegisterViewModel
    {

        [DisplayName("Kullanıcı Adı"), Required(ErrorMessage = "{0} alanı zorunludur"), StringLength(30, ErrorMessage = "{0} alanı max {1} karakterli olmalıdır")]
        public string Username { get; set; }

        [DisplayName("Email"), Required(ErrorMessage = "{0} alanı zorunludur"), StringLength(70, ErrorMessage = "{0} alanı max {1} karakterli olmalıdır")]
        public string Email { get; set; }

        [DisplayName("Şifre"), Required(ErrorMessage = "{0} alanı zorunludur"), DataType(DataType.Password), StringLength(50, ErrorMessage = "{0} alanı max {1} karakterli olmalıdır")]
        public string Password { get; set; }

        [DisplayName("Şifre Tekrar"), Required(ErrorMessage = "{0} alanı zorunludur"), DataType(DataType.Password), StringLength(50, ErrorMessage = "{0} alanı max {1} karakterli olmalıdır"), Compare("Password", ErrorMessage = "Şifreler Uyuşmuyor")]
        public string RePassword { get; set; }
    }
}