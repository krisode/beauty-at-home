using Infrastructure.Contexts;
using System;
using System.Collections.Generic;
using System.ComponentModel.DataAnnotations;
using System.Linq;
using System.Threading.Tasks;

namespace BeautyAtHome.ViewModels
{
    public class BookingDetailCM
    {
        public int Quantity { get; set; }
        //public int Id { get; set; }
        public int BookingId { get; set; }
        public int ServiceId { get; set; }
        public double? ServicePrice { get; set; }
        public string ServiceName { get; set; }

        //public virtual Booking Booking { get; set; }
        //public virtual Service Service { get; set; }
        //public virtual ICollection<FeedBack> FeedBacks { get; set; }

    }
    public class BookingDetailVM
    {
        [Required]
        public int Quantity { get; set; }
        public int Id { get; set; }
        public int BookingId { get; set; }
        public int ServiceId { get; set; }
        public double? ServicePrice { get; set; }
        public string ServiceName { get; set; }

        //public virtual Booking Booking { get; set; }
        //public virtual Service Service { get; set; }
        public BookingVM Booking { get; set; }
        public FeedBackVM FeedBack { get; set; }
    }

    public class BookingDetailSM
    {
        public int Id { get; set; }
        public int QuantityMin { get; set; }
        public int QuantityMax { get; set; }
        public int BookingId { get; set; }
        public int ServiceId { get; set; }
        public double ServicePriceMin { get; set; }
        public double ServicePriceMax { get; set; }
        public string ServiceName { get; set; }

        //public virtual Booking Booking { get; set; }
        //public virtual Service Service { get; set; }
        //public virtual FeedBackVM FeedBack { get; set; }

    }

    public class BookingDetailUM
    {
        [Required]
        public int Quantity { get; set; }
        public int Id { get; set; }
        public int BookingId { get; set; }
        public int ServiceId { get; set; }
        public double? ServicePrice { get; set; }
        public string ServiceName { get; set; }

        //public virtual Booking Booking { get; set; }
        //public virtual Service Service { get; set; }
        //public virtual FeedBackVM FeedBack { get; set; }
    }

    public class BookingDetailForFeedbackVM
    {
        public int Id { get; set; }
        public BookingForFeedbackVM Booking { get; set; }
        public int ServiceId { get; set; }
        public string ServiceName { get; set; }
    }
}
