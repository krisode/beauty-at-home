using AutoMapper;
using BeautyAtHome.ViewModels;
using Infrastructure.Contexts;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;

namespace BeautyAtHome.Utils
{
    public class AutoMapperProfile : Profile
    {
        public AutoMapperProfile()
        {
            CreateMap<ServiceViewModelCM, Service>();
            CreateMap<Service, ServiceViewModelVM>();
            CreateMap<ServiceViewModelVM, Service>();
        }
    }
}
