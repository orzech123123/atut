using System;
using System.Collections.Generic;
using System.ComponentModel.DataAnnotations;

namespace Atut.ViewModels
{
    public class JourneyViewModel
    {
        public JourneyViewModel()
        {
            Countries = new List<CountryViewModel>();
            Invoices = new List<InvoiceViewModel>();
            Vehicles = new List<KeyValueViewModel>();
        }

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

        [Required(ErrorMessage = "Kraje są wymagane")]
        public List<CountryViewModel> Countries { get; set; }

        [Required(ErrorMessage = "Całkowita długość trasy jest wymagana")]
        public int TotalDistance { get; set; }

        [Required(ErrorMessage = "Dystans pokonany w innych krajach jest wymagany")]
        public int OtherCountriesTotalDistance { get; set; }

        [Required(ErrorMessage = "Pojazdy są wymagane")]
        public List<KeyValueViewModel> Vehicles { get; set; }

        [Required(ErrorMessage = "Faktury są wymagane")]
        public List<InvoiceViewModel> Invoices { get; set; }
    }
}
