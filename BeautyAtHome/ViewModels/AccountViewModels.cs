using Microsoft.AspNetCore.Http;
using System;
using System.Collections;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;

namespace BeautyAtHome.ViewModels
{
    public class AccountCM
    {
        public string DisplayName { get; set; }
        public string Phone { get; set; }
        public string Role { get; set; }
        public string Status { get; set; }
        public bool IsBeautyArtist { get; set; }
        public string Email { get; set; }
    }

    public class AccountFormDataCM
    {
        public string DisplayName { get; set; }
        public string Phone { get; set; }
        public string Email{ get; set; }
        public string Address { get; set; }
        public string ServiceTypes { get; set; }
        public ICollection<IFormFile> ImagesAvatar { get; set; }
        public ICollection<IFormFile> ImagesCertificates { get; set; }
    }

    public class AccountSM
    {
        public string Email { get; set; }
        public string DisplayName { get; set; }
        public string Phone { get; set; }
        public string Role { get; set; }
        public string Status { get; set; }
        public int GalleryId { get; set; }
        public bool IsBeautyArtist { get; set; }
    }

    public class AccountUM
    {
        public int Id { get; set; }
        public string DisplayName { get; set; }
        public string Phone { get; set; }
        public string Status { get; set; }
        public IFormFile File { get; set; }

    }

    public class AccountVM
    {
        public int Id { get; set; }
        public string Email { get; set; }
        public string DisplayName { get; set; }
        public string Phone { get; set; }
        public string Role { get; set; }
        public string Status { get; set; }
        public string ServiceTypes { get; set; }
        public bool IsBeautyArtist { get; set; }
        public double RateScore { get; set; }
        public int TotalFeedback { get; set; }
        public ICollection<AddressPagingSM> Addresses { get; set; }
        public GalleryPagingSM Gallery { get; set; }
        public ICollection<ServiceVM> Services { get; set; }
        
    }

    public class AccountPagingSM
    {
        public int Id { get; set; }
        public string DisplayName { get; set; }

        public GalleryPagingSM Gallery { get; set; }

    }

    public class AccountWithImageVM
    {
        public int Id { get; set; }
        public string DisplayName { get; set; }
        public GalleryPagingSM Gallery { get; set; }
    }

    public class AccountNewFirstVM
    {
        public int Id { get; set; }
        public string Email { get; set; }
        public string DisplayName { get; set; }
        public string Phone { get; set; }
        public string Status { get; set; }
        public bool IsBeautyArtist { get; set; }
        public double RateScore { get; set; }
        public int TotalFeedback { get; set; }
        public ICollection<AddressPagingSM> Addresses { get; set; }
        public GalleryPagingSM Gallery { get; set; }

    }

}
