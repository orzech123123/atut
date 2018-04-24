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

        [Required(ErrorMessage = "Nazwa firmy jest wymagana")]
        [Display(Name = "Nazwa firmy")]
        public string CompanyName { get; set; }

        [Required(ErrorMessage = "Nazwa firmy skrócona jest wymagana")]
        [Display(Name = "Nazwa firmy skrócona")]
        public string CompanyNameShort { get; set; }

        [Required(ErrorMessage = "Miejscowość jest wymagana")]
        [Display(Name = "Miejscowość")]
        public string Address { get; set; }
    }
}
