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
        [StringLength(100, ErrorMessage = "{0} powinno posiadać co najmniej {2} i maksymalnie {1} znaków.", MinimumLength = 6)]
        [DataType(DataType.Password)]
        [Display(Name = "Hasło")]
        public string Password { get; set; }

        [Required(ErrorMessage = "Pole jest wymagane.")]
        [DataType(DataType.Password)]
        [Display(Name = "Potwierdzenie Hasła")]
        [Compare("Password", ErrorMessage = "Hasło i potwiedzenie hasła nie są takie same.")]
        public string ConfirmPassword { get; set; }

        [Required(ErrorMessage = "Pole jest wymagane.")]
        [StringLength(60, ErrorMessage = "{0} powinno posiadać co najmniej {2} i maksymalnie {1} znaków.", MinimumLength = 1)]
        [Display(Name = "Imię")]
        public string FirstName { get; set; }

        [Required(ErrorMessage = "Pole jest wymagane.")]
        [StringLength(60, ErrorMessage = "{0} powinno posiadać co najmniej {2} i maksymalnie {1} znaków.", MinimumLength = 1)]
        [Display(Name = "Nazwisko")]
        public string LastName { get; set; }

        [Required(ErrorMessage = "Pole jest wymagane.")]
        [DataType(DataType.Date)]
        [Display(Name = "Data urodzenia")]
        public DateOnly BirthDate { get; set; }

        [Required(ErrorMessage = "Pole jest wymagane.")]
        [Phone]
        [Display(Name = "Numer telefonu")]
        public string PhoneNumber { get; set; }

        [Required(ErrorMessage = "Pole jest wymagane.")]
        [StringLength(60, ErrorMessage = "{0} powinno posiadać co najmniej {2} i maksymalnie {1} znaków.", MinimumLength = 1)]
        [Display(Name = "Ulica")]
        public string Address1 { get; set; }

        [Required(ErrorMessage = "Pole jest wymagane.")]
        [StringLength(10, ErrorMessage = "{0} powinno posiadać co najmniej {2} i maksymalnie {1} znaków.", MinimumLength = 1)]
        [Display(Name = "Nr domu / Nr mieszkania")]
        public string Address2 { get; set; }

        [Required(ErrorMessage = "Pole jest wymagane.")]
        [StringLength(60, ErrorMessage = "{0} powinno posiadać co najmniej {2} i maksymalnie {1} znaków.", MinimumLength = 1)]
        [Display(Name = "Poczta")]
        public string City { get; set; }

        [Required(ErrorMessage = "Pole jest wymagane.")]
        [StringLength(6, ErrorMessage = "{0} powinno posiadać co najmniej {2} i maksymalnie {1} znaków.", MinimumLength = 1)]
        [Display(Name = "Kod pocztowy")]
        [RegularExpression(@"^\d{2}-\d{3}$", ErrorMessage = "{0} musi mieć postać 12-345")]
        public string PostCode { get; set; }

        [Required(ErrorMessage = "Zdjęcie jest wymagane.")]
        [DataType(DataType.Upload)]
        public IFormFile ProfileImage { get; set; }
        public double CropScale { get; set; }
        public int CropX { get; set; }
        public int CropY { get; set; }
    }
}
