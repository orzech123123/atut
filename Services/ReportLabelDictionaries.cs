using Atut.Models;

namespace Atut.Services
{
    public interface IReportLabelDictionary
    {
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
        public string RegistrationNumbers => "Registration";
        public string FinalPlace => "Destination";
        public string StartDate => "Date of arrival";
        public string EndDate => "Departure";
        public string TotalDistance => "Km in total";
        public string CountryDistance(string country) => $"Km in {country}";
        public string AmountOfPeople => "Passengers";
        public string InvoicesAmount => "Total transportfee in PLN";
        public string PartOfCountryInInvoicesAmount(string country) => $"Portion for {country} in PLN";
        public string InvoicesDates => "Invoice date";
        public string ExchangeRate => "Exchange rate";
        public string PartOfCountryInInvoicesAmountInCurrencyAndWithTax(string country, CurrencyType currency) => $"Portion for {country} in {currency.ToString()}";
    }

    public class GermanReportLabelDictionary : IReportLabelDictionary
    {
        public string RegistrationNumbers => "KFZ - Kennzeichen";
        public string FinalPlace => "Reiseziel";
        public string StartDate => "Einreise Datum";
        public string EndDate => "Departure";
        public string TotalDistance => "Km insges";
        public string CountryDistance(string country) => $"Km in {country}";
        public string AmountOfPeople => "Anzahl Passagiere";
        public string InvoicesAmount => "Beforderungsentgelt (PLN)";
        public string PartOfCountryInInvoicesAmount(string country) => $"Anteil {country} in heimischer Wahrung (PLN)";
        public string InvoicesDates => "Rechnungsdatum";
        public string ExchangeRate => "Umrechnungs - Kurs";
        public string PartOfCountryInInvoicesAmountInCurrencyAndWithTax(string country, CurrencyType currency) => $"Anteil {country} ({currency.ToString()})";
    }
}
