using AutoMapper;
using BeautyAtHome.ViewModels;
using Infrastructure.Contexts;
using System.Collections.Generic;

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
            CreateMap<Account, AccountVM>();
            CreateMap<Account, AccountWithImageVM>();
            CreateMap<Account, AccountNewFirstVM>();
            CreateMap<AccountCM, Account>();
            CreateMap<AccountFormDataCM, Account>();
            #endregion

            #region AutoMapper GalleryViewModel
            CreateMap<Gallery, GalleryPagingSM>();
            CreateMap<Gallery, GalleryVM>();
            CreateMap<GalleryCM, Gallery>();
            #endregion

            #region AutoMapper ImageViewModel
            /*CreateMap<ICollection<Image>, ImagesPagingSM>();*/
            CreateMap<Image, ImageDefaultPagingSM>();
            CreateMap<ImageCM, Image>();
            CreateMap<Image, ImageVM>();
            CreateMap<ImageUM, Image>();
            #endregion

            #region AutoMapper ServiceTypeViewModel
            CreateMap<ServiceType, ServiceTypePagingSM>();
            CreateMap<ServiceTypeCM, ServiceType>();
            CreateMap<ServiceType, ServiceTypeVM>();
            #endregion

            #region AutoMapper FeedBackViewModel
            CreateMap<FeedBack, FeedBackVM>();
            CreateMap<FeedBackCM, FeedBack>();
            CreateMap<FeedBackVM, FeedBack>();
            #endregion

            #region AutoMapper BookingViewModel
            CreateMap<Booking, BookingVM>();
            CreateMap<Booking, BookingForFeedbackVM>();
            CreateMap<BookingCM, Booking>();
            #endregion

            #region AutoMapper BookingDetailViewModel
            CreateMap<BookingDetail, BookingDetailVM>();
            CreateMap<BookingDetailCM, BookingDetail>();
            CreateMap<BookingDetail, BookingDetailForFeedbackVM>();
            #endregion

            #region AutoMapper AddressViewModel
            CreateMap<Address, AddressPagingSM>();
            CreateMap<AddressCM, Address>();
            CreateMap<Address, AddressVM>();
            #endregion
        }
    }
}
