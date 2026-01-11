using System.ComponentModel;
using System.ComponentModel.DataAnnotations;

namespace MyNotes.Entities.ValueObjects
{
    public class LoginViewModel
    {

        [DisplayName("Kullanıcı adı"), Required(ErrorMessage = "{0} alanı zorunludur"), StringLength(30, ErrorMessage = "{0} alanı max {1} karakterlidir")]
        public string Username { get; set; }

        [DisplayName("Şifre"), Required(ErrorMessage = "{0} alanı zorunludur"), DataType(DataType.Password), StringLength(50, ErrorMessage = "{0} alanı max {1} karakterlidir")]
        public string Password { get; set; }

    }
}