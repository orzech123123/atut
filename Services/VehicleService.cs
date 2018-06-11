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
        private readonly RoleService _roleService;

        public VehicleService(
            DatabaseContext databaseContext,
            IMapper mapper,
            IHttpContextAccessor httpContextAccessor,
            INotificationManager notificationManager,
            RoleService roleService)
        {
            _databaseContext = databaseContext;
            _mapper = mapper;
            _httpContextAccessor = httpContextAccessor;
            _notificationManager = notificationManager;
            _roleService = roleService;
        }

        public IEnumerable<VehicleViewModel> GetAll()
        {
            IQueryable<Vehicle> query = _databaseContext.Vehicles
                .Include(v => v.User);
            
            if (!_roleService.IsAdmin)
            {
                query = query
                    .Where(v => v.User.UserName == _httpContextAccessor.HttpContext.User.Identity.Name);
            }

            return query
                .Select(v => _mapper.Map<Vehicle, VehicleViewModel>(v))
                .ToList();
        }

        public IEnumerable<KeyValueViewModel> GetAllKeyValueForUser(string userId)
        {
            return _databaseContext.Vehicles
                .Include(v => v.User)
                .Where(v => v.User.Id == userId)
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
            if (viewModel.Id > 0)
            {
                var vehicle = _databaseContext.Vehicles
                    .Include(v => v.User)
                    .Include(v => v.JourneyVehicles)
                    .ThenInclude(jv => jv.Vehicle)
                    .Single(v => v.Id == viewModel.Id);

                if (vehicle.User.Id != viewModel.Company.Key && vehicle.JourneyVehicles.Any())
                {
                    modelState.AddModelError("_FORM", $"Pojazd jest przypisany do jednej z Tras firmy {vehicle.User.CompanyNameShort}. Nie można zmienić pola Firma.");
                }
            }

            if (!string.IsNullOrWhiteSpace(viewModel.RegistrationNumber))
            {
                var sameRegisterNumberVehicle = _databaseContext.Vehicles
                    .Where(v => v.Id != viewModel.Id)
                    .SingleOrDefault(v => v.RegistrationNumber.ToLower() == viewModel.RegistrationNumber.ToLower());

                if (sameRegisterNumberVehicle != null)
                {
                    modelState.AddModelError(nameof(VehicleViewModel.RegistrationNumber), "W bazie istnieje już pojazd z identycznym numerem rejestracyjnym.");
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
            UpdateUser(viewModel, vehicle);

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

        private void UpdateUser(VehicleViewModel viewModel, Vehicle vehicle)
        {
            vehicle.User = _databaseContext.Users.Single(u => u.Id == viewModel.Company.Key);
        }

        public VehicleViewModel Create()
        {
            var viewModel = new VehicleViewModel();
            
            var user = _databaseContext.Users.Single(u => u.UserName == _httpContextAccessor.HttpContext.User.Identity.Name);
            var company = new KeyValueViewModel { Key = user.Id, Value = user.CompanyNameShort };
            viewModel.Company = company;

            return viewModel;
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
