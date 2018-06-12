using Atut.Models;

namespace Atut.Services
{
    public interface IReportLabelDictionary
    {
        string Title { get; }
        string RegistrationNumbers { get; }
        string FinalPlace { get; }
        string StartDate { get; }
        string EndDate { get; }
        string TotalDistance { get; }
        string CountryDistance(string country);
        string AmountOfPeople { get; }
        string InvoicesAmount { get; }
        string PartOfCountryInInvoicesAmount(string country);
        string InvoicesDates { get; }
        string ExchangeRate { get; }
        string PartOfCountryInInvoicesAmountInCurrencyAndWithTax(string country, CurrencyType currency);
    }

    public class EnglishReportLabelDictionary : IReportLabelDictionary
    {
        public string Title => "Attachment for VAT declaration";
        public string RegistrationNumbers => "Registration";
        public string FinalPlace => "Destination";
        public string StartDate => "Date of arrival";
        public string EndDate => "Departure";
        public string TotalDistance => "Km in total";
        public string CountryDistance(string country) => $"Km in {country}";
        public string AmountOfPeople => "Passengers";
        public string InvoicesAmount => "Total transportfee";
        public string PartOfCountryInInvoicesAmount(string country) => $"Portion for {country}";
        public string InvoicesDates => "Invoice dates";
        public string ExchangeRate => "Exchange rates";
        public string PartOfCountryInInvoicesAmountInCurrencyAndWithTax(string country, CurrencyType currency) => $"Portion for {country} in {currency.ToString()}";
    }

    public class GermanReportLabelDictionary : IReportLabelDictionary
    {
        public string Title => "Anlage zur Umsatzsteuervoranmeldung";
        public string RegistrationNumbers => "KFZ - Kennzeichen";
        public string FinalPlace => "Reiseziel";
        public string StartDate => "Einreise Datum";
        public string EndDate => "Departure";
        public string TotalDistance => "Km insges";
        public string CountryDistance(string country) => $"Km in {country}";
        public string AmountOfPeople => "Anzahl Passagiere";
        public string InvoicesAmount => "Beförderungsentgelt";
        public string PartOfCountryInInvoicesAmount(string country) => $"Anteil {country} in heimischer Währung";
        public string InvoicesDates => "Rechnungsdatum";
        public string ExchangeRate => "Umrechnungskurs";
        public string PartOfCountryInInvoicesAmountInCurrencyAndWithTax(string country, CurrencyType currency) => $"Anteil {country} ({currency.ToString()})";
    }
}
