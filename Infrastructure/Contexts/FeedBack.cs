using System;
using System.Collections.Generic;

#nullable disable

namespace Infrastructure.Contexts
{
    public partial class FeedBack
    {
        public int Id { get; set; }
        public double RateScore { get; set; }
        public int BookingDetailId { get; set; }
        public int GalleryId { get; set; }
        public string FeedbackContent { get; set; }
        public DateTime CreateDate { get; set; }

        public virtual BookingDetail BookingDetail { get; set; }
        public virtual Gallery Gallery { get; set; }
    }
}
