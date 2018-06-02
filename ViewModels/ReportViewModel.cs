using System;
using System.Collections.Generic;
using Atut.Models;

namespace Atut.ViewModels
{
    public class ReportViewModel
    {
        public string Country { get; set; }
        public CurrencyType CountryCurrency { get; set; }
        public IList<ReportRowViewModel> Rows = new List<ReportRowViewModel>();
    }

    public class ReportRowViewModel
    {
        public string RegistratioNumber { get; set; }
        public string FinalPlace { get; set; }
        public DateTime StartDate { get; set; }
        public DateTime EndDate { get; set; }
        public int TotalDistance { get; set; }
        public int CountryDistance { get; set; }
        public int AmountOfPeople { get; set; }
        public decimal InvoicesAmount { get; set; }
        public decimal PartOfCountryInInvoicesAmount { get; set; }
        public decimal PartOfCountryInInvoicesAmountInCurrencyAndWithTax { get; set; }
        public IEnumerable<DateTime> InvoicesDates { get; set; }
        public decimal ExchangeRate { get; set; }
    }
}
