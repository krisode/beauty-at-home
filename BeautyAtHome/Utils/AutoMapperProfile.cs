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
            CreateMap<ServicePagingSM, Service>();
            CreateMap<ServiceVM, Service>();
            CreateMap<ServiceCM, Service>();
            #endregion

            #region AutoMapper AccountViewModel
            CreateMap<Account, AccountPagingSM>();
            #endregion

            #region AutoMapper GalleryViewModel
            CreateMap<Gallery, GalleryPagingSM>();
            #endregion

            #region AutoMapper ServiceTypeViewModel
            CreateMap<ServiceType, ServiceTypePagingSM>();
            #endregion

            #region
            CreateMap<FeedBack, FeedBackVM>();
            CreateMap<FeedBackCM, FeedBack>();
            CreateMap<FeedBackVM, FeedBack>();
            #endregion

            #region
            CreateMap<Booking, BookingVM>();
            CreateMap<BookingCM, Booking>();
            #endregion

            #region
            CreateMap<BookingDetail, BookingDetailVM>();
            CreateMap<BookingDetailCM, BookingDetail>();
            #endregion
        }
    }
}
