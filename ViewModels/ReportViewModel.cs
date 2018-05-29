using System;
using System.Collections.Generic;

namespace Atut.ViewModels
{
    public class ReportViewModel
    {
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
        public string Currency { get; set; }
        public decimal PartOfCountryInInvoicesAmountInCurrency { get; set; }
        public string InvoicesDates { get; set; }
    }
}
