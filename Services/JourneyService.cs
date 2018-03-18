using System;
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

        public JourneyService(
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

        public IEnumerable<JourneyViewModel> GetAll()
        {
            return _databaseContext.Journeys
                .Include(v => v.User)
                .Where(v => v.User.UserName == _httpContextAccessor.HttpContext.User.Identity.Name)
                .Select(v => _mapper.Map<Journey, JourneyViewModel>(v))
                .ToList();
        }

        public JourneyViewModel GetOneById(int id)
        {
            var journey = _databaseContext.Journeys
                .Include(j => j.User)
                .Include(j => j.Countries)
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
        }

        public void Save(JourneyViewModel viewModel)
        {
            Journey journey;

            if (viewModel.Id > 0)
            {
                journey = _databaseContext.Journeys
                    .Include(j => j.Countries)
                    .Include(j => j.JourneyVehicles)
                    .Single(j => j.Id == viewModel.Id);
            }
            else
            {
                journey = new Journey();
            }

            _mapper.Map(viewModel, journey);

            journey.User = _databaseContext.Users.Single(u => u.UserName == _httpContextAccessor.HttpContext.User.Identity.Name);

            if (journey.Id <= 0)
            {
                _databaseContext.Journeys.Add(journey);
                _notificationManager.Add(NotificationType.Information, "Trasa została dodana.");
            }
            else
            {
                _notificationManager.Add(NotificationType.Information, "Trasa została zmodyfikowana.");
            }

            //            journey.JourneyVehicles.Add(new JourneyVehicle { Journey = journey, Vehicle = _databaseContext.Vehicles.First() });
            //            journey.JourneyVehicles.Clear();

//            foreach (var journeyJourneyVehicle in journey.JourneyVehicles.ToList())
//            {
//                _databaseContext.Entry(journeyJourneyVehicle).State = EntityState.Deleted;
//            }
//            journey.JourneyVehicles.Clear();
//            _databaseContext.JourneyVehicles.RemoveRange(journey.JourneyVehicles.ToList());
//
//            viewModel.Vehicles.ForEach(v => journey.JourneyVehicles.Add(new JourneyVehicle { Journey = journey, Vehicle = _databaseContext.Vehicles.Single(v2 => v2.Id == v.Id) }));

            foreach (var journeyVehicle in journey.JourneyVehicles.ToList())
            {
                if (viewModel.Vehicles.Any(v => v.Id != journeyVehicle.VehicleId))
                {
                    journey.JourneyVehicles.Remove(journeyVehicle);
                }
            }

            foreach (var vehicle in viewModel.Vehicles)
            {
                if (journey.JourneyVehicles.All(jv => jv.VehicleId != vehicle.Id))
                {
                    journey.JourneyVehicles.Add(new JourneyVehicle
                    {
                        Journey = journey,
                        Vehicle = _databaseContext.Vehicles.Single(v => v.Id == vehicle.Id)
                    });
                }
            }

//            //TODO test remove!!!
//            var country = new Country {Name = "Xxx", Distance = new Random(DateTime.Now.Second).Next(100, 100000)};
//            _databaseContext.Attach(country);
////            journey.Countries.Clear();
//            journey.Countries.Add(country);
        }

        public JourneyViewModel Create()
        {
            return new JourneyViewModel();
        }

        public void Delete(int id)
        {
            var journey = _databaseContext.Journeys.Find(id);

            //TODO czy ten user???

            _databaseContext.Journeys.Remove(journey);
            _notificationManager.Add(NotificationType.Information, "Trasa została usunięta.");
        }
    }
}
