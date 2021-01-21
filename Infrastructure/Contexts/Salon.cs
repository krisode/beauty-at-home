using System;
using System.Collections.Generic;

#nullable disable

namespace Infrastructure.Contexts
{
    public partial class Salon
    {
        public Salon()
        {
            Accounts = new HashSet<Account>();
            Bookings = new HashSet<Booking>();
            Galleries = new HashSet<Gallery>();
            Services = new HashSet<Service>();
        }

        public long Id { get; set; }
        public string SalonType { get; set; }
        public long AddressId { get; set; }

        public virtual Address Address { get; set; }
        public virtual ICollection<Account> Accounts { get; set; }
        public virtual ICollection<Booking> Bookings { get; set; }
        public virtual ICollection<Gallery> Galleries { get; set; }
        public virtual ICollection<Service> Services { get; set; }
    }
}
