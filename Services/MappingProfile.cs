﻿using System.Linq;
using Atut.Models;
using Atut.ViewModels;
using AutoMapper;

namespace Atut.Services
{
    public class MappingProfile : Profile
    {
        public MappingProfile()
        {
            CreateMap<Vehicle, KeyValueViewModel>()
                .ForMember(v => v.Key, member => member.MapFrom(v => v.Id))
                .ForMember(v => v.Value, member => member.MapFrom(v => $"{v.RegistrationNumber} - {v.Name}"));

            CreateMap<Vehicle, VehicleViewModel>()
                .ForMember(j => j.Company, member => member.MapFrom(s => new KeyValueViewModel
                {
                    Key = s.User.Id,
                    Value = s.User.CompanyNameShort
                }));
            CreateMap<VehicleViewModel, Vehicle>();

            CreateMap<Journey, JourneyViewModel>()
                .ForMember(j => j.Vehicles, member => member.MapFrom(s => s.JourneyVehicles.Select(jv => jv.Vehicle)))
                .ForMember(j => j.Company, member => member.MapFrom(s => new KeyValueViewModel
                {
                    Key = s.User.Id,
                    Value = s.User.CompanyNameShort
                }));
            CreateMap<JourneyViewModel, Journey>();

            CreateMap<Country, CountryViewModel>();
            CreateMap<CountryViewModel, Country>();

            CreateMap<Invoice, InvoiceViewModel>();
            CreateMap<InvoiceViewModel, Invoice>();

            CreateMap<VatNumber, VatNumberViewModel>();
            CreateMap<VatNumberViewModel, VatNumber>();
        }
    }
}
