using System;
using System.Collections.Generic;

#nullable disable

namespace Infrastructure.Contexts
{
    public partial class FeedBack
    {
        public int Id { get; set; }
        public int ImageUrl { get; set; }
        public int RateScore { get; set; }
        public int BookingId { get; set; }

        public virtual Booking Booking { get; set; }
    }
}
