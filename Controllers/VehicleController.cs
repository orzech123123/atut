using System.Linq;
using Atut.Models;
using Atut.Services;
using Atut.ViewModels;
using Microsoft.AspNetCore.Mvc;
using Microsoft.AspNetCore.Authorization;

namespace Atut.Controllers
{
    [Authorize]
    public class VehicleController : Controller
    {
        private readonly VehicleService _vehicleService;
        private readonly IDatabaseManager _databaseManager;

        public VehicleController(VehicleService vehicleService, IDatabaseManager databaseManager)
        {
            _vehicleService = vehicleService;
            _databaseManager = databaseManager;
        }

        public IActionResult Index()
        {
            var viewModel = _vehicleService.GetAll();

            return View(viewModel);
        }

        [HttpGet]
        public IActionResult Edit(int id)
        {
            var viewModel = _vehicleService.GetOneById(id);

            return View(viewModel);
        }

        [HttpPost]
        public IActionResult Edit(VehicleViewModel viewModel)
        {
            if (ModelState.IsValid)
            {
                _vehicleService.Save(viewModel);
                _databaseManager.Commit();
                return RedirectToAction("Index");
            }

            return View(viewModel);
        }
    }
}
