using System;
using System.ComponentModel.DataAnnotations;

namespace BeautyAtHome.ViewModels
{
    public class ServiceCM
    {
        public string Description { get; set; }
        public string ServiceName { get; set; }
        public decimal Price { get; set; }
        public int EstimateTime { get; set; }
        [Required]
        public int AccountId { get; set; }
        [Required]
        public int CategoryId { get; set; }
        public bool IsServiceCombo { get; set; }
        [Required]
        public int GalleryId { get; set; }
    }
    public class ServiceVM
    {
        [Required]
        public int Id { get; set; }
        public string Description { get; set; }
        public string ServiceName { get; set; }
        public DateTime CreatedDate { get; set; }
        public DateTime UpdatedDate { get; set; }
        public decimal Price { get; set; }
        public int EstimateTime { get; set; }
        [Required]
        public int AccountId { get; set; }
        [Required]
        public int CategoryId { get; set; }
        public string Status { get; set; }
        public bool IsServiceCombo { get; set; }
        [Required]
        public int GalleryId { get; set; }
    }

    public class ServiceSM
    {
        public int[] Id { get; set; }
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
        public int AccountId { get; set; }
        [Required]
        public int CategoryId { get; set; }
        public string Status { get; set; }
        public bool IsServiceCombo { get; set; }
        [Required]
        public int GalleryId { get; set; }
        public int PageSize { get; set; }
        public int PageIndex { get; set; }

    }

    public class ServiceUM
    {
        [Required]
        public int Id { get; set; }
        public string Description { get; set; }
        public string ServiceName { get; set; }
        public decimal Price { get; set; }
        public int EstimateTime { get; set; }
        public long AccountId { get; set; }
        public long CategoryId { get; set; }
        public bool IsServiceCombo { get; set; }
        public long GalleryId { get; set; }
    }

}
