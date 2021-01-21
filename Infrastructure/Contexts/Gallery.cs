using System;
using System.Collections.Generic;

#nullable disable

namespace Infrastructure.Contexts
{
    public partial class Gallery
    {
        public Gallery()
        {
            Accounts = new HashSet<Account>();
            Bookings = new HashSet<Booking>();
            Images = new HashSet<Image>();
            ServiceTypes = new HashSet<ServiceType>();
            Services = new HashSet<Service>();
        }

        public long Id { get; set; }
        public string Name { get; set; }
        public string Description { get; set; }
        public long SalonId { get; set; }

        public virtual Salon Salon { get; set; }
        public virtual ICollection<Account> Accounts { get; set; }
        public virtual ICollection<Booking> Bookings { get; set; }
        public virtual ICollection<Image> Images { get; set; }
        public virtual ICollection<ServiceType> ServiceTypes { get; set; }
        public virtual ICollection<Service> Services { get; set; }
    }
}
