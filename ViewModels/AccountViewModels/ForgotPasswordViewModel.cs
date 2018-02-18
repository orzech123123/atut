using System.ComponentModel.DataAnnotations;

namespace Atut.ViewModels.AccountViewModels
{
    public class ForgotPasswordViewModel
    {
        [Required(ErrorMessage = "E-mail jest wymagany")]
        [EmailAddress(ErrorMessage = "Niepoprawny e-mail")]
        public string Email { get; set; }

        public string TitleText { get; set; }
        public string ContentText { get; set; }
        public string ButtonText { get; set; }
    }
}
