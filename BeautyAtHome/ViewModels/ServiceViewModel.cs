using System;
using System.ComponentModel.DataAnnotations;

namespace BeautyAtHome.ViewModels
{
    public class ServiceViewModelCM
    {
        public string Description { get; set; }
        public string ServiceName { get; set; }
        public decimal Price { get; set; }
        public int EstimateTime { get; set; }
        [Required]
        public long SalonId { get; set; }
        [Required]
        public long CategoryId { get; set; }
        public bool IsServiceCombo { get; set; }
        [Required]
        public long GalleryId { get; set; }
    }
    public class ServiceViewModelVM
    {
        [Required]
        public long Id { get; set; }
        public string Description { get; set; }
        public string ServiceName { get; set; }
        public DateTime CreatedDate { get; set; }
        public DateTime UpdatedDate { get; set; }
        public decimal Price { get; set; }
        public int EstimateTime { get; set; }
        [Required]
        public long SalonId { get; set; }
        
        public long CategoryId { get; set; }
        public string Status { get; set; }
        public bool IsServiceCombo { get; set; }
        [Required]
        public long GalleryId { get; set; }
    }

    public class ServiceViewModelSM
    {
        public long[] Id { get; set; }
        public string ServiceName { get; set; }
        public DateTime? CreatedAtMin { get; set; }
        public DateTime? CreatedAtMax { get; set; }
        public DateTime? UpdatedAtMin { get; set; }
        public DateTime? UpdatedAtMax { get; set; }
        public decimal LowerPrice { get; set; }
        public decimal UpperPrice { get; set; }
        public int LowerTime { get; set; }
        public int UpperTime { get; set; }
        [Required]
        public long SalonId { get; set; }
        [Required]
        public long CategoryId { get; set; }
        public string Status { get; set; }
        public bool IsServiceCombo { get; set; }
        [Required]
        public long GalleryId { get; set; }
        public int PageSize { get; set; }
        public int PageIndex { get; set; }

    }

    public class ServiceViewModelUM
    {
        [Required]
        public long Id { get; set; }
        public string Description { get; set; }
        public string ServiceName { get; set; }
        public decimal Price { get; set; }
        public int EstimateTime { get; set; }
        public long SalonId { get; set; }
        public long CategoryId { get; set; }
        public bool IsServiceCombo { get; set; }
        public long GalleryId { get; set; }
    }

}
