using System;
using System.Collections.Generic;

#nullable disable

namespace Infrastructure.Contexts
{
    public partial class Service
    {
        public Service()
        {
            BookingDetails = new HashSet<BookingDetail>();
        }

        public int Id { get; set; }
        public string Description { get; set; }
        public string ServiceName { get; set; }
        public DateTime CreatedDate { get; set; }
        public DateTime? UpdatedDate { get; set; }
        public decimal Price { get; set; }
        public int EstimateTime { get; set; }
        public int AccountId { get; set; }
        public int ServiceTypeId { get; set; }
        public string Status { get; set; }
        public int? GalleryId { get; set; }
        public string Summary { get; set; }

        public virtual Account Account { get; set; }
        public virtual Gallery Gallery { get; set; }
        public virtual ServiceType ServiceType { get; set; }
        public virtual ICollection<BookingDetail> BookingDetails { get; set; }
    }
}
