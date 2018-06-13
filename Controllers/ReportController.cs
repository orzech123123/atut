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
        private readonly RoleService _roleService;

        public ReportController(
            IDatabaseManager databaseManager,
            ReportService reportService,
            RoleService roleService)
        {
            _databaseManager = databaseManager;
            _reportService = reportService;
            _roleService = roleService;
        }
        
        [HttpGet]
        public IActionResult GenerateReport(int[] journeyIds, string country)
        {
            //TODO zmienic na lepsze
            if (!_roleService.IsAdmin)
            {
                throw new UnauthorizedAccessException();
            }

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
