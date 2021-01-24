using AutoMapper;
using BeautyAtHome.ViewModels;
using Infrastructure.Contexts;

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
