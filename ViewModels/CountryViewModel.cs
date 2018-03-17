using System.ComponentModel.DataAnnotations;

namespace Atut.ViewModels
{
    public class CountryViewModel
    {
        [Required(ErrorMessage = "Nazwa kraju jest wymagana")]
        public string Name { get; set; }

        [Required(ErrorMessage = "Kilometry są wymagane")]
        public int Distance { get; set; }
    }
}
