using Newtonsoft.Json.Serialization;
using System.ComponentModel.DataAnnotations;
using System.Xml.Linq;

namespace PUS.ViewModels
{
    public class LoginViewModel
    {

        [Required(ErrorMessage = "Należy wpisać adres email")]
        [EmailAddress(ErrorMessage = "Podano niepoprawny adres email")]
        public string Email { get; set; }

        [Required(ErrorMessage = "Należy wpisać hasło")]
        [DataType(DataType.Password)]
        public string Password { get; set; }

        [Display(Name = "Pamietaj mnie na tym komputerze")]
        public bool RememberMe { get; set; }
    }
}
