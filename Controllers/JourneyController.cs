using System.Linq;
using Atut.Models;
using Atut.Services;
using Atut.ViewModels;
using Microsoft.AspNetCore.Mvc;
using Microsoft.AspNetCore.Authorization;

namespace Atut.Controllers
{
    [Authorize]
    public class JourneyController : Controller
    {
        private readonly JourneyService _journeyService;
        private readonly IDatabaseManager _databaseManager;

        public JourneyController(JourneyService journeyService, IDatabaseManager databaseManager)
        {
            _journeyService = journeyService;
            _databaseManager = databaseManager;
        }

        public IActionResult Index()
        {
            var viewModel = _journeyService.GetAll();

            return View(viewModel);
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
            var viewModel = _journeyService.GetOneById(id);

            return View(viewModel);
        }

        [HttpPost]
        public IActionResult Edit(JourneyViewModel viewModel)
        {
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
            _journeyService.Delete(id);
            _databaseManager.Commit();

            return RedirectToAction("Index");
        }
    }
}
