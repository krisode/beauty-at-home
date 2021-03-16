using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;

namespace BeautyAtHome.ViewModels
{
    public class ImageCM
    {
        public string ImageUrl { get; set; }
        public string Description { get; set; }
        public int GalleryId { get; set; }
    }

    public class ImageVM
    {
        public int Id { get; set; }
        public string ImageUrl { get; set; }
        public string Description { get; set; }
        public GalleryPagingSM Gallery { get; set; }
    }

    public class ImageSM
    {
        public string ImageUrl { get; set; }
        public string Description { get; set; }
        public int GalleryId { get; set; }
    }

    public class ImageUM
    {
        public int Id { get; set; }
        public string ImageUrl { get; set; }
        public string Description { get; set; }
        public int GalleryId { get; set; }
    }


}
