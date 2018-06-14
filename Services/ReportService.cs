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
        private readonly CountriesHelper _countriesHelper;

        public ReportService(
            DatabaseContext databaseContext,
            IEmailService emailService,
            IHttpContextAccessor httpContextAccessor,
            IUrlHelper urlHelper,
            CountriesHelper countriesHelper)
        {
            _databaseContext = databaseContext;
            _emailService = emailService;
            _httpContextAccessor = httpContextAccessor;
            _urlHelper = urlHelper;
            _countriesHelper = countriesHelper;
        }

        public void NotifyAdmin(ClaimsPrincipal user, int[] journeyIds, string country, DateTime dateFrom, DateTime dateTo)
        {
            var journeys = _databaseContext.Journeys
                .Where(v => journeyIds.Contains(v.Id))
                .ToList();

            journeys.ForEach(j => j.IsNotified = true);

            var company = user.Claims.Single(c => c.Type == UserClaimTypes.CompanyName).Value;

            var admins = _databaseContext.Users.Where(u => u.IsAdmin).ToList();

            var url = _urlHelper.Action("GenerateReport", "Report", new { journeyIds, country, dateFrom, dateTo, company }, _httpContextAccessor.HttpContext.Request.Scheme);

            admins.ForEach(u =>
            {
                _emailService.SendEmailAsync(
                    u.Email,
                    "Atut - powiadomienie o zakończeniu rozliczenia",
                    $"Firma {company} zakończyła rozliczenie dla kraju {country} " +
                    $"dla okresu {dateFrom:d MMM yyyy} - {dateTo:d MMM yyyy}." +
                    $"<br/>Kliknij w link poniżej, aby wygenerować raport:<br/><a href='{url}'>Generuj raport</a>"
                );
            });
        }

        public ReportViewModel GenerateReport(int[] journeyIds, string country, DateTime dateFrom, DateTime dateTo, string company)
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
                CountryCurrency = _countriesHelper.GetCurrencyForCountry(country),
                DateFrom = dateFrom,
                DateTo = dateTo,
                Company = company
            };

            foreach (var journey in journeys)
            {
                report.Rows.Add(GenerateRow(journey, country));
            }

            report.SumPartOfCountryInInvoicesAmountInCurrencyAndTax = Math.Round(
                report.Rows.Sum(r => r.PartOfCountryInInvoicesAmountInCurrency) /
                _countriesHelper.GetTaxFactorForCountry(country),
                2
            );
            
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
                RegistratioNumbers = journey.JourneyVehicles.Select(v => v.Vehicle.RegistrationNumber).ToList(),
                TotalDistance = journey.TotalDistance,
                CountryDistance = journey.Countries.Single(c => c.Name == country).Distance,
                InvoicesDates = journey.Invoices.Select(i => i.Date).ToList(),
                InvoicesAmounts = journey.Invoices.Select(i => (Math.Round(i.Amount, 2), i.Type)).ToList(),
                ExchangeRates = journey.Invoices.Select(i => GetRateBetweenCurrencies(i.Date, i.Type, _countriesHelper.GetCurrencyForCountry(country))).ToList()
            };

            row.PartsOfCountryInInvoicesAmounts = journey.Invoices.Select(i =>
                (
                    Math.Round(i.Amount * row.CountryDistance / row.TotalDistance, 2),
                    i.Type
                )
            ).ToList();
            
            row.PartOfCountryInInvoicesAmountInCurrency = Math.Round(journey.Invoices.Select((invoice, index) =>
                CalculateBetweenCurrencies(row.PartsOfCountryInInvoicesAmounts[index].Item1, invoice.Date, invoice.Type, _countriesHelper.GetCurrencyForCountry(country))
            ).Sum(), 2);

            return row;
        }

        private decimal GetAmountForCurrency(Invoice invoice, CurrencyType currency)
        {
            return CalculateBetweenCurrencies(invoice.Amount, invoice.Date, invoice.Type, currency);
        }

        private decimal GetRateBetweenCurrencies(DateTime date, CurrencyType sourceCurrency, CurrencyType destCurrency)
        {
            if (sourceCurrency == destCurrency)
            {
                return 1;
            }

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

