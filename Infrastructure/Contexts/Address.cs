using System;
using System.Collections.Generic;

#nullable disable

namespace Infrastructure.Contexts
{
    public partial class Address
    {
        public Address()
        {
            Accounts = new HashSet<Account>();
            Bookings = new HashSet<Booking>();
        }

        public int Id { get; set; }
        public string Location { get; set; }
        public int Latitude { get; set; }
        public int Longtitude { get; set; }
        public int AccountId { get; set; }

        public virtual Account Account { get; set; }
        public virtual ICollection<Account> Accounts { get; set; }
        public virtual ICollection<Booking> Bookings { get; set; }
    }
}
