using System.ComponentModel.DataAnnotations;

namespace Atut.ViewModels.AccountViewModels
{
    public class SendEmailViewModel
    {
        [Required]
        [EmailAddress]
        public string Email { get; set; }

        public string TitleText { get; set; }
        public string ContentText { get; set; }
        public string ButtonText { get; set; }
    }
}
