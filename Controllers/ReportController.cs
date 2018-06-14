using System;
using Atut.Services;
using Atut.Models;
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
        private readonly DatabaseContext _databaseContext;

        public ReportController(
            IDatabaseManager databaseManager,
            ReportService reportService,
            RoleService roleService,
            DatabaseContext databaseContext)
        {
            _databaseManager = databaseManager;
            _reportService = reportService;
            _roleService = roleService;
            _databaseContext = databaseContext;
        }
        
        [HttpGet]
        public IActionResult GenerateReport(int[] journeyIds, string country, DateTime dateFrom, DateTime dateTo, string companyId = null, string company = null)
        {
            //TODO zmienic na lepsze
            if (!_roleService.IsAdmin)
            {
                throw new UnauthorizedAccessException();
            }

            if (company == null)
            {
                company = _databaseContext.Users.Find(companyId).CompanyName;
            }

            var report = _reportService.GenerateReport(journeyIds, country, dateFrom, dateTo, company);

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
