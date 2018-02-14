using System.Collections.Generic;
using System.Linq;
using Atut.Models;
using Atut.ViewModels;
using AutoMapper;

namespace Atut.Services
{
    public class VehicleService
    {
        private readonly DatabaseContext _databaseContext;
        private readonly IMapper _mapper;

        public VehicleService(
            DatabaseContext databaseContext,
            IMapper mapper)
        {
            _databaseContext = databaseContext;
            _mapper = mapper;
        }

        public IEnumerable<VehicleViewModel> GetAll()
        {
            return _databaseContext.Vehicles
                .Select(v => _mapper.Map<Vehicle, VehicleViewModel>(v))
                .ToList();
        }
    }
}
