using System;
using Atut.Services;
using Microsoft.AspNetCore.Mvc;
using Microsoft.AspNetCore.Authorization;

namespace Atut.Controllers
{
    [Authorize]
    public class ReportController : Controller
    {
        private readonly IDatabaseManager _databaseManager;
        private readonly ReportService _reportService;

        public ReportController(
            IDatabaseManager databaseManager,
            ReportService reportService)
        {
            _databaseManager = databaseManager;
            _reportService = reportService;
        }
        
        [HttpGet]
        public IActionResult GenerateReport(int[] journeyIds, string country)
        {
            var report = _reportService.GenerateReport(journeyIds, country);

            return View("Report", report);
        }
        
        [HttpPost]
        public IActionResult NotifyAdmin(int[] journeyIds, string country, DateTime dateFrom, DateTime dateTo)
        {
            _reportService.NotifyAdmin(User, journeyIds, country, dateFrom, dateTo);
            _databaseManager.Commit();

            return Ok();
        }
    }
}
