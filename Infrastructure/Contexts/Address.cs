using System;
using System.Collections.Generic;

#nullable disable

namespace Infrastructure.Contexts
{
    public partial class Address
    {
        public Address()
        {
            Bookings = new HashSet<Booking>();
            Salons = new HashSet<Salon>();
        }

        public long Id { get; set; }
        public string Location { get; set; }
        public int Latitude { get; set; }
        public int Longtitude { get; set; }
        public long AccountId { get; set; }

        public virtual Account Account { get; set; }
        public virtual ICollection<Booking> Bookings { get; set; }
        public virtual ICollection<Salon> Salons { get; set; }
    }
}
