using System;
using System.Collections.Generic;

#nullable disable

namespace Infrastructure.Contexts
{
    public partial class Booking
    {
        public Booking()
        {
            BookingActivities = new HashSet<BookingActivity>();
            BookingDetails = new HashSet<BookingDetail>();
            FeedBacks = new HashSet<FeedBack>();
        }

        public string BookingType { get; set; }
        public string Status { get; set; }
        public int Id { get; set; }
        public DateTime CreateDate { get; set; }
        public int CustomerAccountId { get; set; }
        public int SalonMemberAccountId { get; set; }
        public int SalonOwnerAccountId { get; set; }
        public string Note { get; set; }
        public int AddressId { get; set; }
        public DateTime? UpdateDate { get; set; }
        public string FeedbackContent { get; set; }
        public double? RateScore { get; set; }
        public int? GalleryId { get; set; }

        public virtual Address Address { get; set; }
        public virtual Account CustomerAccount { get; set; }
        public virtual Gallery Gallery { get; set; }
        public virtual Account SalonMemberAccount { get; set; }
        public virtual ICollection<BookingActivity> BookingActivities { get; set; }
        public virtual ICollection<BookingDetail> BookingDetails { get; set; }
        public virtual ICollection<FeedBack> FeedBacks { get; set; }
    }
}
