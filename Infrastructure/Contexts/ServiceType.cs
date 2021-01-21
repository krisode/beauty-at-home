using System;
using System.Collections.Generic;

#nullable disable

namespace Infrastructure.Contexts
{
    public partial class ServiceType
    {
        public ServiceType()
        {
            Services = new HashSet<Service>();
        }

        public long Id { get; set; }
        public string Name { get; set; }
        public string Description { get; set; }
        public long GalleryId { get; set; }

        public virtual Gallery Gallery { get; set; }
        public virtual ICollection<Service> Services { get; set; }
    }
}
