
using Infrastructure.Contexts;
using Microsoft.AspNetCore.Http;
using System;
using System.Collections.Generic;
using System.ComponentModel.DataAnnotations;

namespace BeautyAtHome.ViewModels
{
    public class ServiceCM
    {
        public string Description { get; set; }
        public string ServiceName { get; set; }
        public decimal Price { get; set; }
        public int EstimateTime { get; set; }
        public int AccountId { get; set; }
        public int ServiceTypeId { get; set; }
        /*public int GalleryId { get; set; }*/
        public string Summary { get; set; }

        public IFormFile File { get; set; }
    }
    public class ServiceVM
    {

        public int Id { get; set; }
        public string Description { get; set; }
        public string Summary { get; set; }
        public string ServiceName { get; set; }
        public DateTime CreatedDate { get; set; }
        public DateTime UpdatedDate { get; set; }
        public decimal Price { get; set; }
        public int ServiceTypeId { get; set; }
        public int EstimateTime { get; set; }
        public int AccountId { get; set; }
        public string Status { get; set; }
        public int GalleryId { get; set; }

        public GalleryVM Gallery { get; set; }
    }

    public class ServiceSM
    {
        public string ServiceName { get; set; }
        public string Description { get; set; }
        public DateTime? CreatedAtMin { get; set; }
        public DateTime? CreatedAtMax { get; set; }
        public DateTime? UpdatedAtMin { get; set; }
        public DateTime? UpdatedAtMax { get; set; }
        public decimal LowerPrice { get; set; }
        public decimal UpperPrice { get; set; }
        public int LowerTime { get; set; }
        public int UpperTime { get; set; }
        public int AccountId { get; set; }
        public int ServiceTypeId { get; set; }
        public bool Status { get; set; }
        public int GalleryId { get; set; }

    }

    public class ServiceUM
    {
        public int Id { get; set; }
        public string Description { get; set; }
        public string Summary { get; set; }
        public string ServiceName { get; set; }
        public decimal Price { get; set; }
        public int EstimateTime { get; set; }
        public int AccountId { get; set; }
        public int ServiceTypeId { get; set; }
        public string Status { get; set; }
        public int GalleryId { get; set; }
        public IFormFile File { get; set; }
    }

    public class ServicePagingSM
    {
        public int Id { get; set; }
        public string Description { get; set; }
        public string ServiceName { get; set; }
        public string Summary { get; set; }
        public string Status { get; set; }
        public double RateScore { get; set; }
        public double TotalFeedback { get; set; }
        public decimal Price { get; set; }
        public int EstimateTime { get; set; }
        public DateTime CreatedDate { get; set; }
        public DateTime UpdatedDate { get; set; }
        public AccountPagingSM Account { get; set; }
        public GalleryPagingSM Gallery { get; set; }
        public ServiceTypePagingSM ServiceType { get; set; }
        public ICollection<BookingDetailVM> BookingDetails { get; set; }

    }


}
