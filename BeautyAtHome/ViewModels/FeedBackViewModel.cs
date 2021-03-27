using Infrastructure.Contexts;
using Microsoft.AspNetCore.Http;
using System;
using System.Collections.Generic;
using System.ComponentModel.DataAnnotations;
using System.Linq;
using System.Threading.Tasks;

namespace BeautyAtHome.ViewModels
{
    public class FeedBackCM
    {
        public double RateScore { get; set; }
        public int BookingDetailId { get; set; }
        public int GalleryId { get; set; }
        public string FeedbackContent { get; set; }
        public IFormFile File { get; set; }

    }
    public class FeedBackVM 
    {
        [Required]
        public int Id { get; set; }
        public double RateScore { get; set; }
        public int BookingDetailId { get; set; }
        public GalleryVM Gallery { get; set; }
        public BookingDetailForFeedbackVM BookingDetail{ get; set; }
        public string FeedbackContent { get; set; }
        public DateTime CreateDate { get; set; }
    }

    public class FeedBackSM
    {
        //public int Id { get; set; } // k search feedBack theo ID
        public int ServiceId { get; set; }
        public int WorkerId { get; set; }
        public double RateScoreMin { get; set; }
        public double RateScoreMax { get; set; }
        public int BookingDetailId { get; set; }
        public int GalleryId { get; set; }
        public string FeedbackContent { get; set; }
        public DateTime? CreateDateAtMin { get; set; }
        public DateTime? CreateDateAtMax { get; set; }

        //public virtual BookingDetail BookingDetail { get; set; }
        //public virtual Gallery Gallery { get; set; }

    }

    public class FeedBackUM
    {
        [Required]
        public int Id { get; set; }
        public double RateScore { get; set; }
        public int BookingDetailId { get; set; }
        public int GalleryId { get; set; }
        public string FeedbackContent { get; set; }
        public DateTime CreateDate { get; set; }

        //public virtual BookingDetail BookingDetail { get; set; }
        //public virtual Gallery Gallery { get; set; }
    }

}
