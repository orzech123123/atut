using System;
using System.Collections.Generic;
using System.Linq;
using System.Security.Claims;
using Atut.Identity;
using Atut.Models;
using Atut.ViewModels;
using FixerSharp;
using Microsoft.AspNetCore.Http;
using Microsoft.AspNetCore.Mvc;
using Microsoft.EntityFrameworkCore;
using RestSharp;

namespace Atut.Services
{
    public class ReportService
    {
        private readonly DatabaseContext _databaseContext;
        private readonly IEmailService _emailService;
        private readonly IHttpContextAccessor _httpContextAccessor;
        private readonly IUrlHelper _urlHelper;

        public ReportService(
            DatabaseContext databaseContext,
            IEmailService emailService,
            IHttpContextAccessor httpContextAccessor,
            IUrlHelper urlHelper)
        {
            _databaseContext = databaseContext;
            _emailService = emailService;
            _httpContextAccessor = httpContextAccessor;
            _urlHelper = urlHelper;
        }

        public void NotifyAdmin(ClaimsPrincipal user, int[] journeyIds, string country, DateTime dateFrom, DateTime dateTo)
        {
            var journeys = _databaseContext.Journeys
                .Where(v => journeyIds.Contains(v.Id))
                .ToList();

            journeys.ForEach(j => j.IsNotified = true);

            var companyName = user.Claims.Single(c => c.Type == UserClaimTypes.CompanyName).Value;

            var admins = _databaseContext.Users.Where(u => u.IsAdmin).ToList();

            var url = _urlHelper.Action("GenerateReport", "Report", new { journeyIds, country }, _httpContextAccessor.HttpContext.Request.Scheme);

            admins.ForEach(u =>
            {
                _emailService.SendEmailAsync(
                    u.Email, 
                    "Atut - powiadomienie o zakończeniu rozliczenia",
                    $"Firma {companyName} zakończyła rozliczenie dla kraju {country} " +
                    $"dla okresu {dateFrom:d MMM yyyy} - {dateTo:d MMM yyyy}." +
                    $"<br/>Kliknij w link poniżej, aby wygenerować raport:<br/><a href='{url}'>Generuj raport</a>"
                );
            });
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

        private ReportRowViewModel GenerateRow(Journey journey, string country)
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
                InvoicesDates = journey.Invoices.Select(i => i.Date),
                InvoicesAmount = Math.Round(journey.Invoices.Sum(i => GetAmountForCurrency(i, CurrencyType.PLN)), 2),
                ExchangeRate = GetRateBetweenCurrencies(journey.Invoices.First().Date, CurrencyType.PLN, GetCurrencyForCountry(country))
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

        private decimal GetRateBetweenCurrencies(DateTime date, CurrencyType sourceCurrency, CurrencyType destCurrency)
        {
            if (sourceCurrency != CurrencyType.PLN)
            {
                if (sourceCurrency == CurrencyType.EUR)
                {
                    return 1 / GetExchangeRateRequestResult(sourceCurrency, date); //tutaj jest 1/kurs euro dla pln bo nie da sie z nbp wyciagnac kurs pln dla euro (jedynie na odwrót czyli wlasnie euro dla pln)
                }

                throw new NotSupportedException("Not supported rate checking for sourceCurrency other than PLN or EUR");
            }

            return GetExchangeRateRequestResult(destCurrency, date);
        }

        private decimal GetExchangeRateRequestResult(CurrencyType destCurrency, DateTime date)
        {
            ExchangeRateRequestResult result = null;

            while (result == null)
            {
                var client = new RestClient($"http://api.nbp.pl/api/exchangerates/rates/a/{destCurrency}/{date:yyyy-MM-dd}");
                var request = new RestRequest(Method.GET);
                var response = client.Execute<ExchangeRateRequestResult>(request);

                result = response.Data;

                if (result == null)
                {
                    date = date.AddDays(-1);
                }
            }

            return result.Rates.First().Mid;
            //            return (decimal) Fixer.Rate(sourceCurrency.ToString(), destCurrency.ToString(), date).Rate;
        }

        private decimal CalculateBetweenCurrencies(decimal amount, DateTime date, CurrencyType sourceCurrency, CurrencyType destCurrency)
        {
            if (sourceCurrency == destCurrency)
            {
                return amount;
            }
            
            return amount / GetRateBetweenCurrencies(date, sourceCurrency, destCurrency);

//            return amount / (decimal) 4.1695;
//            return (decimal) Fixer.Convert(sourceCurrency.ToString(), destCurrency.ToString(), (double) amount, date);
        }
    }

    public class ExchangeRateRequestResult
    {
        public List<Rate> Rates { get; set; } = new List<Rate>();
    }

    public class Rate
    {
        public decimal Mid { get; set; }
    }
}

