using System;
using System.Linq;
using Atut.Models;
using Atut.ViewModels;
using FixerSharp;
using Microsoft.EntityFrameworkCore;

namespace Atut.Services
{
    public class ReportService
    {
        private readonly DatabaseContext _databaseContext;

        public ReportService(DatabaseContext databaseContext)
        {
            _databaseContext = databaseContext;
        }

        public ReportViewModel GenerateReport(int[] journeyIds, string country)
        {
            var journeys = _databaseContext.Journeys
                .Include(j => j.User)
                .Include(j => j.Countries)
                .Include(j => j.JourneyVehicles)
                .ThenInclude(jv => jv.Vehicle)
                .Include(j => j.Invoices)
                .Include(j => j.Countries)
                .Where(v => journeyIds.Contains(v.Id))
                .OrderByDescending(j => j.EndDate);

            if (journeyIds.Count() != journeys.Count())
            {
                throw new Exception("Amount of Journeys on backend is not equal to journeyIds count");
            }

            var report = new ReportViewModel
            {
                Country = country,
                CountryCurrency = GetCurrencyForCountry(country)
            };

            foreach (var journey in journeys)
            {
                report.Rows.Add(GenerateRow(journey, country));
            }

            return report;
        }

        public ReportRowViewModel GenerateRow(Journey journey, string country)
        {
            var row = new ReportRowViewModel
            {
                AmountOfPeople = journey.AmountOfPeople,
                EndDate = journey.EndDate,
                FinalPlace = journey.FinalPlace,
                StartDate = journey.StartDate,
                RegistratioNumber = string.Join(", ", journey.JourneyVehicles.Select(v => v.Vehicle.RegistrationNumber)),
                TotalDistance = journey.TotalDistance,
                CountryDistance = journey.Countries.Single(c => c.Name == country).Distance,
                InvoicesDates = string.Join(", ", journey.Invoices.Select(i => i.Date)),
                InvoicesAmount = Math.Round(journey.Invoices.Sum(i => GetAmountForCurrency(i, CurrencyType.PLN)), 2),
                ExchangeRate = (decimal) Fixer.Rate(CurrencyType.PLN.ToString(), GetCurrencyForCountry(country).ToString(),journey.Invoices.First().Date).Rate
            };

            var partOfCountryInInvoicesAmount = row.InvoicesAmount * row.CountryDistance / row.TotalDistance;
            row.PartOfCountryInInvoicesAmount = Math.Round(partOfCountryInInvoicesAmount, 2);

            var partOfCountryInInvoicesAmountInCurrency = CalculateBetweenCurrencies(partOfCountryInInvoicesAmount, journey.Invoices.First().Date, CurrencyType.PLN, GetCurrencyForCountry(country));
            partOfCountryInInvoicesAmountInCurrency /= GetTaxFactorForCountry(country);
            row.PartOfCountryInInvoicesAmountInCurrencyAndWithTax = Math.Round(partOfCountryInInvoicesAmountInCurrency, 2);

            return row;
        }

        private decimal GetTaxFactorForCountry(string country)
        {
            if (country == CountriesProvider.Germany)
            {
                return new decimal(1.19);
            }
            if (country == CountriesProvider.Slovenia)
            {
                return new decimal(1.095);
            }
            if (country == CountriesProvider.Austria)
            {
                return new decimal(1.1);
            }
            if (country == CountriesProvider.Belgium || country == CountriesProvider.Netherlands)
            {
                return new decimal(1.06);
            }
            if (country == CountriesProvider.Denmark || country == CountriesProvider.Croatia)
            {
                return new decimal(1.25);
            }

            throw new NotSupportedException($"Not supported tax for country: {country}");
        }

        private CurrencyType GetCurrencyForCountry(string country)
        {
            if (CountriesProvider.EuroCurrencyCountries.Contains(country))
            {
                return CurrencyType.EUR;
            }

            if (CountriesProvider.HrkCurrencyCountries.Contains(country))
            {
                return CurrencyType.HRK;
            }

            if (CountriesProvider.DkkCurrencyCountries.Contains(country))
            {
                return CurrencyType.DKK;
            }

            throw new NotSupportedException($"Not supported currency for country: {country}");
        }

        private decimal GetAmountForCurrency(Invoice invoice, CurrencyType currency)
        {
            return CalculateBetweenCurrencies(invoice.Amount, invoice.Date, invoice.Type, currency);
        }

        private decimal CalculateBetweenCurrencies(decimal amount, DateTime date, CurrencyType sourceCurrency, CurrencyType destCurrency)
        {
            if (sourceCurrency == destCurrency)
            {
                return amount;
            }

//            return amount / (decimal) 4.1695;

            return (decimal) Fixer.Convert(sourceCurrency.ToString(), destCurrency.ToString(), (double) amount, date);
        }
    }
}
