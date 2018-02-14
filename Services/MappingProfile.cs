using Atut.Models;
using Atut.ViewModels;
using AutoMapper;

namespace Atut.Services
{
    public class MappingProfile : Profile
    {
        public MappingProfile()
        {
            CreateMap<Vehicle, VehicleViewModel>();
            CreateMap<VehicleViewModel, Vehicle>();
        }
    }
}
