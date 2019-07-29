using Atut.Services;
using Atut.ViewModels;
using Microsoft.AspNetCore.Mvc;
using Microsoft.AspNetCore.Authorization;

namespace Atut.Controllers
{
    [Authorize]
    public class VatNumberController : Controller
    {
        private readonly VatNumberService _vatNumberService;
        private readonly IDatabaseManager _databaseManager;

        public VatNumberController(VatNumberService vatNumberService, IDatabaseManager databaseManager)
        {
            _vatNumberService = vatNumberService;
            _databaseManager = databaseManager;
        }

        [HttpGet]
        public IActionResult Edit(string id)
        {
            var viewModel = _vatNumberService.GetByUserId(id);

            return View(viewModel);
        }

        [HttpPost]
        public IActionResult Edit(VatNumbersViewModel viewModel)
        {
            if (ModelState.IsValid)
            {
                _vatNumberService.Save(viewModel);
                _databaseManager.Commit();

                return RedirectToAction("Edit", new {id = viewModel.Company?.Key});
            }

            return View(viewModel);
        }
    }
}