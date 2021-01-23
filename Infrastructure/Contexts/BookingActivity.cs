using System;
using System.Collections.Generic;

#nullable disable

namespace Infrastructure.Contexts
{
    public partial class BookingActivity
    {
        public int Id { get; set; }
        public string Description { get; set; }
        public DateTime CreatedAt { get; set; }
        public int BookingId { get; set; }

        public virtual Booking Booking { get; set; }
    }
}
