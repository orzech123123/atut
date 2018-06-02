using Atut.Filters;
using Atut.Services;
using Atut.ViewModels;
using Microsoft.AspNetCore.Mvc;
using Microsoft.AspNetCore.Authorization;

namespace Atut.Controllers
{
    [Authorize]
    public class ReportController : Controller
    {
        private readonly ReportService _reportService;

        public ReportController(
            ReportService reportService)
        {
            _reportService = reportService;
        }
        
        [HttpGet]
        public IActionResult GenerateReport(int[] journeyIds, string country)
        {
            var report = _reportService.GenerateReport(journeyIds, country);

            return View("Report", report);
        }
        
        [HttpPost]
        public IActionResult NotifyAdmin(string country)
        {
            _reportService.NotifyAdmin(User, country);

            return Ok();
        }
    }
}
