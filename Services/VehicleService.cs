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
    public class VehicleService
    {
        private readonly DatabaseContext _databaseContext;
        private readonly IMapper _mapper;
        private readonly IHttpContextAccessor _httpContextAccessor;
        private readonly INotificationManager _notificationManager;

        public VehicleService(
            DatabaseContext databaseContext,
            IMapper mapper,
            IHttpContextAccessor httpContextAccessor,
            INotificationManager notificationManager)
        {
            _databaseContext = databaseContext;
            _mapper = mapper;
            _httpContextAccessor = httpContextAccessor;
            _notificationManager = notificationManager;
        }

        public IEnumerable<VehicleViewModel> GetAll()
        {
            return _databaseContext.Vehicles
                .Include(v => v.User)
                .Where(v => v.User.UserName == _httpContextAccessor.HttpContext.User.Identity.Name)
                .Select(v => _mapper.Map<Vehicle, VehicleViewModel>(v))
                .ToList();
        }

        public IEnumerable<KeyValueViewModel> GetAllKeyValueForJourney(int journeyId)
        {
            var journey = _databaseContext.Journeys.Include(j => j.User).SingleOrDefault(j => j.Id == journeyId);
            var userName = (journey?.User?.UserName ?? _httpContextAccessor.HttpContext.User.Identity.Name);

            return _databaseContext.Vehicles
                .Include(v => v.User)
                .Where(v => v.User.UserName == userName)
                .Select(v => _mapper.Map<Vehicle, KeyValueViewModel>(v))
                .ToList();
        }

        public VehicleViewModel GetOneById(int id)
        {
            var vehicle = _databaseContext.Vehicles.Include(v => v.User).SingleOrDefault(v => v.Id == id);
            var viewModel = _mapper.Map<Vehicle, VehicleViewModel>(vehicle);

            return viewModel;
        }

        public void ValidateSave(VehicleViewModel viewModel, ModelStateDictionary modelState)
        {
            if (!string.IsNullOrWhiteSpace(viewModel.RegistrationNumber))
            {
                var sameRegisterNumberVehicle = _databaseContext.Vehicles
                    .Where(v => v.User.UserName == _httpContextAccessor.HttpContext.User.Identity.Name)
                    .Where(v => v.Id != viewModel.Id)
                    .SingleOrDefault(v => v.RegistrationNumber.ToLower() == viewModel.RegistrationNumber.ToLower());

                if (sameRegisterNumberVehicle != null)
                {
                    modelState.AddModelError(nameof(VehicleViewModel.RegistrationNumber), "Istnieje już pojazd z identycznym numerem rejestracyjnym.");
                }
            }
        }

        public void Save(VehicleViewModel viewModel)
        {
            Vehicle vehicle;

            if (viewModel.Id > 0)
            {
                vehicle = _databaseContext.Vehicles.Include(v => v.User).SingleOrDefault(v => v.Id == viewModel.Id);
            }
            else
            {
                vehicle = new Vehicle();
            }

            _mapper.Map(viewModel, vehicle);
            UpdateUser(vehicle);

            if (vehicle.Id <= 0)
            {
                _databaseContext.Vehicles.Add(vehicle);
                _notificationManager.Add(NotificationType.Information, "Pojazd został dodany.");
            }
            else
            {
                _notificationManager.Add(NotificationType.Information, "Pojazd został zmodyfikowany.");
            }
        }

        private void UpdateUser(Vehicle vehicle)
        {
            vehicle.User = vehicle.User ?? _databaseContext.Users.Single(u => u.UserName == _httpContextAccessor.HttpContext.User.Identity.Name);
        }

        public VehicleViewModel Create()
        {
            return new VehicleViewModel();
        }

        public void Delete(int id)
        {
            var vehicle = _databaseContext.Vehicles.Include(v => v.JourneyVehicles).Single(v => v.Id == id);

            if (vehicle.JourneyVehicles.Any())
            {
                _notificationManager.Add(NotificationType.Error, "Pojazd nie może zostać usunięty, gdyż jest przypisany do co najmniej jednej Trasy.");
            }
            else
            {
                _databaseContext.Vehicles.Remove(vehicle);
                _notificationManager.Add(NotificationType.Information, "Pojazd został usunięty.");
            }
        }
    }
}
