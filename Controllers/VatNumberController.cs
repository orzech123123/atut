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
        private readonly Authorizer _authorizer;

        public VatNumberController(
            VatNumberService vatNumberService,
            IDatabaseManager databaseManager,
            Authorizer authorizer)
        {
            _vatNumberService = vatNumberService;
            _databaseManager = databaseManager;
            _authorizer = authorizer;
        }

        [HttpGet]
        public IActionResult Edit(string id)
        {
            _authorizer.RequireVatNumberAutorization(id);

            var viewModel = _vatNumberService.GetByUserId(id);

            return View(viewModel);
        }

        [HttpPost]
        public IActionResult Edit(VatNumbersViewModel viewModel)
        {
            _authorizer.RequireVatNumberAutorization(viewModel.Company.Key);

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