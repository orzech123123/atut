using System;
using System.Collections.Generic;
using System.Linq;
using Atut.Models;

namespace Atut.ViewModels
{
    public class ReportViewModel
    {
        public string Country { get; set; }
        public CurrencyType CountryCurrency { get; set; }
        public DateTime DateFrom { get; set; }
        public DateTime DateTo { get; set; }
        public string Company { get; set; }

        public IList<ReportRowViewModel> Rows = new List<ReportRowViewModel>();

        public decimal BruttoSum => Rows.Sum(r => r.PartOfCountryInInvoicesAmountInCurrency);
        public decimal NettoSum { get; set; }
        public decimal VatResult => BruttoSum - NettoSum;
    }

    public class ReportRowViewModel
    {
        public IList<string> RegistratioNumbers { get; set; } = new List<string>();
        public string FinalPlace { get; set; }
        public DateTime StartDate { get; set; }
        public DateTime EndDate { get; set; }
        public int TotalDistance { get; set; }
        public int CountryDistance { get; set; }
        public int AmountOfPeople { get; set; }
        public IList<(decimal, CurrencyType)> InvoicesAmounts { get; set; } = new List<(decimal, CurrencyType)>();
        public IList<(decimal, CurrencyType)> PartsOfCountryInInvoicesAmounts { get; set; } = new List<(decimal, CurrencyType)>();
        public decimal PartOfCountryInInvoicesAmountInCurrency { get; set; }
        public decimal NettoResult { get; set; }
        public IList<DateTime> InvoicesDates { get; set; } = new List<DateTime>();
        public IList<decimal> ExchangeRates { get; set; } = new List<decimal>();
    }
}
