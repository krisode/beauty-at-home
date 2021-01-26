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
            CreateMap<Service, ServicePagingSM>();
            CreateMap<ServiceType, ServiceTypePagingSM>();
            CreateMap<Account, AccountPagingSM>();
            CreateMap<Gallery, GalleryPagingSM>();
            CreateMap<ServiceVM, Service>();
            CreateMap<ServiceCM, Service>();
            CreateMap<ServiceUM, Service>();
            #endregion
        }
    }
}
