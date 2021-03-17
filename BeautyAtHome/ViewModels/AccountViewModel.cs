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
        public String Role { get; set; }
        public string Status { get; set; }
        public bool IsBeautyArtist { get; set; }
        public string Email { get; set; }
    }

    public class AccountUM
    {
        public string DisplayName { get; set; }
        public string Phone { get; set; }
        public string Status { get; set; }
        
    }

    public class AccountVM
    {
        public string DisplayName { get; set; }
        public string Phone { get; set; }
        public String Role { get; set; }
        public string Status { get; set; }
        public bool IsBeautyArtist { get; set; }
        public string Email { get; set; }
        public int DefaultAddressId { get; set; }
        public int GalleryId { get; set; }
    }
}
