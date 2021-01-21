using System;
using System.Collections.Generic;

#nullable disable

namespace Infrastructure.Contexts
{
    public partial class BookingDetail
    {
        public int Quantity { get; set; }
        public long Id { get; set; }
        public long BookingId { get; set; }
        public long ServiceId { get; set; }
        public string Price { get; set; }

        public virtual Booking Booking { get; set; }
        public virtual Service Service { get; set; }
    }
}
