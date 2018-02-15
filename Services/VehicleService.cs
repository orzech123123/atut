using System.Collections.Generic;
using System.Linq;
using Atut.Models;
using Atut.ViewModels;
using AutoMapper;
using Microsoft.AspNetCore.Http;

namespace Atut.Services
{
    public class VehicleService
    {
        private readonly DatabaseContext _databaseContext;
        private readonly IMapper _mapper;
        private readonly IHttpContextAccessor _httpContextAccessor;

        public VehicleService(
            DatabaseContext databaseContext,
            IMapper mapper,
            IHttpContextAccessor httpContextAccessor)
        {
            _databaseContext = databaseContext;
            _mapper = mapper;
            _httpContextAccessor = httpContextAccessor;
        }

        public IEnumerable<VehicleViewModel> GetAll()
        {
            return _databaseContext.Vehicles
                .Select(v => _mapper.Map<Vehicle, VehicleViewModel>(v))
                .ToList();
        }

        public VehicleViewModel GetOneById(int id)
        {
            var vehicle = _databaseContext.Vehicles.Find(id);
            var viewModel = _mapper.Map<Vehicle, VehicleViewModel>(vehicle);

            return viewModel;
        }

        public void Save(VehicleViewModel viewModel)
        {
            Vehicle vehicle;

            if (viewModel.Id > 0)
            {
                vehicle = _databaseContext.Vehicles.Find(viewModel.Id);
            }
            else
            {
                vehicle = new Vehicle();
            }

            _mapper.Map(viewModel, vehicle);

            vehicle.User = _databaseContext.Users.Single(u => u.Email == _httpContextAccessor.HttpContext.User.Identity.Name);

            if (vehicle.Id <= 0)
            {
                _databaseContext.Vehicles.Add(vehicle);
            }
        }
    }
}
