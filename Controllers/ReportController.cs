using System;
using System.Threading.Tasks;
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
        public async Task<IActionResult> GenerateReport(string companyId, string country, DateTime dateFrom, DateTime dateTo, int[] journeyIds = null)
        {
            //TODO zmienic na lepsze
            if (!_roleService.IsAdmin)
            {
                throw new UnauthorizedAccessException();
            }

            try
            {
                var report = await _reportService.GenerateReport(companyId, country, dateFrom, dateTo, journeyIds);
                return View("Report", report);
            }
            catch (Exception e)
            {
                _logger.LogError(e, e.Message);
                return View("Report", null);
            }
        }
        
        [HttpPost]
        public async Task<IActionResult> NotifyAdmin(string country, DateTime dateFrom, DateTime dateTo)
        {
            await _reportService.NotifyAdmin(User, country, dateFrom, dateTo);
            _databaseManager.Commit();

            return Ok();
        }
    }
}
