using System;
using System.ComponentModel.DataAnnotations;
using Microsoft.AspNetCore.Builder;

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

        [Required(ErrorMessage = "Ilość osób jest wymagana")]
        [Range(1, Int32.MaxValue, ErrorMessage = "Ilość osób musi być większa od zera")]
        [Display(Name = "Ilość osób")]
        public int AmountOfPeople { get; set; }

        [Required(ErrorMessage = "Data wyjazdu jest wymagana")]
        [Display(Name = "Data wyjazdu")]
        public DateTime? StartDate { get; set; }

        [Required(ErrorMessage = "Data powrotu jest wymagana")]
        [Display(Name = "Data powrotu")]
        public DateTime? EndDate { get; set; }
    }
}
