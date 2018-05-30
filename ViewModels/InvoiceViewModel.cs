using System;
using System.ComponentModel.DataAnnotations;
using Atut.Models;
using Newtonsoft.Json;
using Newtonsoft.Json.Converters;

namespace Atut.ViewModels
{
    public class InvoiceViewModel
    {
        [Required(ErrorMessage = "Data faktury jest wymagana")]
        public DateTime? Date { get; set; }

        [Required(ErrorMessage = "Typ faktury jest wymagany")]
        [JsonConverter(typeof(StringEnumConverter))]
        public CurrencyType Type { get; set; }

        [Required(ErrorMessage = "Kwota faktury jest wymagana")]
        public decimal Amount { get; set; }
    }
}
