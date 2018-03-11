using System.ComponentModel.DataAnnotations;

namespace Atut.ViewModels
{
    public class JourneyViewModel
    {
        [Required]
        public int Id { get; set; }

        [Required(ErrorMessage = "Miejsce wsiadania jest wymagane")]
        [Display(Name = "Miejsce wsiadania")]
        public string StartingPlace { get; set; }
    }
}
