using System;
using System.Collections.Generic;

#nullable disable

namespace Infrastructure.Contexts
{
    public partial class Image
    {
        public long Id { get; set; }
        public string ImageUrl { get; set; }
        public string Description { get; set; }
        public long GalleryId { get; set; }

        public virtual Gallery Gallery { get; set; }
    }
}
