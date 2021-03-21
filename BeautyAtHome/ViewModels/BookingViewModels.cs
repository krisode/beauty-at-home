using Infrastructure.Contexts;
using System;
using System.Collections.Generic;
using System.ComponentModel.DataAnnotations;
using System.Linq;
using System.Threading.Tasks;

namespace BeautyAtHome.ViewModels
{
    public class BookingCM
    {
        public string BookingType { get; set; }
        public string Status { get; set; }
        //public DateTime CreateDate { get; set; }
        public int CustomerAccountId { get; set; }
        public int BeautyArtistAccountId { get; set; }
        public string Note { get; set; }
        public string EndAddress { get; set; }
       // public DateTime? UpdateDate { get; set; }
        public string BeginAddress { get; set; }
        public double? TotalFee { get; set; }
        public double? TransportFee { get; set; }

    }
    public class BookingVM
    {
        [Required]
        public int Id { get; set; }
        public string BookingType { get; set; }
        public string Status { get; set; }
        public DateTime CreateDate { get; set; }
        public int CustomerAccountId { get; set; } 
        public int BeautyArtistAccountId { get; set; } 
        public string Note { get; set; } 
        public string EndAddress { get; set; } 
        public DateTime? UpdateDate { get; set; } 
        public string BeginAddress { get; set; }
        public double? TotalFee { get; set; }
        public double? TransportFee { get; set; }

        //public virtual Account BeautyArtistAccount { get; set; }
        //public virtual Account CustomerAccount { get; set; }
        //public virtual Gallery Gallery { get; set; }
        //public virtual ICollection<BookingDetail> BookingDetails { get; set; }
    }

    public class BookingSM
    {
        public string BookingType { get; set; }
        public string Status { get; set; }
        public int Id { get; set; }
        public DateTime? CreateDateMin { get; set; }
        public DateTime? CreateDateMax { get; set; }
        public int CustomerAccountId { get; set; }
        public int BeautyArtistAccountId { get; set; }
        public string Note { get; set; }
        public string EndAddress { get; set; }
        public DateTime? UpdateDateMin { get; set; }
        public DateTime? UpdateDateMax { get; set; }
        public string BeginAddress { get; set; }
        public double? TotalFee { get; set; }
        public double? TransportFee { get; set; }

    }

    public class BookingUM
    {
        public string Status { get; set; }
        public int Id { get; set; }

    }
}
