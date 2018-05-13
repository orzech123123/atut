using System.Collections.Generic;
using System.Linq;
using Atut.Models;
using Atut.ViewModels;
using AutoMapper;
using Microsoft.AspNetCore.Http;
using Microsoft.AspNetCore.Mvc.ModelBinding;
using Microsoft.EntityFrameworkCore;

namespace Atut.Services
{
    public class JourneyService
    {
        private readonly DatabaseContext _databaseContext;
        private readonly IMapper _mapper;
        private readonly IHttpContextAccessor _httpContextAccessor;
        private readonly INotificationManager _notificationManager;
        private readonly RoleService _roleService;
        private readonly RequestModelService _requestModelService;

        public JourneyService(
            DatabaseContext databaseContext,
            IMapper mapper,
            IHttpContextAccessor httpContextAccessor,
            INotificationManager notificationManager,
            RoleService roleService,
            RequestModelService requestModelService
            )
        {
            _databaseContext = databaseContext;
            _mapper = mapper;
            _httpContextAccessor = httpContextAccessor;
            _notificationManager = notificationManager;
            _roleService = roleService;
            _requestModelService = requestModelService;
        }

        public IEnumerable<JourneyViewModel> GetAll()
        {
//            var filterModel = _requestModelService.GetModel<JourneyFilterModel>();
            
            IQueryable<Journey> query = _databaseContext.Journeys
                .Include(j => j.User)
                .Include(j => j.Countries)
                .Include(j => j.JourneyVehicles)
                .ThenInclude(jv => jv.Vehicle);

            if (!_roleService.IsAdmin)
            {
                query = query
                    .Where(v => v.User.UserName == _httpContextAccessor.HttpContext.User.Identity.Name);
            }

            return query
                .Select(v => _mapper.Map<Journey, JourneyViewModel>(v))
                .ToList();
        }

        public JourneyViewModel GetOneById(int id)
        {
            var journey = _databaseContext.Journeys
                .Include(j => j.User)
                .Include(j => j.Countries)
                .Include(j => j.Invoices)
                .Include(j => j.JourneyVehicles)
                .ThenInclude(jv => jv.Vehicle)
                .SingleOrDefault(v => v.Id == id);

            var viewModel = _mapper.Map<Journey, JourneyViewModel>(journey);

            return viewModel;
        }

        public void ValidateSave(JourneyViewModel viewModel, ModelStateDictionary modelState)
        {
            if (viewModel.TotalDistance <= 0)
            {
                modelState.AddModelError("TotalDistance", "Całkowita długość trasy musi być większa niż 0");
            }

            if (viewModel.OtherCountriesTotalDistance < 0)
            {
                modelState.AddModelError("OtherCountriesTotalDistance", "Dystans pokonany w innych krajach musi być nie mniejszy niż 0");
            }

            if (!viewModel.Countries.Any())
            {
                modelState.AddModelError("_FORM", "Trasa musi mieć przypisany co najmniej jeden Kraj");
            }

            if (!viewModel.Invoices.Any())
            {
                modelState.AddModelError("_FORM", "Trasa musi mieć przypisaną co najmniej jedną Fakturę");
            }

            if (!viewModel.Vehicles.Any())
            {
                modelState.AddModelError("_FORM", "Trasa musi mieć przypisany co najmniej jeden Pojazd");
            }
        }

        public void Save(JourneyViewModel viewModel)
        {
            Journey journey;

            if (viewModel.Id > 0)
            {
                journey = _databaseContext.Journeys
                    .Include(j => j.User)
                    .Include(j => j.Countries)
                    .Include(j => j.Invoices)
                    .Include(j => j.JourneyVehicles)
                    .Single(j => j.Id == viewModel.Id);
            }
            else
            {
                journey = new Journey();
            }

            _mapper.Map(viewModel, journey);
            UpdateUser(viewModel, journey);
            UpdateVehicles(viewModel, journey);

            if (journey.Id <= 0)
            {
                _databaseContext.Journeys.Add(journey);
                _notificationManager.Add(NotificationType.Information, "Trasa została dodana.");
            }
            else
            {
                _notificationManager.Add(NotificationType.Information, "Trasa została zmodyfikowana.");
            }
        }

        public JourneyViewModel Create()
        {
            var viewModel = new JourneyViewModel();

            var user = _databaseContext.Users.Single(u => u.UserName == _httpContextAccessor.HttpContext.User.Identity.Name);
            var company = new KeyValueViewModel {Key = user.Id, Value = user.CompanyNameShort};
            viewModel.Company = company;

            return viewModel;
        }

        public void Delete(int id)
        {
            var journey = _databaseContext.Journeys.Include(j => j.JourneyVehicles).Single(j => j.Id == id);
            
            journey.JourneyVehicles.Clear();
            _databaseContext.Journeys.Remove(journey);

            _notificationManager.Add(NotificationType.Information, "Trasa została usunięta.");
        }

        private void UpdateUser(JourneyViewModel viewModel, Journey journey)
        {
            journey.User = _databaseContext.Users.Single(u => u.Id == viewModel.Company.Key);
        }

        private void UpdateVehicles(JourneyViewModel viewModel, Journey journey)
        {
            foreach (var journeyVehicle in journey.JourneyVehicles.ToList())
            {
                if (!viewModel.Vehicles.Any() || viewModel.Vehicles.All(v => v.Key != journeyVehicle.VehicleId.ToString()))
                {
                    journey.JourneyVehicles.Remove(journeyVehicle);
                }
            }

            foreach (var vehicle in viewModel.Vehicles)
            {
                if (journey.JourneyVehicles.All(jv => jv.VehicleId.ToString() != vehicle.Key))
                {
                    journey.JourneyVehicles.Add(new JourneyVehicle
                    {
                        Journey = journey,
                        Vehicle = _databaseContext.Vehicles.Single(v => v.Id.ToString() == vehicle.Key)
                    });
                }
            }
        }
    }
}
