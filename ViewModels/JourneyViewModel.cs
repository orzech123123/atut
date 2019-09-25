using System;
using System.Collections.Generic;
using System.ComponentModel.DataAnnotations;
using Atut.Converters;
using Newtonsoft.Json;

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

        [Required(ErrorMessage = "Miejscowość wsiadania jest wymagana")]
        [Display(Name = "Miejscowość wsiadania")]
        public string StartingPlace { get; set; }
        
        [Display(Name = "Miejscowość pośrednia")]
        public string ThroughPlace { get; set; }

        [Required(ErrorMessage = "Miejscowość docelowa + kraj jest wymagana")]
        [Display(Name = "Miejscowość docelowa + kraj")]
        public string FinalPlace { get; set; }

        [Required(ErrorMessage = "Ilość osób jest wymagana")]
        [Range(1, Int32.MaxValue, ErrorMessage = "Ilość osób musi być większa od zera")]
        [Display(Name = "Ilość osób")]
        public int AmountOfPeople { get; set; }

        [Required(ErrorMessage = "Data wyjazdu jest wymagana")]
        [Display(Name = "Data wyjazdu")]
        [JsonConverter(typeof(DateTimeJsonConverter), "yyyy-MM-dd")]
        public DateTime? StartDate { get; set; }

        [Required(ErrorMessage = "Data powrotu jest wymagana")]
        [Display(Name = "Data powrotu")]
        [JsonConverter(typeof(DateTimeJsonConverter), "yyyy-MM-dd")]
        public DateTime? EndDate { get; set; }

        [Required(ErrorMessage = "Kraje są wymagane")]
        public List<CountryViewModel> Countries { get; set; }

        [Required(ErrorMessage = "Ilość km ogółem jest wymagana")]
        public int TotalDistance { get; set; }

        [Required(ErrorMessage = "Ilość km w innych krajach jest wymagana")]
        public int OtherCountriesTotalDistance { get; set; }

        [Required(ErrorMessage = "Pojazdy są wymagane")]
        public List<KeyValueViewModel> Vehicles { get; set; }

        [Required(ErrorMessage = "Faktury są wymagane")]
        public List<InvoiceViewModel> Invoices { get; set; }
        

        public KeyValueViewModel Company { get; set; }

        public bool IsNotified { get; set; }
    }
}
