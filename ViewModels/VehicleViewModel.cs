using System.ComponentModel.DataAnnotations;

namespace Atut.ViewModels
{
    public class VehicleViewModel
    {
        [Required]
        public int Id { get; set; }

        [Required(ErrorMessage = "Nazwa jest wymagana")]
        [Display(Name = "Nazwa")]
        public string Name { get; set; }

        [Required(ErrorMessage = "Numer rejestracyjny jest wymagany")]
        [Display(Name = "Numer rejestracyjny")]
        public string RegistrationNumber { get; set; }
        

        public KeyValueViewModel Company { get; set; }
    }
}
