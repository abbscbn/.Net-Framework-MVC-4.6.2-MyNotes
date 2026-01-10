using System.ComponentModel;
using System.ComponentModel.DataAnnotations;

namespace MyNotes.Entities.ValueObjects
{
    public class LoginViewModel
    {

        [DisplayName("Email"), Required(ErrorMessage = "{0} alanı zorunludur"), StringLength(70, ErrorMessage = "{0} alanı max {1} karakterlidir")]
        public string Email { get; set; }

        [DisplayName("Şifre"), Required(ErrorMessage = "{0} alanı zorunludur"), DataType(DataType.Password), StringLength(50, ErrorMessage = "{0} alanı max {1} karakterlidir")]
        public string Password { get; set; }

    }
}