using Newtonsoft.Json.Serialization;
using System.ComponentModel.DataAnnotations;
using System.Xml.Linq;

namespace PUS.ViewModels
{
    public class LoginViewModel
    {

        [Required(ErrorMessage = "Pole jest wymagane.")]
        [EmailAddress(ErrorMessage = "Podano niepoprawny adres email")]
        [Display(Name = "Email")]
        public string Email { get; set; }

        [Required(ErrorMessage = "Pole jest wymagane.")]
        [DataType(DataType.Password)]
        public string Password { get; set; }

        [Display(Name = "Pamietaj mnie na tym komputerze")]
        public bool RememberMe { get; set; }
    }
}
