using System.Linq;
using System.Threading.Tasks;
using Atut.Filters;
using Atut.Identity;
using Atut.Services;
using Atut.Paging;
using Atut.ViewModels;
using Microsoft.AspNetCore.Mvc;
using Microsoft.AspNetCore.Authorization;
using Microsoft.AspNetCore.Http;

namespace Atut.Controllers
{
    [Authorize]
    public class JourneyController : Controller
    {
        private readonly JourneyService _journeyService;
        private readonly IDatabaseManager _databaseManager;
        private readonly Authorizer _authorizer;
        private readonly IHttpContextAccessor _httpContextAccessor;

        public JourneyController(
            JourneyService journeyService,
            IDatabaseManager databaseManager,
            Authorizer authorizer,
            IHttpContextAccessor httpContextAccessor)
        {
            _journeyService = journeyService;
            _databaseManager = databaseManager;
            _authorizer = authorizer;
            _httpContextAccessor = httpContextAccessor;
        }

        [RequestModel(typeof(JourneyFilterModel))]
        public IActionResult Index()
        {
            return View();
        }

        [HttpGet]
        public IActionResult Create()
        {
            var viewModel = _journeyService.Create();

            return View(viewModel);
        }

        [HttpPost]
        public IActionResult Create(JourneyViewModel viewModel)
        {
            _authorizer.RequireJourneyAutorization(viewModel.Id, viewModel.Company.Key);

            _journeyService.ValidateSave(viewModel, ModelState);

            if (ModelState.IsValid)
            {
                _journeyService.Save(viewModel);
                _databaseManager.Commit();
                return RedirectToAction("Index");
            }

            return View(viewModel);
        }

        [HttpGet]
        public IActionResult Edit(int id)
        {
            _authorizer.RequireJourneyAutorization(
                id, 
                _httpContextAccessor.HttpContext.User.Claims.Single(c => c.Type == UserClaimTypes.CompanyId).Value
            );

            var viewModel = _journeyService.GetOneById(id);

            return View(viewModel);
        }

        [HttpPost]
        public IActionResult Edit(JourneyViewModel viewModel)
        {
            _authorizer.RequireJourneyAutorization(viewModel.Id, viewModel.Company.Key);

            _journeyService.ValidateSave(viewModel, ModelState);

            if (ModelState.IsValid)
            {
                _journeyService.Save(viewModel);
                _databaseManager.Commit();

                return RedirectToAction("Index");
            }

            return View(viewModel);
        }

        [HttpGet]
        public IActionResult Delete(int id)
        {
            _authorizer.RequireJourneyAutorization(
                id,
                _httpContextAccessor.HttpContext.User.Claims.Single(c => c.Type == UserClaimTypes.CompanyId).Value
            );

            _journeyService.Delete(id);
            _databaseManager.Commit();

            return RedirectToAction("Index");
        }

        [HttpGet]
        public async Task<IActionResult> FetchAll(VueTablesPageRequest pageInfo)
        {
            var data = await _journeyService.GetAllAsync(pageInfo);
            var count = await _journeyService.CountAllAsync();

            return Json(new { data, count });
        }
    }
}
