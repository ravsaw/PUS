using System.ComponentModel.DataAnnotations;

namespace PUS.ViewModels
{
    public class RegisterViewModel
    {
        [Required(ErrorMessage = "Pole jest wymagane.")]
        [EmailAddress(ErrorMessage = "Podano niepoprawny adres email.")]
        [Display(Name = "Email")]
        public string Email { get; set; }

        [Required(ErrorMessage = "Pole jest wymagane.")]
        [StringLength(100, ErrorMessage = "{0} posiadać conajmniej {2} i maksymalnie {1} znaków.", MinimumLength = 6)]
        [DataType(DataType.Password)]
        [Display(Name = "Hasło")]
        public string Password { get; set; }

        [Required(ErrorMessage = "Pole jest wymagane.")]
        [DataType(DataType.Password)]
        [Display(Name = "Potwierdzenie Hasła")]
        [Compare("Password", ErrorMessage = "Hasło i potwiedzenie hasła nie są takie same.")]
        public string ConfirmPassword { get; set; }
    }
}
