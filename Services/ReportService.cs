using System;
using System.Linq;
using Atut.Models;
using Atut.ViewModels;
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

            var report = new ReportViewModel();

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
                //TODO
            };

            return row;
        }
    }
}
