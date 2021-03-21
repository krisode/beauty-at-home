using System;
using System.Collections.Generic;

#nullable disable

namespace Infrastructure.Contexts
{
    public partial class Booking
    {
        public Booking()
        {
            BookingDetails = new HashSet<BookingDetail>();
        }

        public string Status { get; set; }
        public int Id { get; set; }
        public DateTime CreateDate { get; set; }
        public int CustomerAccountId { get; set; }
        public int BeautyArtistAccountId { get; set; }
        public string Note { get; set; }
        public string EndAddress { get; set; }
        public DateTime? UpdateDate { get; set; }
        public string BeginAddress { get; set; }
        public double? TotalFee { get; set; }
        public double? TransportFee { get; set; }

        public virtual Account BeautyArtistAccount { get; set; }
        public virtual Account CustomerAccount { get; set; }
        public virtual ICollection<BookingDetail> BookingDetails { get; set; }
    }
}
