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
        private readonly AtutDbContext _dbContext;
        private readonly IDatabaseManager<AtutDbContext> _databaseManager;

        public HomeController(AtutDbContext dbContext, IDatabaseManager<AtutDbContext> databaseManager)
        {
            _dbContext = dbContext;
            _databaseManager = databaseManager;
        }

        public IActionResult Index()
        {
            _dbContext.Vehicles.Add(new Vehicle {Name = "asd", RegistrationNumber = "324"});
            _databaseManager.Commit();

            return View(_dbContext.Vehicles.Count());
        }
    }
}
