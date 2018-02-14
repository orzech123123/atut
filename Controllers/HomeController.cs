using System.Linq;
using Atut.Models;
using Atut.Services;
using Microsoft.AspNetCore.Mvc;
using Microsoft.AspNetCore.Authorization;

namespace Atut.Controllers
{
    [Authorize]
    public class HomeController : Controller
    {
        private readonly DatabaseContext _databaseContext;
        private readonly IDatabaseManager _databaseManager;

        public HomeController(DatabaseContext databaseContext, IDatabaseManager databaseManager)
        {
            _databaseContext = databaseContext;
            _databaseManager = databaseManager;
        }

        public IActionResult Index()
        {
            var user = _databaseContext.Users.Single(u => u.Email == User.Identity.Name);
            var vehicle = _databaseContext.Vehicles.FirstOrDefault();

            _databaseContext.Vehicles.Add(new Vehicle {Name = "asd", RegistrationNumber = "324", User = user });
            _databaseManager.Commit();

            return View(_databaseContext.Vehicles.Count());
        }
    }
}
