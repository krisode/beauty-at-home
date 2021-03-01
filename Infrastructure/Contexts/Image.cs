using System;
using System.Collections.Generic;

#nullable disable

namespace Infrastructure.Contexts
{
    public partial class Image
    {
        public Image()
        {
            Galleries = new HashSet<Gallery>();
        }

        public int Id { get; set; }
        public string ImageUrl { get; set; }
        public string Description { get; set; }
        public int GalleryId { get; set; }
        public string ShareSetting { get; set; }

        public virtual Gallery Gallery { get; set; }
        public virtual ICollection<Gallery> Galleries { get; set; }
    }
}
