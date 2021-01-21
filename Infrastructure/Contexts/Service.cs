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
            ServiceInComboServiceCombos = new HashSet<ServiceInCombo>();
            ServiceInComboServices = new HashSet<ServiceInCombo>();
        }

        public long Id { get; set; }
        public string Description { get; set; }
        public string ServiceName { get; set; }
        public DateTime CreatedDate { get; set; }
        public DateTime UpdatedDate { get; set; }
        public decimal Price { get; set; }
        public int EstimateTime { get; set; }
        public long SalonId { get; set; }
        public long CategoryId { get; set; }
        public string Status { get; set; }
        public bool IsServiceCombo { get; set; }
        public long GalleryId { get; set; }

        public virtual ServiceType Category { get; set; }
        public virtual Gallery Gallery { get; set; }
        public virtual Salon Salon { get; set; }
        public virtual ICollection<BookingDetail> BookingDetails { get; set; }
        public virtual ICollection<ServiceInCombo> ServiceInComboServiceCombos { get; set; }
        public virtual ICollection<ServiceInCombo> ServiceInComboServices { get; set; }
    }
}
