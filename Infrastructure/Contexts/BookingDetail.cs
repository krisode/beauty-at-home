using System;
using System.Collections.Generic;

#nullable disable

namespace Infrastructure.Contexts
{
    public partial class BookingDetail
    {
        public BookingDetail()
        {
            FeedBacks = new HashSet<FeedBack>();
        }

        public int Quantity { get; set; }
        public int Id { get; set; }
        public int BookingId { get; set; }
        public int ServiceId { get; set; }
        public double? ServicePrice { get; set; }
        public string ServiceName { get; set; }

        public virtual Booking Booking { get; set; }
        public virtual Service Service { get; set; }
        public virtual ICollection<FeedBack> FeedBacks { get; set; }
    }
}
