using System.ComponentModel.DataAnnotations;

namespace Atut.ViewModels.AccountViewModels
{
    public class RegisterViewModel
    {
        [Required(ErrorMessage = "E-mail jest wymagany")]
        [EmailAddress(ErrorMessage = "Niepoprawny e-mail")]
        [Display(Name = "Email")]
        public string Email { get; set; }

        [Required(ErrorMessage = "Hasło jest wymagane")]
        [StringLength(100, ErrorMessage = "Hasło musi zawierać co najmniej {2} znaków", MinimumLength = 6)]
        [DataType(DataType.Password)]
        [Display(Name = "Password")]
        public string Password { get; set; }

        [DataType(DataType.Password)]
        [Display(Name = "Confirm password")]
        [Compare("Password", ErrorMessage = "Potwierdzenie hasła nie odpowiada wprowadzonemu hasłu")]
        public string ConfirmPassword { get; set; }

        [Required(ErrorMessage = "Imię jest wymagane")]
        [Display(Name = "Imię")]
        public string FirstName { get; set; }

        [Required(ErrorMessage = "Nazwisko jest wymagane")]
        [Display(Name = "Nazwisko")]
        public string LastName { get; set; }

        [Required(ErrorMessage = "Nr telefonu jest wymagany")]
        [Phone]
        [Display(Name = "Nr telefonu")]
        public string PhoneNumber { get; set; }
    }
}
