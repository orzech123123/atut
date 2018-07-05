using System;
using Atut.Services;
using Atut.Models;
using Microsoft.AspNetCore.Mvc;
using Microsoft.AspNetCore.Authorization;
using Microsoft.Extensions.Logging;

namespace Atut.Controllers
{
    [Authorize]
    public class ReportController : Controller
    {
        private readonly IDatabaseManager _databaseManager;
        private readonly ReportService _reportService;
        private readonly RoleService _roleService;
        private readonly DatabaseContext _databaseContext;
        private readonly ILogger<ReportController> _logger;

        public ReportController(
            IDatabaseManager databaseManager,
            ReportService reportService,
            RoleService roleService,
            DatabaseContext databaseContext,
            ILogger<ReportController> logger)
        {
            _databaseManager = databaseManager;
            _reportService = reportService;
            _roleService = roleService;
            _databaseContext = databaseContext;
            _logger = logger;
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

            try
            {
                var report = _reportService.GenerateReport(journeyIds, country, dateFrom, dateTo, company);
                return View("Report", report);
            }
            catch (Exception e)
            {
                _logger.LogError(e, e.Message);
                return View("Report", null);
            }
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
