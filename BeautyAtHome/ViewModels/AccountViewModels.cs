using System;
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

    public class AccountSM
    {
        public string Email { get; set; }
        public string DisplayName { get; set; }
        public string Phone { get; set; }
        public string Role { get; set; }
        public string Status { get; set; }
        public int GalleryId { get; set; }
        public int DefaultAddressId { get; set; }
        public bool IsBeautyArtist { get; set; }
    }

    public class AccountUM
    {
        public string DisplayName { get; set; }
        public string Phone { get; set; }
        public string Status { get; set; }

    }

    public class AccountVM
    {
        public string Email { get; set; }
        public string DisplayName { get; set; }
        public string Phone { get; set; }
        public string Role { get; set; }
        public string Status { get; set; }
        public bool IsBeautyArtist { get; set; }
        public int DefaultAddressId { get; set; }
        public AddressPagingSM Address { get; set; }
        public GalleryPagingSM Gallery { get; set; }
        public ICollection<ServiceVM> Services { get; set; }
        

    }

    public class AccountPagingSM
    {
        public int Id { get; set; }
        public string DisplayName { get; set; }
    }

}
