using System.Linq;
using Atut.Identity;
using Atut.Models;
using Atut.Services;
using Atut.ViewModels;
using Microsoft.AspNetCore.Mvc;
using Microsoft.AspNetCore.Authorization;
using Microsoft.AspNetCore.Http;
using Newtonsoft.Json;

namespace Atut.Controllers
{
    [Authorize]
    public class VehicleController : Controller
    {
        private readonly VehicleService _vehicleService;
        private readonly IDatabaseManager _databaseManager;
        private readonly Authorizer _authorizer;
        private readonly IHttpContextAccessor _httpContextAccessor;

        public VehicleController(
            VehicleService vehicleService,
            IDatabaseManager databaseManager,
            Authorizer authorizer,
            IHttpContextAccessor httpContextAccessor)
        {
            _vehicleService = vehicleService;
            _databaseManager = databaseManager;
            _authorizer = authorizer;
            _httpContextAccessor = httpContextAccessor;
        }

        public IActionResult Index()
        {
            var viewModel = _vehicleService.GetAll();

            return View(viewModel);
        }

        [HttpGet]
        public IActionResult Create()
        {
            var viewModel = _vehicleService.Create();

            return View(viewModel);
        }

        [HttpPost]
        public IActionResult Create(VehicleViewModel viewModel)
        {
            _authorizer.RequireVehicleAutorization(viewModel.Id, viewModel.Company.Key);

            _vehicleService.ValidateSave(viewModel, ModelState);

            if (ModelState.IsValid)
            {
                _vehicleService.Save(viewModel);
                _databaseManager.Commit();
                return RedirectToAction("Index");
            }

            return View(viewModel);
        }

        [HttpGet]
        public IActionResult Edit(int id)
        {
            _authorizer.RequireVehicleAutorization(
                id,
                _httpContextAccessor.HttpContext.User.Claims.Single(c => c.Type == UserClaimTypes.CompanyId).Value
            );

            var viewModel = _vehicleService.GetOneById(id);

            return View(viewModel);
        }

        [HttpPost]
        public IActionResult Edit(VehicleViewModel viewModel)
        {
            _authorizer.RequireVehicleAutorization(viewModel.Id, viewModel.Company.Key);

            _vehicleService.ValidateSave(viewModel, ModelState);

            if (ModelState.IsValid)
            {
                _vehicleService.Save(viewModel);
                _databaseManager.Commit();

                return RedirectToAction("Index");
            }

            return View(viewModel);
        }

        [HttpGet]
        public IActionResult Delete(int id)
        {
            _authorizer.RequireVehicleAutorization(
                id,
                _httpContextAccessor.HttpContext.User.Claims.Single(c => c.Type == UserClaimTypes.CompanyId).Value
            );

            _vehicleService.Delete(id);
            _databaseManager.Commit();

            return RedirectToAction("Index");
        }

        [HttpGet]
        public IActionResult GetAllForUser([FromQuery] string id)
        {
            return Json(_vehicleService.GetAllKeyValueForUser(id));
        }
    }
}
