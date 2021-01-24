using AutoMapper;
using BeautyAtHome.ViewModels;
using Infrastructure.Contexts;

namespace BeautyAtHome.Utils
{
    public class AutoMapperProfile : Profile
    {
        public AutoMapperProfile()
        {
            #region AutoMapper ServiceViewModel
            CreateMap<ServiceCM, Service>();
            CreateMap<Service, ServiceVM>();
            CreateMap<ServiceVM, Service>();
            CreateMap<ServiceCM, Service>();
            CreateMap<ServiceUM, Service>();
            #endregion
        }
    }
}
