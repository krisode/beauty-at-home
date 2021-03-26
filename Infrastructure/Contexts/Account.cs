using System;
using System.Collections.Generic;
using System.ComponentModel.DataAnnotations.Schema;

#nullable disable

namespace Infrastructure.Contexts
{
    public partial class Account
    {
        public Account()
        {
            Addresses = new HashSet<Address>();
            BookingBeautyArtistAccounts = new HashSet<Booking>();
            BookingCustomerAccounts = new HashSet<Booking>();
            Services = new HashSet<Service>();
        }

        public string Email { get; set; }
        public int Id { get; set; }
        public string DisplayName { get; set; }
        public string ServiceTypes { get; set; }
        public string Phone { get; set; }
        public string Role { get; set; }
        public string Status { get; set; }
        public int? GalleryId { get; set; }
        public bool? IsBeautyArtist { get; set; }

        public virtual Gallery Gallery { get; set; }
        public virtual ICollection<Address> Addresses { get; set; }
        public virtual ICollection<Booking> BookingBeautyArtistAccounts { get; set; }
        public virtual ICollection<Booking> BookingCustomerAccounts { get; set; }
        public virtual ICollection<Service> Services { get; set; }
    }
}
