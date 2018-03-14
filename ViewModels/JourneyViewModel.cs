using System;
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

        [Required(ErrorMessage = "Miejsce pośrednie jest wymagane")]
        [Display(Name = "Miejsce pośrednie")]
        public string ThroughPlace { get; set; }

        [Required(ErrorMessage = "Miejsce końcowe jest wymagane")]
        [Display(Name = "Miejsce końcowe")]
        public string FinalPlace { get; set; }

        [Required(ErrorMessage = "Data wyjazdu jest wymagana")]
        [Display(Name = "Data wyjazdu")]
        public DateTime? StartDate { get; set; }

        [Required(ErrorMessage = "Data powrotu jest wymagana")]
        [Display(Name = "Data powrotu")]
        public DateTime? EndDate { get; set; }
    }
}
